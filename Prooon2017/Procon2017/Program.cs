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

            //サイズ10 ボール2 (全網羅)
            if (Field.Size == 10 && Field.BallNum == 2)
            {
                Coor[] maxStartPosition = null;
                int[] maxRoute = null;
                int maxpoint = 0;
                var startBallPosition = new Coor[Field.BallNum];

                //long maxCalcuTime = 0;
                for (int x0 = 0; x0 < Field.Size; x0++)
                {
                    for (int y0 = 0; y0 < Field.Size; y0++)
                    {
                        for (int x1 = 0; x1 < Field.Size; x1++)
                        {
                            for (int y1 = 0; y1 < Field.Size; y1++)
                            {
                                //var stTimer = DateTime.UtcNow.Millisecond;
                                if (Field.Boad[x0, y0] == 3 ||
                                    Field.Boad[x1, y1] == 3 ||
                                    (x0 == x1 && y0 == y1)
                                    )
                                {
                                    continue;
                                }
                                startBallPosition = new Coor[Field.BallNum];
                                startBallPosition[0] = new Coor(x0, y0);
                                startBallPosition[1] = new Coor(x1, y1);
                                var calculate = new Calculate();
                                calculate.CalculateAllRoute(startBallPosition);
                                if (calculate.MaxPoint > maxpoint)
                                {
                                    maxpoint = calculate.MaxPoint;
                                    maxStartPosition = startBallPosition;
                                    maxRoute = calculate.MaxRoute;
                                }

                                //if (DateTime.UtcNow.Millisecond - stTimer > maxCalcuTime)
                                //{
                                //    maxCalcuTime = DateTime.UtcNow.Millisecond - stTimer;
                                //}
                            }
                        }
                    }
                }
                //出力
                Write(maxStartPosition, maxRoute);
                //Console.WriteLine("最大点" + maxpoint);
                //Console.WriteLine("最大計算時間" + maxCalcuTime);
                //Console.WriteLine(DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds + "ミリ秒くらい時間がかりました！");
            }
            else
            {
                Graph.CreateGraph();

                var groups = Graph.Groups.OrderByDescending(g => g.Count()).ToList();
                var sum = groups.Sum(g => g.Count);
                var excludeFukurokoji = groups
                    .Where(g =>
                        g.Exists(node => Array.IndexOf(node.Edge, null) != -1)
                    )
                    .ToList();

                var oneKatamukeList = new List<Node>[4];
                var katamukePoints = new int[4];
                for (int vector = 0; vector < 4; vector++)
                {
                    var list = Graph.Nodes
                        .Where(node => node.Edge[vector] == null)
                        .Select(node => new KeyValuePair<Node, int>(node, node.Points[vector][Field.Ends[vector][vector % 2 == 0 ? node.Coor.Y : node.Coor.X]]))
                        .OrderByDescending(item => item.Value)
                        .Where((point, rank) => rank < Field.BallNum);
                    oneKatamukeList[vector] = list.Select(item => item.Key).ToList();

                    katamukePoints[vector] = list.Sum(item => item.Value);
                }

                var maxPoint = 0;
                var maxVector = 0;
                for (int vector = 0; vector < 4; vector++)
                {
                    if (katamukePoints[vector] > maxPoint)
                    {
                        maxPoint = katamukePoints[vector];
                        maxVector = vector;
                    }
                }

                //出力
                foreach (var node in oneKatamukeList[maxVector])
                {
                    Console.WriteLine(node.Coor.X + " " + node.Coor.Y);
                }
                Console.WriteLine(maxVector);

                //Console.WriteLine("最大点" + maxPoint);
                //Console.WriteLine(DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds + "ミリ秒くらい時間がかりました！");
            }




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
