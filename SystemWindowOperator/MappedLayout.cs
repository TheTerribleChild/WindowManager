using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public struct MappedWindow
    {
        public object Handle;
        public WindowConfiguration Configuration;
        public string Title;
    }

    public class MappedLayout
    {
        

        public List<MappedWindow> Mapping { get; private set; }

        public MappedLayout()
        {
            this.Mapping = new List<MappedWindow>();
        }

        public void AddMapping(object handle, WindowConfiguration windowConfig, string title = null)
        {
            MappedWindow mappedWindow = new MappedWindow();
            mappedWindow.Handle = handle;
            mappedWindow.Configuration = windowConfig;
            mappedWindow.Title = title;
            Mapping.Add(mappedWindow);
        }

        public void SortByZ()
        {
            Mapping.Sort((win1, win2) => win1.Configuration.Z.CompareTo(win2.Configuration.Z));
        }

        public WindowConfiguration[] GetWindowConfigurations()
        {
            return (from window in Mapping select window.Configuration).ToArray<WindowConfiguration>();
        }
    }
}
