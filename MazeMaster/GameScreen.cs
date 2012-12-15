using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MazeMaster.Game.Humans;

namespace MazeMaster
{
    public class GameScreen
    {
        private Maze CurrentMaze;
        public GameScreen()
        {
               
        }


        public void RestartLevel()
        {
            Random rng = new Random();
            CurrentMaze = new Maze(MazeGenerator.GenerateMaze(25, 25, rng.Next()));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0,25),rng.Next(0,25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
            CurrentMaze.AddUnit(UnitType.Basic, rng.Next(0, 25), rng.Next(0, 25));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            CurrentMaze.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            CurrentMaze.Update(gameTime);
        }
    }
}
