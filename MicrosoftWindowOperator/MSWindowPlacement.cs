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
                return Placement.ptMinPosition.X;
            }
        }

        public override int Y
        {
            get
            {
                return Placement.ptMinPosition.Y;
            }
        }

        public override int Width
        {
            get
            {
                return Placement.ptMaxPosition.X - Placement.ptMinPosition.X;
            }
        }

        public override int Height
        {
            get
            {
                return Placement.ptMaxPosition.Y - Placement.ptMinPosition.Y;
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

        internal MSWindowPlacement(WinApiUtil.WINDOWPLACEMENT placement)
        {
            this.Placement = placement;
        }
    }
}
