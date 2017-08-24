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

        public Node()
        {
            Edge = new Node[4];
        }
    }
}
