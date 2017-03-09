using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    internal class LayoutConfigurationManager
    {
        private static LayoutConfigurationManager instance;

        internal static LayoutConfigurationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LayoutConfigurationManager();
                Console.WriteLine("Layout made");
                return instance;
            }
        }

        internal static string LAYOUT_CONFIGURATION_LOCATION
        {
            get
            {
                return Properties.Settings.Default["WorkingDirectory"] + "Layout.json";
            }
        }

        public List<LayoutConfiguration> LayoutConfigurations { get; private set; }

        private LayoutConfigurationManager()
        {
            Console.WriteLine("Config made ");
            LayoutConfigurations = new List<LayoutConfiguration>();
            //Console.WriteLine("Config made " + LayoutConfigurations == null);
        }

        public static void AddLayout(LayoutConfiguration layout)
        {
            Console.WriteLine("Is null " + Instance.LayoutConfigurations == null);
            Instance.LayoutConfigurations.Add(layout);
        }

        public static void Save()
        {
            Utility.SerializeUtility.SerializeToJsonFile(Instance.LayoutConfigurations, LAYOUT_CONFIGURATION_LOCATION);
        }

        public static void Load()
        {
            Instance.LayoutConfigurations = Utility.SerializeUtility.DeserializeJsonFile<List<LayoutConfiguration>>(LAYOUT_CONFIGURATION_LOCATION);
            if (Instance.LayoutConfigurations == default(List<LayoutConfiguration>))
                Instance.LayoutConfigurations = new List<LayoutConfiguration>();
        }
    }
}
