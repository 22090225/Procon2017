using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    public class Tile
    {
        public int X;
        public int Y;
        public TileState State;
        //public bool IsPassed;

        public enum TileState
        {
            Red,
            Blue,
            Green,
            Wall
        } 
    }
}
