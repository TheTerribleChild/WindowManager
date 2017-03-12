using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{

    public interface IWindowOperator
    {
        bool ApplyMappedLayout(MappedLayout mappedLayout);
        MappedLayout GetCurrentMappedLayout();
    }
}
