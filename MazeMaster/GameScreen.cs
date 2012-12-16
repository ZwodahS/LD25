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

        public float TotalTime;

        public float TotalRansom;

        public void RestartLevel()
        {
            TotalTime = 0;
            Random rng = new Random();
            CurrentMaze = new Maze(MazeGenerator.GenerateMaze(12, 12, rng.Next()));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(1, 11), rng.Next(1, 11));
            CurrentMaze.AddUnit(UnitType.Breaker, rng.Next(1, 11), rng.Next(1, 11));
            CurrentMaze.AddUnit(UnitType.Breaker, rng.Next(1, 11), rng.Next(1, 11));
            CurrentMaze.AddUnit(UnitType.Breaker, rng.Next(1, 11), rng.Next(1, 11));
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
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, new Rectangle(NextTile.CurrentGrid.Col * MazeMaster.TileSize, NextTile.CurrentGrid.Row * MazeMaster.TileSize, MazeMaster.TileSize, MazeMaster.TileSize), GraphicsAssets.Instance.TileHighlight, new Color(255, 255, 255, 0.2f));
            }

            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Next :", new Vector2(400, 20) * MazeMaster.ScreenMultiplier, Color.Black);
            NextTile.DrawAt(spriteBatch, gameTime, new Point(MazeMaster.ScreenMultiplier * 450,MazeMaster.ScreenMultiplier*20));

            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Career Time   : " + (int)(TotalTime / 60) + " mins  " + (TotalTime % 60).ToString("0.00") + " secs", new Vector2(400 * MazeMaster.ScreenMultiplier, 60 * MazeMaster.ScreenMultiplier), Color.Black);
            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Total Earning : " + "$" + (TotalRansom == 0 ? "0" : TotalRansom.ToString("#,#;(#,#)")), new Vector2(400 * MazeMaster.ScreenMultiplier, 80 * MazeMaster.ScreenMultiplier), Color.Black);
            float RPM = TotalRansom / TotalTime * 60;
            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Ransom Per min: " + "$" + (RPM == 0 ? "0" : RPM.ToString("#,#;(#,#)")), new Vector2(400 * MazeMaster.ScreenMultiplier, 100 * MazeMaster.ScreenMultiplier), Color.Black);
            for (int i = 0; i < CurrentMaze.Humans.Count; i++)
            {
                Unit unit = CurrentMaze.Humans[i];
                int pos =  (200 + i * 20)*MazeMaster.ScreenMultiplier;
                unit.DrawAt(spriteBatch, gameTime, new Point(400*MazeMaster.ScreenMultiplier,pos));
                spriteBatch.DrawString(GraphicsAssets.Instance.SideFont,unit.RansomTimeLeft.ToString("0.00") + " Secs To ransom", new Vector2(425*MazeMaster.ScreenMultiplier, pos+(7*MazeMaster.ScreenMultiplier)), Color.Black);
            }


            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            iH.Update();
            TotalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            Grid g = new Grid((int)p.Y / MazeMaster.TileSize, (int)p.X / MazeMaster.TileSize);
            return g;
        }

        public void RandomNext()
        {
            NextTile = MazeGenerator.RandomTile(0,0,new Random());
        }

        public void PlaceTile(Grid g)
        {
            if (CurrentMaze.CanPlace(g))
            {
                CurrentMaze.PlaceTile(NextTile, g);
                RandomNext();
            }
        }

        public void UnitEscape(Unit unit)
        {

        }

        public void RansomPaid(Unit unit)
        {
            TotalRansom += unit.RansomAmount;
        }
    }
}
