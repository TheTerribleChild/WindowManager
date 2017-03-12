using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public abstract class LayoutManager
    {

        public List<LayoutConfiguration> LayoutConfigurations { get; protected set; }

        protected LayoutManager()
        {
            this.LayoutConfigurations = new List<LayoutConfiguration>();
        }

        public void AddLayout(LayoutConfiguration layout)
        {
            LayoutConfigurations.Add(layout);
        }

        public abstract void Save();

        public abstract void Load();

    }
}
