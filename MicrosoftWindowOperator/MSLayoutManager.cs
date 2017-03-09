using System;
using System.Collections.Generic;
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

        public override void Load()
        {
            Utility.SerializeUtility.SerializeToJsonFile(LayoutConfigurations, LAYOUT_CONFIGURATION_LOCATION);
        }

        public override void Save()
        {
            LayoutConfigurations = Utility.SerializeUtility.DeserializeJsonFile<List<LayoutConfiguration>>(LAYOUT_CONFIGURATION_LOCATION);
        }
    }
}
