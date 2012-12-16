using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Assets;

namespace MazeMaster.Game.Humans
{
    /// <summary>
    /// Blood A type unit go front first, afterwhich turn side then go back. It will not destroy wall normally.
    /// If he panics, he will destroy all surrounding walls.
    /// </summary>
    public class BloodA : Unit
    {

        public BloodA()
            : base()
        {
            UnitTypeSource = GraphicsAssets.Instance.UnitType1;
        }
        // move to the next possible place, 
        protected override UnitAction GetNextAction()
        {
            Grid dashTarget = new Grid();
            if (CanSeeExit(out dashTarget))
            {
                return new UnitAction(ActionType.Dash, dashTarget);
            }
            if (IsPanic)
            {
                UnitAction action = new UnitAction(ActionType.Explode, CurrentGrid);
                return action;
            }
            else
            {
                bool canFront = false;
                bool canLeft = false;
                bool canRight = false;
                bool canBack = false;
                Random rng = new Random();
                Grid front = Helper.GetFrontOf(CurrentGrid, CurrentFacingDirection);

                canFront = CanMoveTo(front);
                // go forward first , provided that I have not visit it yet.
                if (canFront && !Memories.Contains(front))
                {
                    return new UnitAction(ActionType.Move, front);
                }
                
                Grid left = Helper.GetLeftOf(CurrentGrid, CurrentFacingDirection);
                canLeft = CanMoveTo(left);
                Grid right = Helper.GetRightOf(CurrentGrid, CurrentFacingDirection);
                canRight = CanMoveTo(right);
                Grid back = Helper.GetBackOf(CurrentGrid, CurrentFacingDirection);
                canBack = CanMoveTo(back);
                if (canLeft && canRight)
                {
                    //check if my left is visited && right is not visited, turn right
                    if (Memories.Contains(left) && !Memories.Contains(right))
                    {
                        return new UnitAction(ActionType.Move, right);
                    }
                    else if (Memories.Contains(right) && !Memories.Contains(left))
                    {
                        return new UnitAction(ActionType.Move, left);
                    }
                    else if (!Memories.Contains(left) && !Memories.Contains(right))
                    {
                        int r = rng.Next(0, 2);
                        if (r == 0)
                        {
                            return new UnitAction(ActionType.Move, right);
                        }
                        else
                        {
                            return new UnitAction(ActionType.Move, left);
                        }
                    }
                }
                else if (canLeft && !Memories.Contains(left))
                {
                    return new UnitAction(ActionType.Move, left);
                }
                else if (canRight && !Memories.Contains(right))
                {
                    return new UnitAction(ActionType.Move, right);
                }
                else if (canBack && !Memories.Contains(back))
                {
                    return new UnitAction(ActionType.Move, back);
                }
                
                List<Grid> gridChoice = new List<Grid>();
                if (canFront)
                {
                    gridChoice.Add(front);
                }
                if (canLeft)
                {
                    gridChoice.Add(left);
                }
                if (canRight)
                {
                    gridChoice.Add(right);
                }
                if (gridChoice.Count == 0)
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
                
                int rand = rng.Next(0, gridChoice.Count);
                return new UnitAction(ActionType.Move, gridChoice[rand]);
            }
        }
    }
}
