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

        public WindowOperator()
        {
            //Console.WriteLine();
        }

        public bool ApplyLayoutConfiguration(TopLevelWindow[] layoutConfiguration)
        {
            

            for (int i = layoutConfiguration.Length-1; i >= 0; i--)
            {
                TopLevelWindow window = layoutConfiguration[i];
                
                uint pid, foregroundPid;
                WinApiUtil.GetWindowThreadProcessId((IntPtr)window.ID, out pid);
                if ((int)pid == Process.GetCurrentProcess().Id)
                    continue;

                uint foreThread = WinApiUtil.GetWindowThreadProcessId(WinApiUtil.GetForegroundWindow(), out foregroundPid);
                uint appThread = WinApiUtil.GetCurrentThreadId();

                WinApiUtil.FLASHWINFO flashInfo = new WinApiUtil.FLASHWINFO();
                flashInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(flashInfo));
                flashInfo.dwFlags = WinApiUtil.FLASHW_STOP;
                flashInfo.uCount = UInt32.MaxValue;
                flashInfo.dwTimeout = 0;
                flashInfo.hwnd = (IntPtr)window.ID;

                if (foreThread != appThread)
                    WinApiUtil.AttachThreadInput(foreThread, appThread, true);

                switch (window.State)
                {
                    case TopLevelWindow.WindowState.NORMAL:
                        WinApiUtil.ShowWindowAsync((IntPtr)window.ID, 1);
                        WinApiUtil.SetWindowPos((IntPtr)window.ID, (IntPtr)WinApiUtil.HWND_TOP, window.Placement.X, window.Placement.Y, window.Placement.Width, window.Placement.Height, WinApiUtil.SWP_SHOWWINDOW);
                        WinApiUtil.SetForegroundWindow((IntPtr)window.ID);
                        break;
                    case TopLevelWindow.WindowState.MINIMIZED:
                        WinApiUtil.ShowWindowAsync((IntPtr)window.ID, 2);
                        break;
                    case TopLevelWindow.WindowState.MAXIMIZED:
                        WinApiUtil.SetWindowPos((IntPtr)window.ID, (IntPtr)WinApiUtil.HWND_TOP, window.Placement.X, window.Placement.Y, window.Placement.Width, window.Placement.Height, WinApiUtil.SWP_SHOWWINDOW);
                        WinApiUtil.ShowWindowAsync((IntPtr)window.ID, 3);
                        WinApiUtil.SetForegroundWindow((IntPtr)window.ID);
                        break;
                }

                WinApiUtil.FlashWindowEx(ref flashInfo);


                if (foreThread != appThread)
                    WinApiUtil.AttachThreadInput(foreThread, appThread, false);
            }

            return true;
        }

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
                WinApiUtil.RECT positionPlacement = new WinApiUtil.RECT();
                
                try
                {
                    if (WinApiUtil.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false && vdm.IsWindowOnCurrentVirtualDesktop(hWnd) && WinApiUtil.GetWindowRect(hWnd, ref positionPlacement))
                    {
                        uint pid = 0;
                        TopLevelWindow.WindowState windowState = TopLevelWindow.WindowState.NORMAL;
                        WinApiUtil.GetWindowThreadProcessId(hWnd, out pid);
                        Process process = Process.GetProcessById((int)pid);
                        WinApiUtil.WINDOWPLACEMENT placement = new WinApiUtil.WINDOWPLACEMENT();
                        WinApiUtil.GetWindowPlacement(hWnd, ref placement);
                        switch (placement.showCmd)
                        {
                            case 1:
                                windowState = TopLevelWindow.WindowState.NORMAL;
                                break;
                            case 2:
                                windowState = TopLevelWindow.WindowState.MINIMIZED;
                                break;
                            case 3:
                                windowState = TopLevelWindow.WindowState.MAXIMIZED;
                                break;
                        }
                        TopLevelWindow w = new TopLevelWindow(process.ToString(), strTitle, (int)hWnd, new Rectangle(positionPlacement.Left, positionPlacement.Top, positionPlacement.Right - positionPlacement.Left, 
                            positionPlacement.Bottom - positionPlacement.Top), zIndex, windowState);
                        windows.Add(w);
                    }
                }catch(System.Runtime.InteropServices.COMException e)
                {
                }catch(Exception e)
                {
                    Console.WriteLine(e);
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
