﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    public class Field
    {
        //静的メンバ
        public static int Size;
        public static int BallNum;
        public static Tile.TileState[][] Ends;
        public static Tile[,] OriginalBoad;

        public int[] MaxRoute { get; set; }

        public int MaxPoint { get; set; }

        public Field()
        {

        }

        public void CalculateAllRoute(Coor[] startBallPosition)
        {
            var boad = new BoadState[Size, Size];
            var points = new int[3];
            var balls = new Ball[BallNum];
            var Route = new List<int>();
            //初期位置設定
            for (int i = 0; i < BallNum; i++)
            {
                balls[i] = new Ball() { Coor = startBallPosition[i] };

                var startTile = OriginalBoad[startBallPosition[i].X, startBallPosition[i].Y];
                balls[i].Points[(int)startTile.State] = 1;
                boad[startBallPosition[i].X, startBallPosition[i].Y].IsPassed = true;
            }
            CoverAllIncline(-1, boad, points, balls, Route);
        }

        public void CoverAllIncline(int lastIncline, BoadState[,] lastBoad, int[] lastPoints, Ball[] lastBalls, List<int> lastRoute)
        {
            if (lastRoute.Count() > 2 && lastRoute[0] == 3 && lastRoute[1] == 2 && lastRoute[2] == 3)
            {

            }
            //全ての玉が外に出たら最大値を比較して、終了
            if (lastBalls.FirstOrDefault(b => b.IsOut == false) == null)
            {
                if (lastPoints.Sum() == 17)
                {

                }
                var tmpPointSum = lastPoints.Sum();
                if (tmpPointSum > MaxPoint)
                {
                    MaxPoint = tmpPointSum;
                    MaxRoute = lastRoute.ToArray();
                }
                return;
            }

            var currentBoad = new BoadState[Size, Size];

            var currentPoints = new int[3];

            var currentBalls = new Ball[BallNum];

            List<int> currentRoute = null;

            CopyDataToCurrent(lastBoad, lastPoints, lastBalls, lastRoute, currentBoad, currentPoints, currentBalls, ref currentRoute);
            for (int i = 0; i < 4; i++)
            {
                //前回と同じ操作はしない
                if (i == lastIncline)
                {
                    continue;
                }
                //1回傾ける ポイントがある場合
                if (InclineField(i, currentBoad, currentPoints, currentBalls, currentRoute))
                {

                    //次へ
                    CoverAllIncline(i, currentBoad, currentPoints, currentBalls, currentRoute);
                    //終わったら元に戻して別の方向へ
                    CopyDataToCurrent(lastBoad, lastPoints, lastBalls, lastRoute, currentBoad, currentPoints, currentBalls, ref currentRoute);

                }
                //ポイントが無い場合、元に戻して別方向へ
                else
                {
                    //全方向動かした場合は何もせず終了(重複ループ)
                    if (i == 3 || ( lastIncline == 3 && i == 2 ))
                    {
                        return;
                    }
                    //元に戻す
                    CopyDataToCurrent(lastBoad, lastPoints, lastBalls, lastRoute, currentBoad, currentPoints, currentBalls, ref currentRoute);
                    continue;
                }

            }
            //全パターン回ったら終了
            //無限ループ？起きないはず
            return;
        }

        /// <summary>
        /// ベクトルの方向に1回傾け
        /// ポイントが増える移動をした場合はtrue
        /// </summary>
        /// <param name="vector"></param>
        public bool InclineField(int vector, BoadState[,] boad, int[] points, Ball[] balls, List<int> route)
        {
            var includesPoints = false;
            switch (vector)
            {
            case 0:
                balls = balls.OrderBy(b => b.Coor.X).ToArray();
                break;
            case 1:
                balls = balls.OrderBy(b => b.Coor.Y).ToArray();
                break;
            case 2:
                balls = balls.OrderByDescending(b => b.Coor.X).ToArray();
                break;
            case 3:
                balls = balls.OrderByDescending(b => b.Coor.Y).ToArray();
                break;
            }

            for (int i = 0; i < BallNum; i++)
            {
                if (balls[i].IsOut)
                {
                    continue;
                }
                boad[balls[i].Coor.X, balls[i].Coor.Y].IsBall = false;
                //壁にぶつかるかボールにぶつかるか外に出るまで移動
                while (true)
                {
                    var next = AddVector(balls[i].Coor, MoveOne(vector));

                    if (IsOut(next, vector))
                    {

                        //外に出た場合、ポイントを加算して終了
                        balls[i].IsOut = true;
                        var endStatus = Ends[vector][vector % 2 == 0 ? next.Y : next.X];
                        points[(int)endStatus] += balls[i].Points[(int)endStatus];
                        includesPoints = true;

                        break;
                    }
                    else if (OriginalBoad[next.X, next.Y].State == Tile.TileState.Wall || boad[next.X, next.Y].IsBall)
                    {
                        boad[balls[i].Coor.X, balls[i].Coor.Y].IsBall = true;
                        break;
                    }
                    else
                    {
                        if (!boad[next.X, next.Y].IsPassed)
                        {
                            balls[i].Points[(int)OriginalBoad[next.X, next.Y].State]++;
                            boad[next.X, next.Y].IsPassed = true;
                            includesPoints = true;
                        }
                        balls[i].Coor = next;

                    }
                }
            }

            route.Add(vector);

            return includesPoints;

        }

        //lastの値をcurrentにコピーします
        private void CopyDataToCurrent(BoadState[,] lastBoad, int[] lastPoints, Ball[] lastBalls, List<int> lastRoute, BoadState[,] currentBoad, int[] currentPoints, Ball[] currentBalls, ref List<int> currentRoute)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    currentBoad[x, y] = lastBoad[x, y];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                currentPoints[i] = lastPoints[i];
            }
            for (int i = 0; i < BallNum; i++)
            {
                currentBalls[i] = new Ball()
                {
                    Coor = lastBalls[i].Coor,
                    IsOut = lastBalls[i].IsOut
                };
                for (int j = 0; j < 3; j++)
                {
                    currentBalls[i].Points[j] = lastBalls[i].Points[j];
                }
            }
            currentRoute = new List<int>(lastRoute);
        }

        private Coor AddVector(Coor coor1, Coor coor2)
        {
            return new Coor(coor1.X + coor2.X, coor1.Y + coor2.Y);
        }
        private Coor MoveOne(int vector)
        {
            switch (vector)
            {
            case 0:
                return new Coor(-1, 0);
            case 1:
                return new Coor(0, -1);
            case 2:
                return new Coor(1, 0);
            case 3:
                return new Coor(0, 1);
            default:
                throw new Exception("存在しないベクトルです");
            }
        }

        private bool IsOut(Coor coor, int vector)
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


        //入力
        public static void InitializeField()
        {
            Size = int.Parse(Console.In.ReadLine());
            BallNum = int.Parse(Console.In.ReadLine());

            OriginalBoad = new Tile[Size, Size];
            Ends = new Tile.TileState[4][];
            for (int i = 0; i < Size; i++)
            {
                var line = ( Console.In.ReadLine() ).Split(' ')
                    .Select((c, x) => new Tile()
                    {
                        X = x,
                        Y = i,
                        State = ConvertCharToState(c)
                    })
                    .ToArray();
                for (int j = 0; j < Size; j++)
                {
                    OriginalBoad[j, i] = line[j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                var line = ( Console.In.ReadLine() ).Split(' ')
                    .Select((c, x) => ConvertCharToState(c));
                Ends[i] = line.ToArray();
            }
        }

        static private Tile.TileState ConvertCharToState(string c)
        {
            switch (c)
            {
            case "r":
                return Tile.TileState.Red;
            case "b":
                return Tile.TileState.Blue;
            case "g":
                return Tile.TileState.Green;
            case "w":
                return Tile.TileState.Wall;
            default:
                throw new Exception("[" + c + "]" + "は存在しないカラーです");
            }
        }

    }
}
