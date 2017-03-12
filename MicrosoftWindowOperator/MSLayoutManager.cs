using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace MicrosoftWindowOperator
{
    public class MSLayoutManager : LayoutManager
    {
        internal static string LAYOUT_CONFIGURATION_LOCATION
        {
            get
            {
                return Properties.Settings.Default["WorkingDirectory"] + "Layout.json";
            }
        }

        public override void Save()
        {
            if (!Directory.Exists(Properties.Settings.Default["WorkingDirectory"].ToString()))
                Directory.CreateDirectory(Properties.Settings.Default["WorkingDirectory"].ToString());
            
            Utility.SerializeUtility.SerializeToJsonFile(LayoutConfigurations, LAYOUT_CONFIGURATION_LOCATION);
        }

        public override void Load()
        {
            if (!File.Exists(LAYOUT_CONFIGURATION_LOCATION))
                return;

            LayoutConfigurations = Utility.SerializeUtility.DeserializeJsonFile<List<LayoutConfiguration>>(LAYOUT_CONFIGURATION_LOCATION);
            if (LayoutConfigurations == null)
                LayoutConfigurations = new List<LayoutConfiguration>();
        }
    }
}
