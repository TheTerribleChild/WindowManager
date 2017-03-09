using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SystemWindowOperator
{
    public class TopLevelWindow : WindowInfo
    {
        public string Title { get; private set; }
        public int WindowHandle { get; private set; }

        public TopLevelWindow(string applicationName, string title, int whnd, WindowPlacement placement, int z) : base(applicationName, placement, z)
        {
            this.Title = title;
            this.WindowHandle = whnd;
        }

        public override string ToString()
        {
            return ApplicationName + "|" + Title + "|" + WindowHandle + "|" + Z;
        }

        
    }
}
