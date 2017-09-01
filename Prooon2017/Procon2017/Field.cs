using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017
{
    public static class Field
    {
        public static int Size;
        public static int BallNum;
        public static int[,] Boad;
        public static int[][] Ends;


        public static void InitializeField()
        {
            Size = int.Parse(Console.In.ReadLine());
            BallNum = int.Parse(Console.In.ReadLine());

            Boad = new int[Size, Size];
            Ends = new int[4][];
            for (int i = 0; i < Size; i++)
            {
                var line = (Console.In.ReadLine()).Split(' ')
                    .ToArray();
                for (int j = 0; j < Size; j++)
                {
                    Boad[j, i] = StringToInt(line[j]);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Ends[i] = (Console.In.ReadLine()).Split(' ').Select(str => StringToInt(str)).ToArray();
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
