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

        public static bool IsBlocked(WallType[] walls,ObstacleType[] obstacles)
        {
            foreach (WallType wall in walls)
            {
                if (wall == WallType.Sealed) 
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
    }
}
