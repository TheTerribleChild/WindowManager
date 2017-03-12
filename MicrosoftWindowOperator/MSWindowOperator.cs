using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemWindowOperator;
using System.Management;

namespace MicrosoftWindowOperator
{
    class MSWindowOperator : IWindowOperator
    {
        public bool ApplyMappedLayout(MappedLayout mappedLayout)
        {
            if (mappedLayout.Mapping.Count == 0)
                return false;

            mappedLayout.SortByZ();

            bool successful = true;
            uint foregroundPid;
            uint foreThread = WinApiUtil.GetWindowThreadProcessId(WinApiUtil.GetForegroundWindow(), out foregroundPid);
            uint appThread = WinApiUtil.GetCurrentThreadId();

            WinApiUtil.AllowSetForegroundWindow(Process.GetCurrentProcess().Id);
            if (foreThread != appThread)
                WinApiUtil.AttachThreadInput(foreThread, appThread, true);

            try
            {
                foreach (MappedWindow window in mappedLayout.Mapping.Reverse<MappedWindow>())
                {
                    Console.WriteLine(window.Configuration.ApplicationName);
                    WinApiUtil.WINDOWPLACEMENT placement = (window.Configuration.Placement as MSWindowPlacement).Placement;
                    IntPtr handle = (IntPtr)window.Handle;

                    if (!WinApiUtil.IsWindowVisible(handle))
                        continue;

                    if (!WinApiUtil.SetWindowPlacement(handle, ref placement))
                    {
                        int retries;
                        for (retries = 0; retries < WinApiUtil.API_MAX_RETRY && !WinApiUtil.SetWindowPlacement(handle, ref placement); retries++)
                            Thread.Sleep(10);

                        if (retries == WinApiUtil.API_MAX_RETRY)
                        {
                            Console.Error.WriteLine("Failed to set placement: " + window.Configuration.ApplicationName);
                            continue;
                        }
                    }

                    //If window is minimized, no need to bring it to foreground.
                    //do
                    //{
                    if (window.Configuration.Placement.WindowState != WindowPlacementState.Minimized && !WinApiUtil.SetForegroundWindow(handle))
                    {
                        int retries;
                        for (retries = 0; retries < WinApiUtil.API_MAX_RETRY && !WinApiUtil.SetForegroundWindow(handle); retries++)
                            Thread.Sleep(10);

                        if (retries == WinApiUtil.API_MAX_RETRY)
                        {
                            //Used to stop applications in task bar from flashing in case SetForegroundWindow fails
                            WinApiUtil.FLASHWINFO flashInfo = new WinApiUtil.FLASHWINFO();
                            flashInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(flashInfo));
                            flashInfo.dwFlags = WinApiUtil.FLASHW_STOP;
                            flashInfo.uCount = 0;
                            flashInfo.dwTimeout = 0;
                            flashInfo.hwnd = handle;
                            WinApiUtil.FlashWindowEx(ref flashInfo);

                            Console.Error.WriteLine("Failed to set foreground: " + window);
                        }
                    }
                    //} while (!IsHandleInForeground(handle));
                    
                    //WinApiUtil.SendMessage(handle, WinApiUtil.WmPaint, IntPtr.Zero, IntPtr.Zero);
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                successful = false;
            }

            if (foreThread != appThread)
                WinApiUtil.AttachThreadInput(foreThread, appThread, false);

            return successful;
        }

        /// <summary>
        /// Get the current layout of the windows.
        /// </summary>
        /// <returns>Returns a list of current TopLevelWindow</returns>
        public MappedLayout GetCurrentMappedLayout()
        {
            MappedLayout mapping = new MappedLayout();
            VirtualDesktopManager vdm = new VirtualDesktopManager();
            int zIndex = 0;

            WinApiUtil.EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = WinApiUtil.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();
                WinApiUtil.WINDOWPLACEMENT placement = new WinApiUtil.WINDOWPLACEMENT();

                try
                {

                    if (WinApiUtil.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false && vdm.IsWindowOnCurrentVirtualDesktop(hWnd) && WinApiUtil.GetWindowPlacement(hWnd, ref placement) && !IsInvisibleWin10BackgroundAppWindow(hWnd))
                    {
                        uint pid = 0;
                        WinApiUtil.GetWindowThreadProcessId(hWnd, out pid);
                        Process process = Process.GetProcessById((int)pid);
                        IsInvisibleWin10BackgroundAppWindow(hWnd);
                        TopLevelWindow w = new TopLevelWindow(process.ProcessName, strTitle, (int)hWnd, new MSWindowPlacement(placement), zIndex);
                        mapping.AddMapping(hWnd, new WindowConfiguration(process.ProcessName, new MSWindowPlacement(placement), zIndex), strTitle);
                        zIndex++;
                    }
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }

                return true;
            };

            if (WinApiUtil.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
                return mapping;

            return null;
        }

