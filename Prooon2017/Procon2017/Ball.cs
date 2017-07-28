using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    public class Ball
    {
        public Coor Coor { get; set; }

        public int[] Points;
        public bool IsOut;
        public Ball()
        {
            this.Points = new int[3];
        }
    }
}
