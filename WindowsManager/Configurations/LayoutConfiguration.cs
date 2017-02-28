using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    internal class LayoutConfiguration
    {
        internal Dictionary<string, int> Configuration { get; private set; }
        internal int ScreenSettingId { get; private set; }

        internal LayoutConfiguration(WindowConfiguration[] windowConfigurations, int screenSettingId)
        {
            this.Configuration = new Dictionary<string, int>();
            this.ScreenSettingId = screenSettingId;

            foreach (WindowConfiguration configuration in windowConfigurations)
                this.Configuration[configuration.ProcessName] = configuration.Key;

        }

        internal bool Contains(string process)
        {
            return Configuration.ContainsKey(process);
        }

        internal bool ContainsAll(string[] processes)
        {
            foreach(string processName in processes)
            {
                if (!Configuration.ContainsKey(processName))
                    return false;
            }
            return true;
        }
    }

    internal static class LayoutConfigurationFactory
    {

    }
}
