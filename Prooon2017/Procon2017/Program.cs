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
            Graph.InitializeField();
            Graph.CreateGraph();



            //Field.InitializeField();
            //Coor[] maxStartPosition = null;
            //int[] maxRoute = null;
            //int maxpoint = 0;


            //var combi = new bool[Field.Size, Field.Size];
            //var startBallPosition = new Coor[Field.BallNum];

            //var coor = startBallPosition[Field.Size - 1];

            //for (int i = 0; i < Field.Size; i++)
            //{
            //    startBallPosition[i] = Next(startBallPosition[i]);
            //}


            //for (int x0 = 0; x0 < Field.Size; x0++)
            //{
            //    for (int y0 = 0; y0 < Field.Size; y0++)
            //    {
            //        for (int x1 = 0; x1 < Field.Size; x1++)
            //        {
            //            for (int y1 = 0; y1 < Field.Size; y1++)
            //            {
            //                if (Field.OriginalBoad[x0, y0].State == Tile.TileState.Wall ||
            //                    Field.OriginalBoad[x1, y1].State == Tile.TileState.Wall
            //                    )
            //                {
            //                    continue;
            //                }
            //                startBallPosition = new Coor[Field.BallNum];
            //                startBallPosition[0] = new Coor(x0, y0);
            //                startBallPosition[1] = new Coor(x1, y1);
            //                var field = new Field();
            //                field.CalculateAllRoute(startBallPosition);
            //                if (field.MaxPoint > maxpoint)
            //                {
            //                    maxpoint = field.MaxPoint;
            //                    maxStartPosition = startBallPosition;
            //                    maxRoute = field.MaxRoute;
            //                }
            //            }
            //        }
            //    }
            //}

            //for (int x = 0; x < Field.Size; x++)
            //{
            //    for (int y = 0; y < Field.Size; y++)
            //    {
            //        if (Field.OriginalBoad[x, y].State == Tile.TileState.Wall)
            //        {
            //            continue;
            //        }
            //        var startBallPosition = new Coor[Field.BallNum];
            //        startBallPosition[0] = new Coor(x, y);
            //        var field = new Field();
            //        field.CalculateAllRoute(startBallPosition);
            //        if (field.MaxPoint > maxpoint)
            //        {
            //            maxpoint = field.MaxPoint;
            //            maxStartPosition = startBallPosition;
            //            maxRoute = field.MaxRoute;
            //        }
            //    }
            //}

            //startBallPosition[0] = new Coor(0, 0);
            //startBallPosition[1] = new Coor(2, 1);
            //var field = new Field();
            //field.CalculateAllRoute(startBallPosition);
            //if (field.MaxPoint > maxpoint)
            //{
            //    maxpoint = field.MaxPoint;
            //    maxStartPosition = startBallPosition;
            //    maxRoute = field.MaxRoute;
            //}



            ////出力
            //Write(maxStartPosition, maxRoute);
            //Console.WriteLine("最大点" + maxpoint);
            Console.WriteLine(DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds + "ミリ秒くらい時間がかりました！");


        }

        private static Coor Next(Coor coor)
        {
            if (coor.X == Field.Size - 1)
            {
                if (coor.Y == Field.Size - 1)
                {
                    return new Coor(0, 0);
                }
                return new Coor(0, coor.Y + 1);
            }
            coor.X++;
            return coor;
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
