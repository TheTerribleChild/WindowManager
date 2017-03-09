using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace MicrosoftWindowOperator
{
    class MSWindowOperator
    {
        public bool ApplyLayoutConfiguration(MappedLayout mappedLayout)
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
                foreach (MappedWindow window in mappedLayout.Mapping)
                {
                    WinApiUtil.WINDOWPLACEMENT placement = (window.Configuration.Placement as MSWindowPlacement).Placement;
                    IntPtr handle = (IntPtr)window.Handle;

                    if (WinApiUtil.IsWindowVisible(handle))
                        continue;

                    if (!WinApiUtil.SetWindowPlacement(handle, ref placement))
                    {
                        int retries;
                        for (retries = 0; retries < WinApiUtil.API_MAX_RETRY && !WinApiUtil.SetWindowPlacement(handle, ref placement); retries++)
                            Thread.Sleep(10);

                        if (retries == WinApiUtil.API_MAX_RETRY)
                        {
                            Console.Error.WriteLine("Failed to set placement: " + window);
                            continue;
                        }
                    }

                    //If window is minimized, no need to bring it to foreground.
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

                    Thread.Sleep(10);
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
        public MappedLayout GetTopLevelWindow()
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
                    if (WinApiUtil.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false && vdm.IsWindowOnCurrentVirtualDesktop(hWnd) && WinApiUtil.GetWindowPlacement(hWnd, ref placement))
                    {
                        uint pid = 0;
                        WinApiUtil.GetWindowThreadProcessId(hWnd, out pid);
                        Process process = Process.GetProcessById((int)pid);
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
    }
}
