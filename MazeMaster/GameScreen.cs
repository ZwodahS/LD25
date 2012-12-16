using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MazeMaster.Game.Humans;
using MazeMaster.Inputs;
using MazeMaster.Assets;

namespace MazeMaster
{
    public class GameScreen
    {
        private Maze CurrentMaze;

        public Tile NextTile;
        public InputHandler iH;
        public GameScreen()
        {
            iH = new InputHandler();
        }


        public void RestartLevel()
        {
            Random rng = new Random();
            CurrentMaze = new Maze(MazeGenerator.GenerateMaze(14, 14, rng.Next()));
            CurrentMaze.AddUnit(UnitType.Breaker, rng.Next(2, 12), rng.Next(2, 12));
            CurrentMaze.Parent = this;
            RandomNext();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            CurrentMaze.Draw(spriteBatch, gameTime);
            if (NextTile.CurrentGrid.Row != -1)
            {
                NextTile.Draw(spriteBatch, gameTime);
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, new Rectangle(NextTile.CurrentGrid.Col * 32, NextTile.CurrentGrid.Row * 32, 32, 32), GraphicsAssets.Instance.TileHighlight, new Color(255, 255, 255, 0.2f));
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            iH.Update();
            CurrentMaze.Update(gameTime);
            Grid highlightedGrid = PointToGrid(iH.Pos);
            if (CurrentMaze.CanPlace(highlightedGrid))
            {
                NextTile.CurrentGrid = highlightedGrid;
            }
            else
            {
                NextTile.CurrentGrid = new Grid(-1, -1);
            }

            if (iH.MouseLeft[InputHandler.THIS])
            {
                PlaceTile(highlightedGrid);
            }
        }


        public Grid PointToGrid(Point p)
        {
            Grid g = new Grid((int)p.Y / 32, (int)p.X / 32);
            return g;
        }

        public void RandomNext()
        {
            NextTile = MazeGenerator.RandomTile(0,0,new Random());
        }

        public void PlaceTile(Grid g)
        {
            CurrentMaze.PlaceTile(NextTile, g);
            RandomNext();
        }

        public void UnitEscape(Unit unit)
        {

        }
    }
}
