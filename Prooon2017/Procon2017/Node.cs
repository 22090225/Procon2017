using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    public class Node
    {
        public Coor Coor;
        public int Index;
        public Node[] Edge;
        public bool Grouped;
        public bool Danger;
        //vector×color
        public int[][] Points;

        public Node()
        {
            Edge = new Node[4];
            Points = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                Points[i] = new int[3];
            }
        }
    }
}
