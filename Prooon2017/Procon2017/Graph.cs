using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    public class Graph
    {
        public static int Size;
        public static int BallNum;
        public static string[][] Ends;
        public static string[,] OriginalBoad;

        public static void InitializeField()
        {
            Size = int.Parse(Console.In.ReadLine());
            BallNum = int.Parse(Console.In.ReadLine());

            OriginalBoad = new string[Size, Size];
            Ends = new string[4][];
            for (int i = 0; i < Size; i++)
            {
                var line = (Console.In.ReadLine()).Split(' ')
                    .ToArray();
                for (int j = 0; j < Size; j++)
                {
                    OriginalBoad[j, i] = line[j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Ends[i] = (Console.In.ReadLine()).Split(' ');
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
                    if (OriginalBoad[x, y] == "w")
                    {
                        var coor = new Coor(x, y);
                        for (int vector = 0; vector < 4; vector++)
                        {
                            var nextCoor = MoveOne(coor, vector);
                            if (!IsOut(nextCoor, vector) &&
                                OriginalBoad[nextCoor.X, nextCoor.Y] != "w" &&
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
            foreach (var item in nodeList.Select((v, i) => new { v, i }))
            {
                for (int vector = 0; vector < 4; vector++)
                {
                    var coor = item.v.Coor;

                    while (true)
                    {
                        coor = MoveOne(coor, vector);

                        if (IsOut(coor, vector))
                        {
                            break;
                        }
                        if (OriginalBoad[coor.X, coor.Y] == "w")
                        {
                            //一歩戻る
                            coor = MoveOne(coor, (vector + 2) % 4);
                            item.v.Edge[vector] = nodes[coor.X, coor.Y];
                            break;
                        }
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
            //出力
            //for (int i = 0; i < nodeList.Count(); i++)
            //{
            //    for (int j = 0; j < nodeList.Count(); j++)
            //    {
            //        Console.Write(matrix[i, j] + " ");
            //    }
            //    Console.Write("\n");

            //}



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
    }
}
