using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMaster.Game.Humans
{
    public struct UnitAction
    {
        public ActionType Type;
        public Grid TargetGrid;
        public UnitAction(ActionType type, Grid targetGrid)
        {
            this.Type = type;
            this.TargetGrid = targetGrid;
        }
    }
}
