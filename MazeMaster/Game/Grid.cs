using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMaster.Game
{
    public struct Grid
    {
        public int Row;
        public int Col;
        public Grid(int Row, int Col)
        {
            this.Row = Row;
            this.Col = Col;
        }
        public static bool operator !=(Grid grid1, Grid grid2)
        {
            if (grid1.Row != grid2.Row || grid1.Col != grid2.Col)
            {
                return true;
            }
            return false;
        }
        public static bool operator ==(Grid grid1, Grid grid2)
        {
            if (grid1.Row == grid2.Row && grid1.Col == grid2.Col)
            {
                return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            if (obj is Grid)
            {
                return ((Grid)obj) == this;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Row *10000 +  Col;
        }
    }
}
