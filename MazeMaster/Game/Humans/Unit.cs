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
        public Maze ContainingMaze;

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

        protected List<Grid> Memories;
        protected int MaxMemory;

        public float PanicLevel;

        public void EraseMemory(Grid g)
        {
            for (int i = Memories.Count - 1; i >= 0; i--)
            {
                if (Memories[i] == g)
                {
                    Memories.RemoveAt(i);
                }
            }
        }
        public void AddMemory(Grid g)
        {
            int index = Memories.IndexOf(g);
            if (index != -1)
            {
                //refresh memory;
                Memories.RemoveAt(index); 
            }
            Memories.Add(g);
            while (Memories.Count > MaxMemory)
            {
                Memories.RemoveAt(0);
            }
        }

        public Unit()
        {
            Memories = new List<Grid>();
            MaxMemory = 10;
            IsPanic = false;
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
                ContainingMaze.RansomPaid(this);
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
                    else if (action.Type == ActionType.Explode)
                    {
                        panicAnimation = 0;
                    }
                    else if (action.Type == ActionType.Ghost)
                    {
                        panicAnimation = 0;
                        GhostTo(action);
                    }
                    else if (action.Type == ActionType.Rage)
                    {
                        panicAnimation = 0;
                    }
                    else if (action.Type == ActionType.Dash)
                    {
                        DashTo(action.TargetGrid);
                    }
                    else
                    {

                    }
                }
            }
            
        }
        public void UpdateAction(GameTime gameTime)
        {
            UnitAction action = ((UnitAction)CurrentAction);
            if (action.Type == ActionType.Move || action.Type == ActionType.Dash)
            {
                ActionMove(action, gameTime);
            }
            else if (action.Type == ActionType.DestroyWeakWall)
            {
                ActionDestroyWeakWall(action,gameTime);
            }
            else if (action.Type == ActionType.Explode)
            {
                ActionExplode(action, gameTime);
            }
            else if (action.Type == ActionType.Rage)
            {
                ActionRage(action, gameTime);
            }
            else if (action.Type == ActionType.Ghost)
            {
                ActionGhost(action, gameTime);
            }
            else if (action.Type == ActionType.None)
            {
                Panic();
            }
        }

        private void ActionGhost(UnitAction action, GameTime gameTime)
        {
            panicAnimation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ActionMove(action, gameTime); // delegate movement to move
            if (CurrentAction == null) // reached destination
            {
                EndPanic();
            }
        }

        private void ActionRage(UnitAction action, GameTime gameTime)
        {
            panicAnimation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float damage = (1) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            {// damage surrounding wall of currentile
                Tile currTile = ContainingMaze.GetTile(CurrentGrid);
                Tile targetTile = ContainingMaze.GetTile(Helper.GetFrontOf(CurrentGrid, CurrentFacingDirection));
                if (CurrentFacingDirection == Direction.Up)
                {
                    if (Helper.HasWall(currTile.UpWall))
                    {
                        DamageWall(currTile, Tile.Up, damage);
                    }
                    if (Helper.HasWall(targetTile.DownWall))
                    {
                        DamageWall(targetTile, Tile.Down, damage);
                    }
                    if (Helper.HasWall(targetTile.UpWall))
                    {
                        DamageWall(targetTile, Tile.Up, damage);
                    }
                }
                else if (CurrentFacingDirection == Direction.Down)
                {
                    if (Helper.HasWall(currTile.DownWall))
                    {
                        DamageWall(currTile, Tile.Down, damage);
                    }
                    if (Helper.HasWall(targetTile.UpWall))
                    {
                        DamageWall(targetTile, Tile.Up, damage);
                    }
                    if (Helper.HasWall(targetTile.DownWall))
                    {
                        DamageWall(targetTile, Tile.Down, damage);
                    }
                }
                else if (CurrentFacingDirection == Direction.Left)
                {
                    if (Helper.HasWall(currTile.LeftWall))
                    {
                        DamageWall(currTile, Tile.Left, damage);
                    }
                    if (Helper.HasWall(targetTile.RightWall))
                    {
                        DamageWall(targetTile, Tile.Right, damage);
                    }
                    if (Helper.HasWall(targetTile.LeftWall))
                    {
                        DamageWall(targetTile, Tile.Left, damage);
                    }
                }
                else if (CurrentFacingDirection == Direction.Right)
                {
                    if (Helper.HasWall(currTile.RightWall))
                    {
                        DamageWall(currTile, Tile.Right, damage);
                    }
                    if (Helper.HasWall(targetTile.LeftWall))
                    {
                        DamageWall(targetTile, Tile.Left, damage);
                    }
                    if (Helper.HasWall(targetTile.RightWall))
                    {
                        DamageWall(targetTile, Tile.Right, damage);
                    }
                    
                }
            }


            if (panicAnimation >= 3)
            {
                StopExplode();
            }
        }
        protected float panicAnimation; 
        private void ActionExplode(UnitAction action, GameTime gameTime)
        {
            panicAnimation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float damage = (1) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            {// damage surrounding wall of currentile
                Tile currentTile = ContainingMaze.GetTile(CurrentGrid);
                if (Helper.HasWall(currentTile.UpWall))
                {
                    DamageWall(currentTile, Tile.Up, damage);
                }
                if (Helper.HasWall(currentTile.LeftWall))
                {
                    DamageWall(currentTile, Tile.Left, damage);
                }
                if (Helper.HasWall(currentTile.RightWall))
                {
                    DamageWall(currentTile, Tile.Right, damage);
                }
                if (Helper.HasWall(currentTile.DownWall))
                {
                    DamageWall(currentTile, Tile.Down, damage);
                }
            }

            Tile leftTile = ContainingMaze.GetTile(new Grid(CurrentGrid.Row, CurrentGrid.Col - 1));
            if (leftTile != null)
            {
                if (Helper.HasWall(leftTile.RightWall))
                {
                    DamageWall(leftTile, Tile.Right, damage);
                }
            }
            Tile rightTile = ContainingMaze.GetTile(new Grid(CurrentGrid.Row, CurrentGrid.Col + 1));
            if (rightTile != null)
            {
                if (Helper.HasWall(rightTile.LeftWall))
                {
                    DamageWall(rightTile, Tile.Left, damage);
                }
            }
            Tile upTile = ContainingMaze.GetTile(new Grid(CurrentGrid.Row - 1, CurrentGrid.Col));
            if (upTile != null)
            {
                if (Helper.HasWall(upTile.DownWall))
                {
                    DamageWall(upTile, Tile.Down, damage);
                }
            }

            Tile downTile = ContainingMaze.GetTile(new Grid(CurrentGrid.Row + 1, CurrentGrid.Col));
            if (downTile != null)
            {
                if (Helper.HasWall(downTile.UpWall))
                {
                    DamageWall(downTile, Tile.Up, damage);
                }
            }


            if (panicAnimation >= 3)
            {
                StopExplode();
            }
        }
        
        private void StopExplode()
        {
            CurrentAction = null;
            panicAnimation = 0;
            EndPanic();
        }
        private void EndPanic()
        {
            PanicLevel = 0;
            IsPanic = false;
        }
        private void ActionMove(UnitAction action, GameTime gameTime)
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
        private void ActionDestroyWeakWall(UnitAction action,GameTime gameTime)
        {
            Direction d = Helper.GetDirection(this.CurrentGrid, action.TargetGrid);
            CurrentFacingDirection = d;
            Grid face = Helper.GetFrontOf(this.CurrentGrid, d);
            Tile t = ContainingMaze.GetTile(face);
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
                DamageWall(t, targetwall,damage);
            }
        }

        private void DamageWall(Tile t, int targetwall,float damage)
        {
            if (targetwall == Tile.Down)
            {
                t.DownWall = WallState.Breaking;
            }
            else if (targetwall == Tile.Up)
            {
                t.UpWall = WallState.Breaking;
            }
            else if (targetwall == Tile.Left)
            {
                t.LeftWall = WallState.Breaking;
            }
            else if (targetwall == Tile.Right)
            {
                t.RightWall = WallState.Breaking;
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

            if (CurrentAction != null )
            {
                UnitAction action = ((UnitAction)CurrentAction);
                if (action.Type == ActionType.Explode || action.Type == ActionType.Rage)
                {
                    Rectangle effectDraw = drawBound;
                    effectDraw.X += 16 * MazeMaster.ScreenMultiplier;
                    effectDraw.Y += 16 * MazeMaster.ScreenMultiplier;
                    spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, effectDraw, GraphicsAssets.Instance.ExplodingRect, Color.White, MathHelper.ToRadians(panicAnimation * 360), new Vector2(16, 16), SpriteEffects.None, 0f);
                }
                else if (action.Type == ActionType.Ghost)
                {
                    Rectangle effectDraw = drawBound;
                    effectDraw.X += 16 * MazeMaster.ScreenMultiplier;
                    effectDraw.Y += 16 * MazeMaster.ScreenMultiplier;
                    spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, effectDraw, GraphicsAssets.Instance.GhostRect, Color.White, MathHelper.ToRadians(panicAnimation * 360), new Vector2(16, 16), SpriteEffects.None, 0f);
                }
            }

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
            if (Memories.Contains(CurrentGrid))
            {
                Revisit(CurrentGrid);
            }
            AddMemory(CurrentGrid);
            
            Tile t = ContainingMaze.GetTile(CurrentGrid);
            if (t.IsExit)
            {
                ContainingMaze.UnitEscape(this);
            }
        }

        public void Revisit(Grid g)
        {
            PanicLevel += 10;
            if (PanicLevel >= 100)
            {
                Panic();
            }
        }
        public void GhostTo(UnitAction action)
        {
            CurrentAction = action;
            if (MovementVector != Vector2.Zero)
            {
                return;
            }
            TargetGrid = action.TargetGrid;
            TargetLocation = new Vector2(TargetGrid.Col * MazeMaster.TileSize, TargetGrid.Row * MazeMaster.TileSize);
            MovementVector = (TargetLocation - Position) / 10;
            MovementVector.Normalize();
            MovementVector *= 50 * MazeMaster.ScreenMultiplier;
            CurrentFacingDirection = Helper.GetDirection(CurrentGrid, TargetGrid);
        }
        public void DashTo(Grid grid)
        {
            if (MovementVector != Vector2.Zero)
            {
                // cannot move
                return;
            }
            TargetGrid = grid;
            TargetLocation = new Vector2(grid.Col * MazeMaster.TileSize, grid.Row * MazeMaster.TileSize);
            MovementVector = (TargetLocation - Position) / 10;
            MovementVector.Normalize();
            MovementVector *= 100 * MazeMaster.ScreenMultiplier;
            CurrentFacingDirection = Helper.GetDirection(CurrentGrid, TargetGrid);
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
            Tile currTile = ContainingMaze.GetTile(CurrentGrid);
            Tile targetTile = ContainingMaze.GetTile(grid);
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

        protected bool IsPanic = true;
        public virtual void Panic()
        {
            IsPanic = true;
            CurrentAction = null; // force a new action. The child will decide what to do
        }

        internal static List<Unit> GetRandomTargets(int num)
        {
            List<Unit> targets = new List<Unit>();
            Random rng = new Random();
            for (int i = 0; i < num; i++)
            {
                int type = rng.Next(0, 3);
                Unit unit = null;
                if (type == 0)
                {
                    unit = CreateUnit(BloodType.A, rng);
                }
                else if (type == 1)
                {
                    unit = CreateUnit(BloodType.B, rng);
                }
                else if (type == 2)
                {
                    unit = CreateUnit(BloodType.O, rng);
                }
                targets.Add(unit);
            }
            return targets;
        }
        private static Unit CreateUnit(BloodType type,Random rng)
        {
            Unit unit = null;
            float timeBonus = 0.5f + (float)rng.NextDouble();
            if (type == BloodType.A)
            {
                unit = new BloodA();
                unit.RansomAmount = (int)(50000 * timeBonus);
                unit.RansomTimeLeft = (int)(80 * timeBonus);
            }
            else if (type == BloodType.B)
            {
                unit = new BloodB();
                unit.RansomAmount = (int)(50000 * timeBonus);
                unit.RansomTimeLeft = (int)(50 * timeBonus);
            }
            else if (type == BloodType.O)
            {
                unit = new BloodO();
                unit.RansomAmount = (int)(50000 * timeBonus);
                unit.RansomTimeLeft = (int)(70 * timeBonus);
            }
            unit.CurrentFacingDirection = Helper.RandomDirection(false);
            unit.BorderColor = new Color(rng.Next(100, 256), rng.Next(100, 256), rng.Next(100, 256), 255);
            unit.InternalColor = new Color(rng.Next(100, 256), rng.Next(100, 256), rng.Next(100, 256), 255);
            return unit;
        }

        public bool CanSeeExit(out Grid nextGrid)
        {
            nextGrid = new Grid(-1, -1);
            
            
            Tile curr = ContainingMaze.GetTile(CurrentGrid);
            if (!Helper.IsBlocked(new WallState[] { curr.LeftWall }, new ObstacleType[] { }))
            {
                bool found = true;
                Grid g = CurrentGrid;
                g.Col -= 1;
                while (ContainingMaze.InRange(g))
                {
                    Tile t = ContainingMaze.GetTile(g);
                    if (t.IsExit)
                    {
                        break;
                    }
                    if (Helper.IsBlocked(new WallState[] { t.LeftWall, t.RightWall }, new ObstacleType[] { }))
                    {
                        found = false;
                        break;
                    }
                    g.Col -= 1;
                }
                if (found)
                {
                    nextGrid = new Grid(CurrentGrid.Row, CurrentGrid.Col - 1);
                    return true;
                }
            }

            if (!Helper.IsBlocked(new WallState[] { curr.RightWall }, new ObstacleType[] { }))
            {
                Grid g = CurrentGrid;
                g.Col += 1;
                bool found = true;
                while (ContainingMaze.InRange(g))
                {
                    Tile t = ContainingMaze.GetTile(g);
                    if (t.IsExit)
                    {
                        break;
                    }
                    if (Helper.IsBlocked(new WallState[] { t.LeftWall, t.RightWall }, new ObstacleType[] { }))
                    {
                        found = false;
                        break;
                    }
                    g.Col += 1;
                }
                if (found)
                {
                    nextGrid = new Grid(CurrentGrid.Row, CurrentGrid.Col + 1);
                    return true;
                }
            }

            if (!Helper.IsBlocked(new WallState[] { curr.DownWall }, new ObstacleType[] { }))
            {
                Grid g = CurrentGrid;
                g.Row += 1;
                bool found = true;
                while (ContainingMaze.InRange(g))
                {
                    Tile t = ContainingMaze.GetTile(g);
                    if (t.IsExit)
                    {
                        break;
                    }
                    if (Helper.IsBlocked(new WallState[] { t.UpWall, t.DownWall }, new ObstacleType[] { }))
                    {
                        found = false;
                        break;
                    }
                    g.Row += 1;
                }
                if (found)
                {
                    nextGrid = new Grid(CurrentGrid.Row + 1, CurrentGrid.Col);
                    return true;
                }
            }

            if (!Helper.IsBlocked(new WallState[] { curr.UpWall }, new ObstacleType[] { }))
            {
                Grid g = CurrentGrid;
                g.Row -= 1;
                bool found = true;
                while (ContainingMaze.InRange(g))
                {
                    Tile t = ContainingMaze.GetTile(g);
                    if (t.IsExit)
                    {
                        break;
                    }
                    if (Helper.IsBlocked(new WallState[] { t.UpWall, t.DownWall }, new ObstacleType[] { }))
                    {
                        found = false;
                        break;
                    }
                    g.Row -= 1;
                }
                if (found)
                {
                    nextGrid = new Grid(CurrentGrid.Row - 1, CurrentGrid.Col);
                    return true;
                }
            }
            return false;
        }
    }
}
