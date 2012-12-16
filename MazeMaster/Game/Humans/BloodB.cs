using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Assets;

namespace MazeMaster.Game.Humans
{
    
    public class BloodB : Unit
    {
        public BloodB()
            : base()
        {
            UnitTypeSource = GraphicsAssets.Instance.UnitType2;
        }
        protected bool CanDestroyWall(WallState currTile, WallState targetTile)
        {
            if (currTile == WallState.None || currTile == WallState.Broken )
            {
                if (targetTile == WallState.Sealed || targetTile == WallState.Breaking)
                {
                    return true;
                }
            }
            return false;
        }
        // move to the next possible place, 
        protected override UnitAction GetNextAction()
        {
            Grid dashTarget = new Grid();
            if (CanSeeExit(out dashTarget))
            {
                return new UnitAction(ActionType.Dash, dashTarget);
            }
            Random rng = new Random();
            // go forward first
            Grid front = Helper.GetFrontOf(CurrentGrid, CurrentFacingDirection);
            Grid left = Helper.GetLeftOf(CurrentGrid, CurrentFacingDirection);
            Grid right = Helper.GetRightOf(CurrentGrid, CurrentFacingDirection);
            Grid back = Helper.GetBackOf(CurrentGrid, CurrentFacingDirection);

            Tile frontTile = ContainingMaze.GetTile(front);
            Tile leftTile = ContainingMaze.GetTile(left);
            Tile rightTile = ContainingMaze.GetTile(right);
            Tile currTile = ContainingMaze.GetTile(CurrentGrid);

            bool canFront = CanMoveTo(front);
            bool canLeft = CanMoveTo(left);
            bool canRight = CanMoveTo(right);
            bool canBack = CanMoveTo(back);

            if (IsPanic)
            {
                if (!canFront)
                {
                    return new UnitAction(ActionType.Ghost, front);
                }
                else if (!canBack)
                {
                    return new UnitAction(ActionType.Ghost, back);
                }
                else if (!canLeft)
                {
                    return new UnitAction(ActionType.Ghost, left);
                }
                else if(!canRight)
                {
                    return new UnitAction(ActionType.Ghost, right);
                }
            }
            if (canFront && !Memories.Contains(front))
            {
                return new UnitAction(ActionType.Move, front);
            }
            
            // if the front can be destroy, continue
            {
                WallState curr, target;
                Helper.GetFrontWallPair(currTile, frontTile, CurrentFacingDirection, out curr, out target);
                if (CanDestroyWall(curr, target))
                {
                    return new UnitAction(ActionType.DestroyWeakWall, front);
                }
            }
            // next on list is to try left or right , if there is a unvisited place
            if (canLeft && canRight)
            {
                
                if (Memories.Contains(left) && !Memories.Contains(right))
                {
                    return new UnitAction(ActionType.Move, right);
                }
                else if (Memories.Contains(right) && !Memories.Contains(left))
                {
                    return new UnitAction(ActionType.Move, left);
                }
                else if (Memories.Contains(left) && Memories.Contains(right))
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
            // try to destroy any wall around me.
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
                else if (canDestroyRight)
                {
                    return new UnitAction(ActionType.DestroyWeakWall, right);
                }
            }
            // no wall to destroy and all path is explored , randomly choose one and go.
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

            int x = rng.Next(0, gridChoice.Count);
            return new UnitAction(ActionType.Move, gridChoice[x]);
            
        }
    }
}
