using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace MicrosoftWindowOperator
{
    public class WindowOperator : IWindowOperator
    {
        /// <summary>
        /// Used to restore the current window layout to the state specified in the parameter.
        /// </summary>
        /// <param name="layoutConfiguration"></param>
        /// <returns>Return whether the call was successful or not</returns>
        public bool ApplyLayoutConfiguration(TopLevelWindow[] windowsLayout)
        {
            if (windowsLayout.Length == 0)
                return false;

            TopLevelWindow[] sortedLayoutConfiguration = (from window in windowsLayout orderby window.Z descending select window).ToArray<TopLevelWindow>();
            bool successful = true;
            uint foregroundPid;
            uint foreThread = WinApiUtil.GetWindowThreadProcessId(WinApiUtil.GetForegroundWindow(), out foregroundPid);
            uint appThread = WinApiUtil.GetCurrentThreadId();

            WinApiUtil.AllowSetForegroundWindow(Process.GetCurrentProcess().Id);
            if (foreThread != appThread)
                WinApiUtil.AttachThreadInput(foreThread, appThread, true);

            try
            {
                foreach (TopLevelWindow window in sortedLayoutConfiguration)
                {
                    WinApiUtil.WINDOWPLACEMENT placement = (window.Placement as WindowPlacement).Placement;

                    if(!WinApiUtil.SetWindowPlacement((IntPtr)window.ID, ref placement))
                    {
                        int retries;
                        for (retries = 0; retries < WinApiUtil.API_MAX_RETRY && !WinApiUtil.SetWindowPlacement((IntPtr)window.ID, ref placement); retries++)
                            Thread.Sleep(10);

                        if (retries == WinApiUtil.API_MAX_RETRY)
                        {
                            Console.Error.WriteLine("Failed to set placement: " + window);
                            continue;
                        }
                    }

                    //If window is minimized, no need to bring it to foreground.
                    if (window.Placement.WindowState != WindowPlacementState.Minimized && !WinApiUtil.SetForegroundWindow((IntPtr)window.ID))
                    {
                        int retries;
                        for (retries = 0; retries < WinApiUtil.API_MAX_RETRY && !WinApiUtil.SetForegroundWindow((IntPtr)window.ID); retries++)
                            Thread.Sleep(10);

                        if (retries == WinApiUtil.API_MAX_RETRY)
                        {
                            //Used to stop applications in task bar from flashing in case SetForegroundWindow fails
                            WinApiUtil.FLASHWINFO flashInfo = new WinApiUtil.FLASHWINFO();
                            flashInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(flashInfo));
                            flashInfo.dwFlags = WinApiUtil.FLASHW_STOP;
                            flashInfo.uCount = 0;
                            flashInfo.dwTimeout = 0;
                            flashInfo.hwnd = (IntPtr)window.ID;
                            WinApiUtil.FlashWindowEx(ref flashInfo);

                            Console.Error.WriteLine("Failed to set foreground: " + window);
                        }
                    }

                    Thread.Sleep(10);
                }
            }catch(Exception e)
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
        /// <param name="blacklist">specify which application to block from reading</param>
        /// <returns>Returns a list of current TopLevelWindow</returns>
        public TopLevelWindow[] GetTopLevelWindow(string[] blacklist)
        {
            List<TopLevelWindow> windows = new List<TopLevelWindow>();
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
                        TopLevelWindow w = new TopLevelWindow(process.ToString(), strTitle, (int)hWnd, new WindowPlacement(placement), zIndex);
                        windows.Add(w);
                    }
                }catch(System.Runtime.InteropServices.COMException e)
                {
                }catch(Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                zIndex++;
                return true;
            };

            if (WinApiUtil.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
                return windows.ToArray<TopLevelWindow>();

            return null;
        }
    }
}
