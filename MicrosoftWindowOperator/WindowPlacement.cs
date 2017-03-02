using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace MicrosoftWindowOperator
{
    public class WindowPlacement : SystemWindowOperator.IWindowPlacement
    {
        internal WinApiUtil.WINDOWPLACEMENT Placement { get; }

        public int X
        {
            get
            {
                return Placement.ptMinPosition.X;
            }
        }

        public int Y
        {
            get
            {
                return Placement.ptMinPosition.Y;
            }
        }

        public int Width
        {
            get
            {
                return Placement.ptMaxPosition.X - Placement.ptMinPosition.X;
            }
        }

        public int Height
        {
            get
            {
                return Placement.ptMaxPosition.Y - Placement.ptMinPosition.Y;
            }
        }

        public WindowPlacementState WindowState
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

        internal WindowPlacement(WinApiUtil.WINDOWPLACEMENT placement)
        {
            this.Placement = placement;
        }
    }
}
