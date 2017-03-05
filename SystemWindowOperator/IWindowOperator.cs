using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public interface IWindowOperator
    {
        TopLevelWindow[] GetTopLevelWindow();
        bool ApplyLayoutConfiguration(TopLevelWindow[] layoutConfiguration);

    }
}
