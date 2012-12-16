using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Assets;

namespace MazeMaster.Game.Humans
{
    
    public class Breaker : Unit
    {
        public Breaker()
            : base()
        {
            UnitTypeSource = GraphicsAssets.Instance.UnitType2;
        }
        protected bool CanDestroyWall(WallState currTile, WallState targetTile)
        {
            if (currTile == WallState.None || currTile == WallState.Broken )
            {
                if (targetTile == WallState.Sealed)
                {
                    return true;
                }
            }
            return false;
        }
        // move to the next possible place, 
        protected override UnitAction GetNextAction()
        {
            // go forward first
            Grid front = Helper.GetFrontOf(CurrentGrid, CurrentFacingDirection);
            Grid left = Helper.GetLeftOf(CurrentGrid, CurrentFacingDirection);
            Grid right = Helper.GetRightOf(CurrentGrid, CurrentFacingDirection);
            Grid back = Helper.GetBackOf(CurrentGrid, CurrentFacingDirection);

            Tile frontTile = TargetMaze.GetTile(front);
            Tile leftTile = TargetMaze.GetTile(left);
            Tile rightTile = TargetMaze.GetTile(right);
            Tile currTile = TargetMaze.GetTile(CurrentGrid);
            if (CanMoveTo(front))
            {
                return new UnitAction(ActionType.Move, front);
            }
            // Check for wall that can be destroy :D  ._. tons of useless code =/
            {
                WallState curr, target;
                Helper.GetFrontWallPair(currTile, frontTile, CurrentFacingDirection, out curr, out target);
                if (CanDestroyWall(curr, target))
                {
                    return new UnitAction(ActionType.DestroyWeakWall, front);
                }
            }
            {
                WallState leftWall, rightWall, leftTargetWall, rightTargetWall;
                Helper.GetLeftWallPair(currTile, leftTile, CurrentFacingDirection, out leftWall, out leftTargetWall);
                Helper.GetRightWallPair(currTile, rightTile, CurrentFacingDirection, out rightWall, out rightTargetWall);
                bool canDestroyLeft = CanDestroyWall(leftWall, leftTargetWall);
                bool canDestroyRight = CanDestroyWall(rightWall, rightTargetWall);
                if (canDestroyLeft)
                {
                    if (canDestroyRight)
                    {
                        int rand = new Random().Next(0, 2);
                        if (rand == 0)
                        {
                            return new UnitAction(ActionType.DestroyWeakWall, left);
                        }
                        return new UnitAction(ActionType.DestroyWeakWall, right);
                    }
                    else
                    {
                        return new UnitAction(ActionType.DestroyWeakWall, left);
                    }
                }
                else if(canDestroyRight)
                {
                    return new UnitAction(ActionType.DestroyWeakWall, right);
                }
            }

            
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
