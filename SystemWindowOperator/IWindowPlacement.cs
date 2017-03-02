using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public enum WindowPlacementState { Normal, Minimized, Maximized }

    public interface IWindowPlacement
    {
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
        WindowPlacementState WindowState { get; }
    }
}
