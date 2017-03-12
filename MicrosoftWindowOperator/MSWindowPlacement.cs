using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace MicrosoftWindowOperator
{
    public class MSWindowPlacement : SystemWindowOperator.WindowPlacement
    {
        public WinApiUtil.WINDOWPLACEMENT Placement { get; }

        public override int X
        {
            get
            {
                return Placement.rcNormalPosition.Left;
            }
        }

        public override int Y
        {
            get
            {
                return Placement.rcNormalPosition.Top;
            }
        }

        public override int Width
        {
            get
            {
                return Placement.rcNormalPosition.Right - Placement.rcNormalPosition.Left;
            }
        }

        public override int Height
        {
            get
            {
                return Placement.rcNormalPosition.Bottom - Placement.rcNormalPosition.Top;
            }
        }

        public override WindowPlacementState WindowState
        {
            get
            {
                switch (Placement.showCmd)
                {
                    case 1:
                        return WindowPlacementState.Normal;
                    case 2:
                        return WindowPlacementState.Minimized;
                    case 3:
                        return WindowPlacementState.Maximized;
                }
                return WindowPlacementState.Normal;
            }
        }

        public MSWindowPlacement(WinApiUtil.WINDOWPLACEMENT placement)
        {
            this.Placement = placement;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3}, {4}, {5})", X, Y, Width, Height, WindowState, Placement.showCmd);
        }
    }
}
