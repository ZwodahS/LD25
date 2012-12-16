using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MazeMaster.Game.Humans;

namespace MazeMaster.Game
{
    public class Maze
    {
        protected Tile[,] Tiles;

        public List<Unit> Humans;
        public Maze(Tile[,] tiles)
        {
            this.Tiles = tiles;
            Humans = new List<Unit>();
        }

        public void AddUnit(UnitType type,int row,int col)
        {
            Grid grid = new Grid(row, col);
            Unit unit = null;
            if (type == UnitType.Basic)
            {
                unit = new BasicHuman();
            }
            else if (type == UnitType.Breaker)
            {
                unit = new Breaker();
            }
            unit.SetGrid(grid);
            unit.TargetMaze = this;
            unit.CurrentFacingDirection = Helper.RandomDirection(false);
            Humans.Add(unit);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int r = 0; r <= Tiles.GetUpperBound(0); r++)
            {
                for (int c = 0; c <= Tiles.GetUpperBound(1); c++)
                {
                    Tiles[r, c].Draw(spriteBatch, gameTime);
                }
            }
            foreach (Unit unit in Humans)
            {
                unit.Draw(spriteBatch, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int r = 0; r <= Tiles.GetUpperBound(0); r++)
            {
                for (int c = 0; c <= Tiles.GetUpperBound(1); c++)
                {
                    Tiles[r, c].Update(gameTime);
                }
            }
            foreach (Unit unit in Humans)
            {
                unit.Update(gameTime);
            }
        }

        public Tile GetTile(Grid grid)
        {
            if (!InRange(grid))
            {
                return null;
            }
            return Tiles[grid.Row, grid.Col];
        }
        public bool InRange(Grid grid)
        {
            if (grid.Row < 0 || grid.Col < 0 || grid.Row >= Tiles.GetLength(0) || grid.Col >= Tiles.GetLength(1))
            {
                return false;
            }
            return true;
        }
    }
}
