using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    internal class WindowConfiguration
    {
        internal string ProcessName { get; private set; }
        internal Rectangle WindowPlacement { get; private set; }
        internal int Key { get; private set; }
        internal List<int> TitleHistory { get; private set; }

        internal WindowConfiguration(string processName, Rectangle windowPlacement)
        {
            this.ProcessName = processName;
            this.WindowPlacement = windowPlacement;
            this.Key = String.Format("{0}{1}", processName, WindowPlacement.ToString()).GetHashCode();
            this.TitleHistory = new List<int>();
        }

        internal void AddTitleToHistory(string windowTitle)
        {
            TitleHistory.Add(windowTitle.GetHashCode());
        }
    }

    internal static class WindowConfigurationFactory
    {

    }
}
