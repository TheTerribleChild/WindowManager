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

    }
}
