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
                    if (r < 2 || r >= (row - 2) || c < 2 || c >= (col - 2))
                    {
                        tiles[r, c] = ExitTile(r, c);
                    }
                    else
                    {
                        tiles[r, c] = RandomTile(r, c, rng);
                    }
                }
            }


            return tiles;
        }

        public static Tile RandomTile(int row, int col, Random random)
        {
            Tile t = new Tile(row, col);
            int rand = random.Next(0, 2);
            t.LeftWall = rand == 0 ? WallState.Sealed : WallState.None;
            rand = random.Next(0, 2);
            t.RightWall = rand == 0 ? WallState.Sealed : WallState.None;
            rand = random.Next(0, 2);
            t.UpWall = rand == 0 ? WallState.Sealed : WallState.None;
            rand = random.Next(0, 2);
            t.DownWall = rand == 0 ? WallState.Sealed : WallState.None;

            return t;
        }

        public static Tile FullTile(int row, int col)
        {
            Tile t = new Tile(row, col);
            t.LeftWall = WallState.Sealed;
            t.RightWall = WallState.Sealed;
            t.UpWall = WallState.Sealed;
            t.DownWall = WallState.Sealed;

            return t;
        }

        public static Tile ExitTile(int row, int col)
        {
            Tile t = new Tile(row, col);
            t.IsExit = true;
            return t;
        }
    }
}
