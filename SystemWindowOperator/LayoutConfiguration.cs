using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public abstract class LayoutConfiguration
    {
        public WindowConfiguration[] WindowConfigurations { get; private set; }
        public HashSet<int> FastMatchingSet { get; private set; }
        public int ScreenSettingId { get; private set; }

        public LayoutConfiguration(WindowConfiguration[] windowConfigurations, int screenSettingId)
        {
            this.WindowConfigurations = new WindowConfiguration[windowConfigurations.Length];
            this.ScreenSettingId = screenSettingId;

            string[] appName = new string[windowConfigurations.Length];

            for (int i = 0; i < windowConfigurations.Length; i++)
            {
                appName[i] = windowConfigurations[i].ApplicationName;
                this.WindowConfigurations[i] = windowConfigurations[i];
            }

            this.FastMatchingSet = LayoutConfiguration.GetFastApplicationMatchingSet(appName);
        }

        public bool ContainsAll(HashSet<int> processes)
        {
            return FastMatchingSet.IsSupersetOf(processes);
        }

        public static HashSet<int> GetFastApplicationMatchingSet(string[] applicationName)
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
}
