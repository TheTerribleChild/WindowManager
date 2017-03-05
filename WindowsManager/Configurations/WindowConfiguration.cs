using System;
using System.Collections.Generic;
using SystemWindowOperator;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    internal class WindowConfiguration
    {
        internal TopLevelWindow Window { get; private set; }
        internal string ApplicationName
        {
            get
            {
                return Window.ApplicationName;
            }
        }

        internal int Key { get; private set; }
        internal List<int> TitleHistory { get; private set; }

        internal WindowConfiguration(TopLevelWindow window)
        {
            this.Window = window;
            this.Key = String.Format("{0}{1}", window.Title, window.Placement.GetHashCode()).GetHashCode();
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
