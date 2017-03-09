using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public class WindowConfiguration
    {

        public string ApplicationName { get; private set; }
        public WindowPlacement Placement { get; private set; }
        public int Z { get; private set; }

        public WindowConfiguration(string applicationName, WindowPlacement placement, int z)
        {
            this.ApplicationName = applicationName;
            this.Placement = placement;
            this.Z = z;
        }

        public int ArchiveID
        {
            get
            {
                return String.Format("{0},{1},{2}", ApplicationName, Placement, Z).GetHashCode();
            }
        }

        public void SetZ(int Z)
        {
            if (Z >= 0)
                this.Z = Z;
        }
    }
}
