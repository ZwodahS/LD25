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
        public Direction CurrentFacingDirection;

        public UnitAction? CurrentAction;

        public float RansomTimeLeft;
        public float RansomAmount;


        protected Rectangle UnitTypeSource;
        public Color InternalColor;
        public Color BorderColor;

        public Unit()
        {
        }

        public void SetGrid(Grid grid)
        {
            CurrentGrid = grid;
            TargetGrid = grid;
            Position = new Vector2(grid.Col * MazeMaster.TileSize, grid.Row * MazeMaster.TileSize);
        }
        public void SetPosition(Vector2 vector)
        {
            this.Position = vector;
        }



        public void Update(GameTime gameTime)
        {
            RansomTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (RansomTimeLeft <= 0)
            {
                TargetMaze.RansomPaid(this);
            }
            else 
            {
                if (CurrentAction != null)
                {
                    UpdateAction(gameTime);
                }
                else
                {
                    UnitAction action = GetNextAction();
                    CurrentAction = action;
                    if (action.Type == ActionType.Move)
                    {
                        MoveTo(action.TargetGrid);
                    }
                    else if (action.Type == ActionType.DestroyWeakWall)
                    {
                        DestroyWeakWall(action.TargetGrid);
                    }
                    else
                    {
                        CurrentAction = null;
                    }
                }
            }
            
        }
        public void UpdateAction(GameTime gameTime)
        {
            UnitAction action = ((UnitAction)CurrentAction);
            if (action.Type == ActionType.Move)
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
                else
                {

                }
            }
            else if (action.Type == ActionType.DestroyWeakWall)
            {
                Direction d = Helper.GetDirection(this.CurrentGrid, action.TargetGrid);
                CurrentFacingDirection = d;
                Grid face = Helper.GetFrontOf(this.CurrentGrid, d);
                Tile t = TargetMaze.GetTile(face);
                float damage = (1) * (float)gameTime.ElapsedGameTime.TotalSeconds; // the (1) is the "damage per sec" done by the unit
                int targetwall = 0;
                if (d == Direction.Up) //wall target is down of tile
                {
                    targetwall = Tile.Down;
                }
                else if (d == Direction.Down)
                {
                    targetwall = Tile.Up;
                }
                else if (d == Direction.Left)
                {
                    targetwall = Tile.Right;
                }
                else if (d == Direction.Right)
                {
                    targetwall = Tile.Left;
                }
                // check for destruction first.
                bool destroyed = false;
                if (targetwall == Tile.Down)
                {
                    if (t.DownWall == WallState.Broken || t.DownWall == WallState.None)
                    {
                        destroyed = true;
                    }
                }
                else if (targetwall == Tile.Up)
                {
                    if (t.UpWall == WallState.Broken || t.UpWall == WallState.None)
                    {
                        destroyed = true;
                    }
                }
                else if (targetwall == Tile.Left)
                {
                    if (t.LeftWall == WallState.Broken || t.LeftWall == WallState.None)
                    {
                        destroyed = true;
                    }
                }
                else if (targetwall == Tile.Right)
                {
                    if (t.RightWall == WallState.Broken || t.RightWall == WallState.None)
                    {
                        destroyed = true;
                    }
                }
                if (destroyed)
                {
                    CurrentAction = null;
                    t.WallAnimateTime[targetwall] = 0;
                }
                else
                {
                    if (d == Direction.Up) //wall target is down of tile
                    {
                        t.DownWall = WallState.Breaking;
                    }
                    else if (d == Direction.Down)
                    {
                        t.UpWall = WallState.Breaking;
                    }
                    else if (d == Direction.Left)
                    {
                        t.RightWall = WallState.Breaking;
                    }
                    else if (d == Direction.Right)
                    {
                        t.LeftWall = WallState.Breaking;
                    }
                    t.WallAnimateTime[targetwall] += damage;
                    if (t.WallAnimateTime[targetwall] >= 1)
                    {
                        if (targetwall == Tile.Down)
                        {
                            t.DownWall = WallState.Broken;
                        }
                        else if (targetwall == Tile.Up)
                        {
                            t.UpWall = WallState.Broken;
                        }
                        else if (targetwall == Tile.Left)
                        {
                            t.LeftWall = WallState.Broken;
                        }
                        else if (targetwall == Tile.Right)
                        {
                            t.RightWall = WallState.Broken;
                        }
                    }
                }

            }
        }
        public void DrawAt(SpriteBatch spriteBatch, GameTime gameTime,Point location)
        {
            int drawOffset = 0;
            switch (CurrentFacingDirection)
            {
                case Direction.Right: drawOffset = 32; break;
                case Direction.Down: drawOffset = 64; break;
                case Direction.Left: drawOffset = 96; break;
                default:
                    break;
            }
            Rectangle drawBound = new Rectangle((int)location.X, (int)location.Y, MazeMaster.TileSize, MazeMaster.TileSize);
            Rectangle draw = GraphicsAssets.Instance.UnitBorder;
            draw.X += drawOffset;
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, drawBound, draw, BorderColor);
            draw = GraphicsAssets.Instance.UnitInternal;
            draw.X += drawOffset;
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, drawBound, draw, InternalColor);
            draw = UnitTypeSource;
            draw.X += drawOffset;
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, drawBound, draw, Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int drawOffset = 0;
            switch (CurrentFacingDirection)
            {
                case Direction.Right: drawOffset = 32; break;
                case Direction.Down: drawOffset = 64; break;
                case Direction.Left: drawOffset = 96; break;
                default:
                    break;
            }
            Rectangle drawBound = new Rectangle((int)Position.X, (int)Position.Y, MazeMaster.TileSize, MazeMaster.TileSize);
            Rectangle draw = GraphicsAssets.Instance.UnitBorder;
            draw.X += drawOffset;
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, drawBound, draw, BorderColor);
            draw = GraphicsAssets.Instance.UnitInternal;
            draw.X += drawOffset;
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, drawBound, draw, InternalColor);
            draw = UnitTypeSource;
            draw.X += drawOffset;
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, drawBound, draw, Color.White);
        }

        public Rectangle DrawBound
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, MazeMaster.TileSize, MazeMaster.TileSize);
            }
        }

        public void ReachLocation()
        {
            MovementVector = Vector2.Zero;
            CurrentGrid = TargetGrid;
            CurrentAction = null;
            Tile t = TargetMaze.GetTile(CurrentGrid);
            if (t.IsExit)
            {
                TargetMaze.UnitEscape(this);
            }

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
            TargetLocation = new Vector2(grid.Col * MazeMaster.TileSize, grid.Row * MazeMaster.TileSize);
            MovementVector = (TargetLocation - Position)/10;
            MovementVector.Normalize();
            MovementVector *= 50 * MazeMaster.ScreenMultiplier;
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
                if (Helper.IsBlocked(new WallState[] { currTile.RightWall, targetTile.LeftWall },new ObstacleType[]{targetTile.Obstacle}))
                {
                    return false;
                }
                return true;
            }
            else if (moveDirection == Direction.Left)
            {
                if (Helper.IsBlocked(new WallState[] { currTile.LeftWall, targetTile.RightWall }, new ObstacleType[] { targetTile.Obstacle }))
                {
                    return false;
                }
                return true;
            }
            else if (moveDirection == Direction.Up)
            {
                if (Helper.IsBlocked(new WallState[] { currTile.UpWall, targetTile.DownWall }, new ObstacleType[] { targetTile.Obstacle }))
                {
                    return false;
                }
                return true;
            }
            else if (moveDirection == Direction.Down)
            {
                if (Helper.IsBlocked(new WallState[] { currTile.DownWall, targetTile.UpWall }, new ObstacleType[] { targetTile.Obstacle }))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void DestroyWeakWall(Grid grid) //TODO : refactor this
        {
            
        }
        
        /// <summary>
        /// The main method that all new "maze solving algorithm" needs to implement.
        /// </summary>
        /// <returns></returns>
        protected abstract UnitAction GetNextAction();

        internal static List<Unit> GetRandomTargets(int num)
        {
            List<Unit> targets = new List<Unit>();
            Random rng = new Random();
            for (int i = 0; i < num; i++)
            {
                int type = rng.Next(0, 2);
                Unit unit = null;
                if (type == 0)
                {
                    unit = CreateUnit(UnitType.Basic, rng);
                }
                else if (type == 1)
                {
                    unit = CreateUnit(UnitType.Breaker, rng);
                }
                targets.Add(unit);
            }
            return targets;
        }
        private static Unit CreateUnit(UnitType type,Random rng)
        {
            Unit unit = null;
            float timeBonus = 0.5f + (float)rng.NextDouble();
            if (type == UnitType.Basic)
            {
                unit = new BasicHuman();
                unit.RansomAmount = (int)(50000 * timeBonus);
                unit.RansomTimeLeft = (int)(40 * timeBonus);
            }
            else if (type == UnitType.Breaker)
            {
                unit = new Breaker();
                unit.RansomAmount = (int)(100000 * timeBonus);
                unit.RansomTimeLeft = (int)(25 * timeBonus);
            }
            unit.CurrentFacingDirection = Helper.RandomDirection(false);
            unit.BorderColor = new Color(rng.Next(100, 256), rng.Next(100, 256), rng.Next(100, 256), 255);
            unit.InternalColor = new Color(rng.Next(100, 256), rng.Next(100, 256), rng.Next(100, 256), 255);
            return unit;
        }
    }
}