        private bool IsInvisibleWin10BackgroundAppWindow(IntPtr hWnd)
        {
            int CloakedVal;
            int hRes = WinApiUtil.DwmGetWindowAttribute(hWnd, (int)WinApiUtil.DwmWindowAttribute.DWMWA_CLOAKED, out CloakedVal, sizeof(int));
            return CloakedVal != 0;
        }

        private bool IsHandleInForeground(IntPtr hWnd)
        {
            MappedLayout layout = GetCurrentMappedLayout();
            layout.SortByZ();
            return (IntPtr)layout.Mapping[0].Handle == hWnd;
        }

        private void GetProcessStat(Process process)
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var result in searcher.Get())
                {
                    Console.WriteLine("=========");
                    
                    Console.WriteLine("CreationClassName:        " + result["CreationClassName"]);
                    Console.WriteLine("Caption:                  " + result["Caption"]);
                    Console.WriteLine("CommandLine:              " + result["CommandLine"]);
                    Console.WriteLine("CreationDate:             " + result["CreationDate"]);
                    Console.WriteLine("CSCreationClassName:      " + result["CSCreationClassName"]);
                    Console.WriteLine("CSName:                   " + result["CSName"]);
                    Console.WriteLine("Description:              " + result["Description"]);
                    Console.WriteLine("ExecutablePath:           " + result["ExecutablePath"]);
                    Console.WriteLine("ExecutionState:           " + result["ExecutionState"]);
                    Console.WriteLine("Handle:                   " + result["Handle"]);
                    Console.WriteLine("HandleCount:              " + result["HandleCount"]);
                    Console.WriteLine("InstallDate:              " + result["InstallDate"]);
                    Console.WriteLine("KernelModeTime:           " + result["KernelModeTime"]);
                    Console.WriteLine("MaximumWorkingSetSize:    " + result["MaximumWorkingSetSize"]);
                    Console.WriteLine("MinimumWorkingSetSize:    " + result["MinimumWorkingSetSize"]);
                    Console.WriteLine("Name:                     " + result["Name"]);
                    Console.WriteLine("OSCreationClassName:      " + result["OSCreationClassName"]);
                    Console.WriteLine("OSName:                   " + result["OSName"]);
                    Console.WriteLine("OtherOperationCount:      " + result["OtherOperationCount"]);
                    Console.WriteLine("OtherTransferCount:       " + result["OtherTransferCount"]);
                    Console.WriteLine("PageFaults:               " + result["PageFaults"]);
                    Console.WriteLine("PageFileUsage:            " + result["PageFileUsage"]);
                    Console.WriteLine("ParentProcessId:          " + result["ParentProcessId"]);
                    Console.WriteLine("PeakPageFileUsage:        " + result["PeakPageFileUsage"]);
                    Console.WriteLine("PeakVirtualSize:          " + result["PeakVirtualSize"]);
                    Console.WriteLine("PeakWorkingSetSize:       " + result["PeakWorkingSetSize"]);
                    Console.WriteLine("Priority:                 " + result["Priority"]);
                    Console.WriteLine("PrivatePageCount:         " + result["PrivatePageCount"]);
                    Console.WriteLine("ProcessId:                " + result["ProcessId"]);
                    Console.WriteLine("QuotaNonPagedPoolUsage:   " + result["QuotaNonPagedPoolUsage"]);
                    Console.WriteLine("QuotaPagedPoolUsage:      " + result["QuotaPagedPoolUsage"]);
                    Console.WriteLine("QuotaPeakNonPagedPoolUse: " + result["QuotaPeakNonPagedPoolUsage"]);
                    Console.WriteLine("QuotaPeakPagedPoolUsage:  " + result["QuotaPeakPagedPoolUsage"]);
                    Console.WriteLine("ReadOperationCount:       " + result["ReadOperationCount"]);
                    Console.WriteLine("ReadTransferCount:        " + result["ReadTransferCount"]);
                    Console.WriteLine("SessionId:                " + result["SessionId"]);
                    Console.WriteLine("Status:                   " + result["Status"]);
                    Console.WriteLine("TerminationDate:          " + result["TerminationDate"]);
                    Console.WriteLine("ThreadCount:              " + result["ThreadCount"]);
                    Console.WriteLine("UserModeTime:             " + result["UserModeTime"]);
                    Console.WriteLine("VirtualSize:              " + result["VirtualSize"]);
                    Console.WriteLine("WindowsVersion:           " + result["WindowsVersion"]);
                    Console.WriteLine("WorkingSetSize:           " + result["WorkingSetSize"]);
                    Console.WriteLine("WriteOperationCount:      " + result["WriteOperationCount"]);
                    Console.WriteLine("WriteTransferCount:       " + result["WriteTransferCount"]);
                    Console.WriteLine("=========");
                }
            }

        }
    }
}
