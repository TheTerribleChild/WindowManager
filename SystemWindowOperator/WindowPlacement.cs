using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public enum WindowPlacementState { Normal, Minimized, Maximized }

    public abstract class WindowPlacement
    {
        public abstract int X { get; }
        public abstract int Y { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract WindowPlacementState WindowState { get; }

        public override int GetHashCode()
        {
            return String.Format("{0},{1},{2},{3},{4}", X, Y, Width, Height, WindowState).GetHashCode();
        }
    }
}
