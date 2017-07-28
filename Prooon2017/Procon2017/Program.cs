using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculateStartTime = DateTime.UtcNow;

            Field.InitializeField();

            var startBallPosition = new Coor[Field.BallNum];
            Coor[] maxStartPosition = null;
            int[] maxRoute = null;
            int maxpoint = 0;
            //for (int x = 0; x < Field.Size; x++)
            //{
            //    for (int y = 0; y < Field.Size; y++)
            //    {
            //        if (Field.OriginalBoad[x, y].State == Tile.TileState.Wall)
            //        {
            //            continue;
            //        }
            var x = 9;
            var y = 1;
                    startBallPosition[0] = new Coor(x, y);
                    var field = new Field();
                    field.CalculateAllRoute(startBallPosition);
                    if (field.MaxPoint > maxpoint)
                    {
                        maxpoint = field.MaxPoint;
                        maxStartPosition = startBallPosition;
                        maxRoute = field.MaxRoute;
                    }
            //    }
            //}
            /*
             * ここでボールポジ探索
             * 
             */

            /*
            * ここでボールポジ探索
            * 
            */


            //出力
            Write(maxStartPosition, maxRoute);
            Console.WriteLine("最大点" + maxpoint);
            Console.WriteLine(DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds + "ミリ秒くらい時間がかりました！");


        }

        static private void Write(Coor[] ballposi, int[] route)
        {
            foreach (var coor in ballposi)
            {
                Console.WriteLine(coor.X + " " + coor.Y);
            }
            foreach (var direction in route)
            {
                Console.WriteLine(direction);
            }
        }
    }
}
