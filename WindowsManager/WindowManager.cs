using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace WindowsManager
{
    public class WindowManager
    {
        private static WindowManager instance;
        private IWindowOperator windowOperator;
        private LayoutManager layoutConfigManager; 

        public static WindowManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new WindowManager();
                return instance;
            }
        }

        private WindowManager()
        {
            //windowOperator = WindowOperatorFactory.CreateWindowOperator();
            windowOperator = WindowOperatorActivator.CreateWindowOperator();
            layoutConfigManager = WindowOperatorActivator.CreateLayoutManager();
        }

        public void Test()
        {
            DateTime start = DateTime.Now;
            LoadConfigurationFromFile();
            WindowConfiguration[] windows = GetFilteredWindows();
            LayoutConfiguration layoutConfig = GetCurrentLayoutConfiguration();
            layoutConfigManager.AddLayout(layoutConfig);
            SaveConfigurationToFile();
            DateTime end = DateTime.Now;
            Console.WriteLine(start - end);
        }

        private LayoutConfiguration GetCurrentLayoutConfiguration()
        {
            WindowConfiguration[] windows = GetFilteredWindows();            
            LayoutConfiguration layout = new LayoutConfiguration(windows, 0);
            return layout;
        }

        private WindowConfiguration[] GetFilteredWindows()
        {

            WindowConfiguration[] windows = windowOperator.GetCurrentMappedLayout().GetWindowConfigurations();
            List<WindowConfiguration> savedWindows = new List<WindowConfiguration>();
            int zIndex = 0;
            foreach(WindowConfiguration window in windows)
            {
                if (IsBlacklisted(window))
                    continue;

                window.SetZ(zIndex++);
                savedWindows.Add(window);
            }

            return savedWindows.ToArray();
            
            return null;
        }

        private bool IsBlacklisted(WindowConfiguration window)
        {
            //Fill with blacklist logic later.
            return false;
        }

        private void SaveConfigurationToFile()
        {
            new Thread(delegate () {
                layoutConfigManager.Save();
            }).Start();
        }

        private bool LoadConfigurationFromFile()
        {
            layoutConfigManager.Load();
            return true;
        }
    }
}
