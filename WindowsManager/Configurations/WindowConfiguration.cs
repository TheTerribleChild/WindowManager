using System;
using System.Collections.Generic;
using SystemWindowOperator;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    public class WindowConfiguration
    {
        public WindowInfo Window { get; private set; }
        public string ApplicationName
        {
            get
            {
                return Window.ApplicationName;
            }
        }

        public int Key {
            get
            {
                return Window.ArchiveID;
            }
        }

        public WindowPlacement Placement
        {
            get
            {
                return Window.Placement;
            }
        }

        public List<int> TitleHistory { get; private set; }

        public WindowConfiguration(WindowInfo window)
        {
            this.Window = new WindowInfo(window.ApplicationName, window.Placement, window.Z);
            this.TitleHistory = new List<int>();
        }

        public void AddTitleToHistory(string windowTitle)
        {
            TitleHistory.Add(windowTitle.GetHashCode());
        }
    }

    public static class WindowConfigurationFactory
    {

    }
}
