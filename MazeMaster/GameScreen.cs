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
            CurrentMaze = new Maze(MazeGenerator.GenerateMaze(25, 25, new Random().Next()));
            CurrentMaze.AddUnit(UnitType.Basic, 12, 12);
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
