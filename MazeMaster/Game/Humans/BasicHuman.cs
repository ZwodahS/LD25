using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Assets;

namespace MazeMaster.Game.Humans
{
    public class BasicHuman : Unit
    {
        public BasicHuman()
            : base()
        {
            SourceBound = GraphicsAssets.Instance.Characters[0];
        }
        // move to the next possible place, 
        protected override UnitAction GetNextAction()
        {
            // go forward first
            Grid target = Helper.GetFrontOf(CurrentGrid, CurrentFacingDirection);
            if(CanMoveTo(target))
            {
                return new UnitAction(ActionType.Move, target);
            }

            Grid left = Helper.GetLeftOf(CurrentGrid, CurrentFacingDirection);
            Grid right = Helper.GetRightOf(CurrentGrid, CurrentFacingDirection);
            Grid back = Helper.GetBackOf(CurrentGrid,CurrentFacingDirection);
            if (CanMoveTo(left) && CanMoveTo(right))
            {
                Random rng = new Random();
                int rand = rng.Next(0, 2);
                if (rand == 0)
                {
                    return new UnitAction(ActionType.Move, left);
                }
                else
                {
                    return new UnitAction(ActionType.Move, right);
                }
            }
            else if (CanMoveTo(left))
            {
                return new UnitAction(ActionType.Move, left);
            }
            else if (CanMoveTo(right))
            {
                return new UnitAction(ActionType.Move, right);
            }
            else if (CanMoveTo(back))
            {
                return new UnitAction(ActionType.Move, back);
            }
            else
            {
                return new UnitAction(ActionType.None, CurrentGrid);
            }
        }
    }
}
