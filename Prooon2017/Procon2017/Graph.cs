using System;
using System.Collections.Generic;
using System.Linq;

namespace Procon2017
{
    public class Graph
    {

        public static Node[,] NodeMap;
        public static List<Node> Nodes;
        public static int?[,] Matrix;
        public static List<List<Node>> Groups;
        public static void CreateGraph()
        {
            NodeMap = new Node[Field.Size, Field.Size];
            Nodes = new List<Node>();

            //ノードの抽出
            for (int x = 0; x < Field.Size; x++)
            {
                for (int y = 0; y < Field.Size; y++)
                {
                    if (Field.Boad[x, y] == 3)
                    {
                        var coor = new Coor(x, y);
                        for (int vector = 0; vector < 4; vector++)
                        {
                            var nextCoor = MoveOne(coor, vector);
                            if (!IsOut(nextCoor, vector) &&
                                Field.Boad[nextCoor.X, nextCoor.Y] != 3 &&
                                NodeMap[nextCoor.X, nextCoor.Y] == null
                                )
                            {
                                var node = new Node()
                                {
                                    Coor = nextCoor,
                                    Index = Nodes.Count()
                                };
                                NodeMap[nextCoor.X, nextCoor.Y] = node;
                                Nodes.Add(node);
                            }
                        }
                    }
                }
            }

            //ノードの紐つけ
            //foreach (var item in Nodes.Select((v, i) => new { v, i }))
            foreach (var node in Nodes)
            {
                for (int vector = 0; vector < 4; vector++)
                {
                    var coor = node.Coor;
                    node.Points[vector][Field.Boad[coor.X, coor.Y]]++;
                    var points = new int[3];
                    while (true)
                    {
                        coor = MoveOne(coor, vector);

                        if (IsOut(coor, vector))
                        {
                            break;
                        }
                        if (Field.Boad[coor.X, coor.Y] == 3)
                        {
                            //一歩戻る
                            coor = MoveOne(coor, (vector + 2) % 4);
                            node.Edge[vector] = NodeMap[coor.X, coor.Y];
                            node.Points[vector][Field.Boad[coor.X, coor.Y]]--;
                            break;
                        }
                        node.Points[vector][Field.Boad[coor.X, coor.Y]]++;
                    }
                }
            }

            //行列作成
            Matrix= new int?[Nodes.Count(), Nodes.Count()];
            foreach (var item in Nodes.Select((v, i) => new { v, i }))
            {
                for (int vector = 0; vector < 4; vector++)
                {
                    if (item.v.Edge[vector] != null)
                    {
                        Matrix[item.i, item.v.Edge[vector].Index] = vector;
                    }
                }
            }

            //グループ分け
            Groups = new List<List<Node>>();
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

            foreach (var node in Nodes)
            {
                if (!node.Grouped)
                {
                    group = new List<Node>();
                    Grouping(node);
                    Groups.Add(group);
                }
            }

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
                    return coor.X == Field.Size;
                case 3:
                    return coor.Y == Field.Size;
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
