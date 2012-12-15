using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MazeMaster.Assets;

namespace MazeMaster.Game.Humans
{
    public abstract class Unit
    {
        public Grid TargetGrid;
        public Grid CurrentGrid;
        public Maze TargetMaze;

        public Vector2 Position;
        protected Vector2 MovementVector;
        protected Vector2 TargetLocation;
        protected Rectangle SourceBound;
        public Direction CurrentFacingDirection;
        public Unit()
        {
        }

        public void SetGrid(Grid grid)
        {
            CurrentGrid = grid;
            TargetGrid = grid;
            Position = new Vector2(grid.Col * 32, grid.Row * 32);
        }
        public void SetPosition(Vector2 vector)
        {
            this.Position = vector;
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentGrid != TargetGrid)
            {
                // for simplicity sake, the movement in this game will only only either be in the x or y direction and never both.
                Vector2 move = (float)(gameTime.ElapsedGameTime.TotalSeconds) * MovementVector;
                Position = Position + move;
                if (move.X != 0)
                {
                    if (move.X > 0)
                    {
                        if (Position.X > TargetLocation.X)
                        {
                            Position.X = TargetLocation.X;
                            ReachLocation();
                        }
                    }
                    else
                    {
                        if (Position.X < TargetLocation.X)
                        {
                            Position.X = TargetLocation.X;
                            ReachLocation();
                        }
                    }
                }
                else
                {
                    if (move.Y > 0)
                    {
                        if (Position.Y > TargetLocation.Y)
                        {
                            Position.Y = TargetLocation.Y;
                            ReachLocation();
                        }
                    }
                    else
                    {
                        if (Position.Y < TargetLocation.Y)
                        {
                            Position.Y = TargetLocation.Y;
                            ReachLocation();
                        }
                    }
                }
            }
            else // reach location or finish action
            {
                UnitAction action = GetNextAction();
                if (action.Type == ActionType.Move)
                {
                    MoveTo(action.TargetGrid);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), SourceBound, Color.White);
        }

        public Rectangle DrawBound
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
            }
        }

        public void ReachLocation()
        {
            MovementVector = Vector2.Zero;
            CurrentGrid = TargetGrid;
            Console.WriteLine("{0},{1}",CurrentGrid.Row,CurrentGrid.Col);
        }

        /// <summary>
        /// assumption , the grid is adjacent to it
        /// </summary>
        /// <param name="grid"></param>
        public void MoveTo(Grid grid)
        {
            if (MovementVector != Vector2.Zero)
            {
                // cannot move
                return;
            }
            TargetGrid = grid;
            TargetLocation = new Vector2(grid.Col * 32, grid.Row * 32);
            MovementVector = (TargetLocation - Position)/10;
            MovementVector.Normalize();
            MovementVector *= 100;
            CurrentFacingDirection = Helper.GetDirection(CurrentGrid, TargetGrid);
        }
        /// <summary>
        /// assumption , the grid is adjacent to it
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public bool CanMoveTo(Grid grid)
        {
            Tile currTile = TargetMaze.GetTile(CurrentGrid);
            Tile targetTile = TargetMaze.GetTile(grid);
            if (targetTile == null)
            {
                return false;
            }
            Direction moveDirection = Helper.GetDirection(CurrentGrid, grid);
            if (moveDirection == Direction.None)
            {
                return true; // since we are already there , then it "can" move to the location.
            }
            // ensure that there is no sealed wall or obstacle in that direction of movement
            else if (moveDirection == Direction.Right)
            {
                if (Helper.IsBlocked(new WallType[] { currTile.RightWall, targetTile.LeftWall },new ObstacleType[]{targetTile.Obstacle}))
                {
                    return false;
                }
                return true;
            }
            else if (moveDirection == Direction.Left)
            {
                if (Helper.IsBlocked(new WallType[] { currTile.LeftWall, targetTile.RightWall }, new ObstacleType[] { targetTile.Obstacle }))
                {
                    return false;
                }
                return true;
            }
            else if (moveDirection == Direction.Up)
            {
                if (Helper.IsBlocked(new WallType[] { currTile.UpWall, targetTile.DownWall }, new ObstacleType[] { targetTile.Obstacle }))
                {
                    return false;
                }
                return true;
            }
            else if (moveDirection == Direction.Down)
            {
                if (Helper.IsBlocked(new WallType[] { currTile.DownWall, targetTile.UpWall }, new ObstacleType[] { targetTile.Obstacle }))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// The main method that all new "maze solving algorithm" needs to implement.
        /// </summary>
        /// <returns></returns>
        protected abstract UnitAction GetNextAction();
    }
}
