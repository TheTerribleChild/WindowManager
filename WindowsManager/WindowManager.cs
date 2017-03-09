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
        private Configurations.WindowConfigurationManager windowConfigManager;
        private Configurations.LayoutConfigurationManager layoutConfigManager; 

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
            windowOperator = WindowOperatorFactory.CreateWindowOperator();
            windowConfigManager = Configurations.WindowConfigurationManager.Instance;
            layoutConfigManager = Configurations.LayoutConfigurationManager.Instance;
        }

        public void Test()
        {
            LoadConfigurationFromFile();
            TopLevelWindow[] windows = GetFilteredWindows();
            Configurations.LayoutConfiguration layoutConfig = new Configurations.LayoutConfiguration(GetWindowConfiguration(GetFilteredWindows()), 0);
            Configurations.LayoutConfigurationManager.AddLayout(layoutConfig);
            SaveConfigurationToFile();
        }

        private Configurations.WindowConfiguration[] GetWindowConfiguration(TopLevelWindow[] windows)
        {
            Configurations.WindowConfiguration[] windowConfigs = new Configurations.WindowConfiguration[windows.Length];
            for(int i = 0; i < windows.Length; i++)
                windowConfigs[i] = new Configurations.WindowConfiguration(windows[i] as WindowInfo);

            return windowConfigs;
        }

        private Configurations.LayoutConfiguration GetCurrentLayoutConfiguration()
        {
            TopLevelWindow[] windows = GetFilteredWindows();
            Configurations.WindowConfiguration[] windowConfigs = new Configurations.WindowConfiguration[windows.Length];
            for(int i = 0; i < windows.Length; i++)
                windowConfigs[i] = new Configurations.WindowConfiguration(windows[i]);
            
            Configurations.LayoutConfiguration config = new Configurations.LayoutConfiguration(windowConfigs, 0);
            return config;
        }

        private TopLevelWindow[] GetFilteredWindows()
        {
            TopLevelWindow[] windows = windowOperator.GetTopLevelWindow();
            List<TopLevelWindow> savedWindows = new List<TopLevelWindow>();
            int zIndex = 0;
            foreach(TopLevelWindow window in windows)
            {
                if (IsBlacklisted(window))
                    continue;

                window.SetZ(zIndex++);
                savedWindows.Add(window);
            }

            return savedWindows.ToArray();
        }

        private bool IsBlacklisted(TopLevelWindow window)
        {
            //Fill with blacklist logic later.
            return false;
        }

        private void SaveConfigurationToFile()
        {
            new Thread(delegate () {

                if (!Directory.Exists(Properties.Settings.Default["WorkingDirectory"].ToString()))
                    Directory.CreateDirectory(Properties.Settings.Default["WorkingDirectory"].ToString());

                Configurations.WindowConfigurationManager.Save();
                Configurations.LayoutConfigurationManager.Save();
            }).Start();
        }

        private bool LoadConfigurationFromFile()
        {
            if (!Directory.Exists(Properties.Settings.Default["WorkingDirectory"].ToString()))
                return false;

            Configurations.WindowConfigurationManager.Load();
            Configurations.LayoutConfigurationManager.Load();
            return true;
        }
    }
}
