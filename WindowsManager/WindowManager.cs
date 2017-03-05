using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace WindowsManager
{
    public class WindowManager
    {
        private static WindowManager instance;
        private IWindowOperator windowOperator;
        private Configurations.WindowConfigurationManager windowConfigManager;
        private Configurations.LayoutConfigurationManager layoutManager;

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
            layoutManager = Configurations.LayoutConfigurationManager.Instance;
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
    }
}
