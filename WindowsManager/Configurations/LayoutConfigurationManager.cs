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
                return instance;
            }
        }
    }
}
