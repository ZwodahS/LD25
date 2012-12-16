using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMaster.Game
{
    // the collection of methods that i always use
    class Helper
    {
        // assuming that we are always either in the same row or column
        public static Direction GetDirection(Grid source, Grid destination)
        {
            if (source.Col == destination.Col && source.Row == destination.Row)
            {
                return Direction.None;
            }
            if (source.Col == destination.Col)
            {
                if (source.Row < destination.Row)
                {
                    return Direction.Down;
                }
                else
                {
                    return Direction.Up;
                }

            }
            else
            {
                if (source.Col < destination.Col)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }
        }
        public static Direction RandomDirection(bool useNone)
        {
            int rand = 0;
            if (useNone)
            {
                rand = (new Random()).Next(0, 5);
            }
            else
            {
                rand = (new Random()).Next(0, 4);
            }
            switch (rand)
            {
                case 0: return Direction.Right;
                case 1: return Direction.Left;
                case 2: return Direction.Up;
                case 3: return Direction.Down;
                case 4: return Direction.None;
                default: return Direction.None;
            }
        }

        public static bool IsBlocked(WallState[] walls,ObstacleType[] obstacles)
        {
            foreach (WallState wall in walls)
            {
                if (wall == WallState.Sealed || wall == WallState.Breaking) 
                {
                    return true;
                }
            }
            foreach (ObstacleType obstacle in obstacles)
            {
                if (obstacle == ObstacleType.Rock)
                {
                    return true;
                }
            }
            return false;
        }

        public static Grid GetFrontOf(Grid currentGrid, Direction facingDirection)
        {
            Grid targetGrid = currentGrid;
            if (facingDirection == Direction.Up)
            {
                targetGrid.Row -= 1;
            }
            else if (facingDirection == Direction.Down)
            {
                targetGrid.Row += 1;
            }
            else if (facingDirection == Direction.Left)
            {
                targetGrid.Col -= 1;
            }
            else if (facingDirection == Direction.Right)
            {
                targetGrid.Col += 1;
            }
            else // None
            {

            }
            return targetGrid;
        }
        public static Grid GetLeftOf(Grid currentGrid, Direction facingDirection)
        {
            Grid targetGrid = currentGrid;
            if (facingDirection == Direction.Up)
            {
                targetGrid.Col -= 1;
            }
            else if (facingDirection == Direction.Down)
            {
                targetGrid.Col += 1;
            }
            else if (facingDirection == Direction.Left)
            {
                targetGrid.Row += 1;
            }
            else if (facingDirection == Direction.Right)
            {
                targetGrid.Row -= 1;
            }
            else // None
            {

            }
            return targetGrid;
        }
        public static Grid GetRightOf(Grid currentGrid, Direction facingDirection)
        {
            Grid targetGrid = currentGrid;
            if (facingDirection == Direction.Up)
            {
                targetGrid.Col += 1;
            }
            else if (facingDirection == Direction.Down)
            {
                targetGrid.Col -= 1;
            }
            else if (facingDirection == Direction.Left)
            {
                targetGrid.Row -= 1;
            }
            else if (facingDirection == Direction.Right)
            {
                targetGrid.Row += 1;
            }
            else // None
            {

            }
            return targetGrid;
        }
        public static Grid GetBackOf(Grid currentGrid, Direction facingDirection)
        {
            Grid targetGrid = currentGrid;
            if (facingDirection == Direction.Up)
            {
                targetGrid.Row += 1;
            }
            else if (facingDirection == Direction.Down)
            {
                targetGrid.Row -= 1;
            }
            else if (facingDirection == Direction.Left)
            {
                targetGrid.Col += 1;
            }
            else if (facingDirection == Direction.Right)
            {
                targetGrid.Col -= 1;
            }
            else // None
            {

            }
            return targetGrid;
        }

        /// <summary>
        /// Get the wall state of the wall in front of the unit at the current tile
        /// </summary>
        /// <param name="tile">the current tile</param>
        /// <param name="facingDirection">the facing direction</param>
        /// <returns></returns>
        public static WallState GetFrontWallOf(Tile tile, Direction facingDirection)
        {
            if (facingDirection == Direction.Up)
            {
                return tile.UpWall;
            }
            else if (facingDirection == Direction.Down)
            {
                return tile.DownWall;
            }
            else if (facingDirection == Direction.Left)
            {
                return tile.LeftWall;
            }
            else if (facingDirection == Direction.Right)
            {
                return tile.RightWall;
            }
            else // None
            {
                return WallState.None;
            }
        }
        public static WallState GetBackWallOf(Tile tile, Direction facingDirection)
        {
            if (facingDirection == Direction.Up)
            {
                return tile.DownWall;
            }
            else if (facingDirection == Direction.Down)
            {
                return tile.UpWall;
            }
            else if (facingDirection == Direction.Left)
            {
                return tile.RightWall;
            }
            else if (facingDirection == Direction.Right)
            {
                return tile.LeftWall;
            }
            else // None
            {
                return WallState.None;
            }
        }
        public static WallState GetLeftWallOf(Tile tile, Direction facingDirection)
        {
            if (facingDirection == Direction.Up)
            {
                return tile.LeftWall;
            }
            else if (facingDirection == Direction.Down)
            {
                return tile.RightWall;
            }
            else if (facingDirection == Direction.Left)
            {
                return tile.DownWall;
            }
            else if (facingDirection == Direction.Right)
            {
                return tile.UpWall;
            }
            else // None
            {
                return WallState.None;
            }
        }
        public static WallState GetRightWallOf(Tile tile, Direction facingDirection)
        {
            if (facingDirection == Direction.Up)
            {
                return tile.RightWall;
            }
            else if (facingDirection == Direction.Down)
            {
                return tile.LeftWall;
            }
            else if (facingDirection == Direction.Left)
            {
                return tile.UpWall;
            }
            else if (facingDirection == Direction.Right)
            {
                return tile.DownWall;
            }
            else // None
            {
                return WallState.None;
            }
        }

        public static void GetFrontWallPair(Tile currTile, Tile targetTile, Direction facingDirection, out WallState currWall, out WallState targetWall)
        {
            currWall = GetFrontWallOf(currTile, facingDirection);
            targetWall = GetBackWallOf(targetTile, facingDirection);
        }
        public static void GetBackWallPair(Tile currTile, Tile targetTile, Direction facingDirection, out WallState currWall, out WallState targetWall)
        {
            currWall = GetBackWallOf(currTile, facingDirection);
            targetWall = GetFrontWallOf(targetTile, facingDirection);
        }
        public static void GetLeftWallPair(Tile currTile, Tile targetTile, Direction facingDirection, out WallState currWall, out WallState targetWall)
        {
            currWall = GetLeftWallOf(currTile, facingDirection);
            targetWall = GetRightWallOf(targetTile, facingDirection);
        }
        public static void GetRightWallPair(Tile currTile, Tile targetTile, Direction facingDirection, out WallState currWall, out WallState targetWall)
        {
            currWall = GetRightWallOf(currTile, facingDirection);
            targetWall = GetLeftWallOf(targetTile, facingDirection);
        }
    }
}
