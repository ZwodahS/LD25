using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMaster.Game
{
    public class MazeGenerator
    {

        public static Tile[,] GenerateMaze(int row, int col, int seed)
        {
            Random rng = new Random(seed);
            Tile[,] tiles = new Tile[row, col];
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    tiles[r, c] = randomTile(r, c,rng);
                }
            }


            return tiles;
        }

        private static Tile randomTile(int row, int col, Random random)
        {
            Tile t = new Tile(row, col);
            int rand = random.Next(0, 2);
            t.LeftWall = rand == 0 ? WallType.Sealed : WallType.None;
            rand = random.Next(0, 2);
            t.RightWall = rand == 0 ? WallType.Sealed : WallType.None;
            rand = random.Next(0, 2);
            t.UpWall = rand == 0 ? WallType.Sealed : WallType.None;
            rand = random.Next(0, 2);
            t.DownWall = rand == 0 ? WallType.Sealed : WallType.None;

            return t;
        }

        private static Tile fullTile(int row, int col)
        {
            Tile t = new Tile(row, col);
            t.LeftWall = WallType.Sealed;
            t.RightWall = WallType.Sealed;
            t.UpWall = WallType.Sealed;
            t.DownWall = WallType.Sealed;

            return t;
        }
    }
}
