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
        internal object Placement { get; private set; }
        internal int Key { get; private set; }
        internal List<int> TitleHistory { get; private set; }

        internal WindowConfiguration(string processName, object placement)
        {
            this.ProcessName = processName;
            this.Placement = placement;
            this.Key = String.Format("{0}{1}", processName, placement.GetHashCode()).GetHashCode();
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
