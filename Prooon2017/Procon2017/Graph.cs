using System;
using System.Collections.Generic;
using System.Linq;

namespace Procon2017
{
    public class Graph
    {
        public static int Size;
        public static int BallNum;
        public static int[][] Ends;
        public static int[,] OriginalBoad;

        public static void InitializeField()
        {
            Size = int.Parse(Console.In.ReadLine());
            BallNum = int.Parse(Console.In.ReadLine());

            OriginalBoad = new int[Size, Size];
            Ends = new int[4][];
            for (int i = 0; i < Size; i++)
            {
                var line = (Console.In.ReadLine()).Split(' ')
                    .ToArray();
                for (int j = 0; j < Size; j++)
                {
                    OriginalBoad[j, i] = StringToInt(line[j]);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Ends[i] = (Console.In.ReadLine()).Split(' ').Select(str => StringToInt(str)).ToArray();
            }
        }

        public static void CreateGraph()
        {
            var nodes = new Node[Size, Size];
            var nodeList = new List<Node>();

            //ノードの抽出
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (OriginalBoad[x, y] == 3)
                    {
                        var coor = new Coor(x, y);
                        for (int vector = 0; vector < 4; vector++)
                        {
                            var nextCoor = MoveOne(coor, vector);
                            if (!IsOut(nextCoor, vector) &&
                                OriginalBoad[nextCoor.X, nextCoor.Y] != 3 &&
                                nodes[nextCoor.X, nextCoor.Y] == null
                                )
                            {
                                var node = new Node()
                                {
                                    Coor = nextCoor,
                                    Index = nodeList.Count()
                                };
                                nodes[nextCoor.X, nextCoor.Y] = node;
                                nodeList.Add(node);
                            }
                        }
                    }
                }
            }

            //ノードの紐つけ
            //foreach (var item in nodeList.Select((v, i) => new { v, i }))
            foreach (var node in nodeList)
            {
                for (int vector = 0; vector < 4; vector++)
                {
                    var coor = node.Coor;
                    node.Points[vector][OriginalBoad[coor.X, coor.Y]]++;
                    var points = new int[3];
                    while (true)
                    {
                        coor = MoveOne(coor, vector);

                        if (IsOut(coor, vector))
                        {
                            break;
                        }
                        if (OriginalBoad[coor.X, coor.Y] == 3)
                        {
                            //一歩戻る
                            coor = MoveOne(coor, (vector + 2) % 4);
                            node.Edge[vector] = nodes[coor.X, coor.Y];
                            node.Points[vector][OriginalBoad[coor.X, coor.Y]]--;
                            break;
                        }
                        node.Points[vector][OriginalBoad[coor.X, coor.Y]]++;
                    }
                }
            }

            //行列作成
            var matrix = new int?[nodeList.Count(), nodeList.Count()];
            foreach (var item in nodeList.Select((v, i) => new { v, i }))
            {
                for (int vector = 0; vector < 4; vector++)
                {
                    if (item.v.Edge[vector] != null)
                    {
                        matrix[item.i, item.v.Edge[vector].Index] = vector;
                    }
                }
            }

            //グループ分け
            var groupList = new List<List<Node>>();
            List<Node> group;
            void Grouping(Node node)
            {
                group.Add(node);
                node.Grouped = true;
                for (int vector = 0; vector < 4; vector++)
                {
                    if (node.Edge[vector] != null &&
                        node.Index != node.Edge[vector].Index &&
                        !node.Edge[vector].Grouped)
                    {
                        Grouping(node.Edge[vector]);
                    }
                }
            }

            foreach (var node in nodeList)
            {
                if (!node.Grouped)
                {
                    group = new List<Node>();
                    Grouping(node);
                    groupList.Add(group);
                }
            }

            groupList = groupList.OrderByDescending(g => g.Count()).ToList();
            var sum = groupList.Sum(g => g.Count);
            var excludeFukurokoji = groupList
                .Where(g =>
                    g.Exists(node => Array.IndexOf(node.Edge, null) != -1)
                )
                .ToList();

            var oneKatamukeList = new List<Node>[4];
            var katamukePoints = new int[4];
            for (int vector = 0; vector < 4; vector++)
            {
                var list = nodeList
                    .Where(node => node.Edge[vector] == null)
                    .Select(node => new KeyValuePair<Node, int>(node, node.Points[vector][Ends[vector][vector % 2 == 0 ? node.Coor.Y : node.Coor.X]]))
                    .OrderByDescending(item => item.Value)
                    .Where((point, rank) => rank < BallNum);
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
            foreach(var node in oneKatamukeList[maxVector])
            {
                Console.WriteLine(node.Coor.X + " " + node.Coor.Y);
            }
            Console.WriteLine(maxVector);



        }

        private static Coor AddVector(Coor coor1, Coor coor2)
        {
            return new Coor(coor1.X + coor2.X, coor1.Y + coor2.Y);
        }
        private static Coor MoveOne(Coor coor, int vector)
        {
            switch (vector)
            {
                case 0:
                    return new Coor(coor.X - 1, coor.Y);
                case 1:
                    return new Coor(coor.X, coor.Y - 1);
                case 2:
                    return new Coor(coor.X + 1, coor.Y);
                case 3:
                    return new Coor(coor.X, coor.Y + 1);
                default:
                    throw new Exception("存在しないベクトルです");
            }
        }

        private static bool IsOut(Coor coor, int vector)
        {
            switch (vector)
            {
                case 0:
                    return coor.X == -1;
                case 1:
                    return coor.Y == -1;
                case 2:
                    return coor.X == Size;
                case 3:
                    return coor.Y == Size;
                default:
                    throw new Exception("存在しないベクトルです");
            }
        }

        private static int StringToInt(string str)
        {
            switch (str)
            {
                case "r":
                    return 0;
                case "b":
                    return 1;
                case "g":
                    return 2;
                case "w":
                    return 3;
                default:
                    throw new Exception("存在しない色です");
            }
        }
    }
}
