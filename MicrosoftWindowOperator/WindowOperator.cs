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
        }

        /// <summary>
        /// Used to restore the current window layout to the state specified in the parameter.
        /// </summary>
        /// <param name="layoutConfiguration"></param>
        /// <returns></returns>
        public bool ApplyLayoutConfiguration(TopLevelWindow[] layoutConfiguration)
        {
            foreach (TopLevelWindow window in layoutConfiguration.Reverse())
            {
                uint foregroundPid;
                uint foreThread = WinApiUtil.GetWindowThreadProcessId(WinApiUtil.GetForegroundWindow(), out foregroundPid);
                uint appThread = WinApiUtil.GetCurrentThreadId();
                WinApiUtil.WINDOWPLACEMENT placement = (window.Placement as WinApiUtil.WindowPlacement).GetStruct();

                if (foreThread != appThread)
                    WinApiUtil.AttachThreadInput(foreThread, appThread, true);

                for (int i = 0; i < 10 && !WinApiUtil.SetWindowPlacement((IntPtr)window.ID, ref placement); i++)
                    Console.WriteLine("Fail to set foreground");

                for (int i = 0; i < 10 && !WinApiUtil.SetForegroundWindow((IntPtr)window.ID); i++)
                    Console.WriteLine("Fail to set foreground");
                
                if (foreThread != appThread)
                    WinApiUtil.AttachThreadInput(foreThread, appThread, false);
                   
            }

            return true;
        }

        /// <summary>
        /// Get the current layout of the windows.
        /// </summary>
        /// <param name="blacklist"></param>
        /// <returns></returns>
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
                //WinApiUtil.RECT positionPlacement = new WinApiUtil.RECT();
                WinApiUtil.WINDOWPLACEMENT placement = new WinApiUtil.WINDOWPLACEMENT();

                try
                {
                    if (WinApiUtil.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false && vdm.IsWindowOnCurrentVirtualDesktop(hWnd) && WinApiUtil.GetWindowPlacement(hWnd, ref placement))
                    {
                        uint pid = 0;
                        WinApiUtil.GetWindowThreadProcessId(hWnd, out pid);
                        Process process = Process.GetProcessById((int)pid);
                        if (Process.GetCurrentProcess().Id != (int)pid)
                        {
                            TopLevelWindow w = new TopLevelWindow(process.ToString(), strTitle, (int)hWnd, new WinApiUtil.WindowPlacement(placement), zIndex);
                            windows.Add(w);
                        }
                        else
                        {
                            Console.WriteLine("Found myself");
                        }
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
