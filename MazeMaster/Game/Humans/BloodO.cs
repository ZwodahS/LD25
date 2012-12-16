using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Assets;

namespace MazeMaster.Game.Humans
{
    public class BloodO : Unit
    {
        public BloodO()
        {
            UnitTypeSource = GraphicsAssets.Instance.UnitType3;
        }
        protected override UnitAction GetNextAction()
        {
            Grid dashTarget = new Grid();
            if (CanSeeExit(out dashTarget))
            {
                return new UnitAction(ActionType.Dash, dashTarget);
            }
            if (IsPanic)
            {
                UnitAction action = new UnitAction(ActionType.Rage,CurrentGrid);
                return action;
            }
            else
            {
                Random rng = new Random();
                List<Grid> primaryChoices = new List<Grid>();
                Grid front = Helper.GetFrontOf(CurrentGrid, CurrentFacingDirection);
                bool canFront = CanMoveTo(front);
                Grid left = Helper.GetLeftOf(CurrentGrid, CurrentFacingDirection);
                bool canLeft = CanMoveTo(left);
                Grid right = Helper.GetRightOf(CurrentGrid, CurrentFacingDirection);
                bool canRight = CanMoveTo(right);
                Grid back = Helper.GetBackOf(CurrentGrid, CurrentFacingDirection);
                bool canBack = CanMoveTo(back);

                if (canFront && !Memories.Contains(front))
                {
                    primaryChoices.Add(front);
                }
                if (canLeft && !Memories.Contains(left))
                {
                    primaryChoices.Add(left);
                }
                if (canRight && !Memories.Contains(right))
                {
                    primaryChoices.Add(right);
                }
                if (primaryChoices.Count != 0)
                {
                    Grid choice = primaryChoices[rng.Next(0, primaryChoices.Count)];
                    return new UnitAction(ActionType.Move, choice);
                }
                
                List<Grid> secondaryChoices = new List<Grid>();
                if (canFront)
                {
                    secondaryChoices.Add(front);
                }
                if (canLeft)
                {
                    secondaryChoices.Add(left);
                }
                if (canRight)
                {
                    secondaryChoices.Add(right);
                }
                if (secondaryChoices.Count == 0)
                {
                    if (canBack)
                    {
                        return new UnitAction(ActionType.Move, back);
                    }
                    else
                    {
                        return new UnitAction(ActionType.None, CurrentGrid);
                    }
                }

                int rand = rng.Next(0, secondaryChoices.Count);
                return new UnitAction(ActionType.Move, secondaryChoices[rand]);
            }
        }
    }
}
