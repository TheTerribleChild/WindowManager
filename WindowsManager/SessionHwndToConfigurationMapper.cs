using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager
{
    internal class SessionHwndToConfigurationMapper
    {
        private static SessionHwndToConfigurationMapper instance;

        internal static SessionHwndToConfigurationMapper Instance
        {
            get
            {
                if (instance == null)
                    instance = new SessionHwndToConfigurationMapper();
                return instance;
            }
        }
    }
}
