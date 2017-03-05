using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager.Configurations
{
    internal class LayoutConfiguration
    {
        internal List<int> Configuration { get; private set; }
        internal HashSet<int> FastMatchingSet { get; private set; }
        internal int ScreenSettingId { get; private set; }

        internal LayoutConfiguration(WindowConfiguration[] windowConfigurations, int screenSettingId)
        {
            this.Configuration = new List<int>();
            this.ScreenSettingId = screenSettingId;
            string[] appName = new string[windowConfigurations.Length];
            for (int i = 0; i < windowConfigurations.Length; i++)
                appName[i] = windowConfigurations[i].ApplicationName;

            this.FastMatchingSet = LayoutConfiguration.GetFastApplicationMatchingSet(appName);

        }

        internal bool ContainsAll(HashSet<int> processes)
        {
            return FastMatchingSet.IsSupersetOf(processes);
        }

        internal static HashSet<int> GetFastApplicationMatchingSet(string[] applicationName)
        {
            HashSet<int> hashSet = new HashSet<int>();
            Dictionary<int, int> counter = new Dictionary<int, int>();
            int hash = 0;
            foreach (string app in applicationName)
            {
                hash = app.GetHashCode();
                if (counter.ContainsKey(hash))
                    counter[hash]++;
                else
                    counter[hash] = 1;
                hashSet.Add(String.Format("{0}{1}", app, counter[hash]).GetHashCode());
            }

            return hashSet;
        }
    }

    internal static class LayoutConfigurationFactory
    {
        
    }
}
