using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    internal class WindowConfigurationManager
    {
        private static WindowConfigurationManager instance;

        internal static WindowConfigurationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new WindowConfigurationManager();
                return instance;
            }
        }

        internal static string WINDOW_CONFIGURATION_LOCATION
        {
            get
            {
                return Properties.Settings.Default["WorkingDirectory"] + "Windows.json";
            }
        }

        public Dictionary<int, WindowConfiguration> WindowConfigurations { get; private set; }

        private WindowConfigurationManager()
        {
            this.WindowConfigurations = new Dictionary<int, WindowConfiguration>();
        }

        public static void Save()
        {
            if (instance != null)
                Utility.SerializeUtility.SerializeToJsonFile(Instance.WindowConfigurations, WINDOW_CONFIGURATION_LOCATION);
        }

        public static bool Load()
        {
            Instance.WindowConfigurations = Utility.SerializeUtility.DeserializeJsonFile<Dictionary<int, WindowConfiguration>>(WINDOW_CONFIGURATION_LOCATION);
            return Instance.WindowConfigurations != default(Dictionary<int, WindowConfiguration>);
        }

    }
}
