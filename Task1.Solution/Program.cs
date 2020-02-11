using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "..\\..\\TestData\\Map6.txt";
            DrawMap(path);
        }

        static void DrawMap(string path)
        {
            Console.SetWindowSize(40, 20);
            Console.SetBufferSize(41, 21);

            string[] allLines = File.ReadAllLines(path);

            int[] objectPosition = null, bridgePosition = null;

            foreach (string line in allLines)
            {
                if (line != "")
                {
                    string[] content = line.Replace("  ", " ").ToLower().Split('(');

                    string keyWord = content[0].Trim();
                    string positionLine = content[1].Replace(')', ' ').Trim();

                    if (keyWord == "bridge")
                    {
                        bridgePosition = FindBridgeOrTreasurePosition(positionLine);
                    }
                    else if (keyWord == "treasure")
                    {
                        objectPosition = FindBridgeOrTreasurePosition(positionLine);
                        DrawBridgeOrTreasure(keyWord, objectPosition);
                    }
                    else if (keyWord == "base")
                    {
                        objectPosition = FindBasePosition(positionLine);
                        DrawBase(objectPosition);
                    }
                    else if (keyWord == "water")
                    {
                        objectPosition = FindWaterPosition(positionLine);
                        DrawWater(objectPosition);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Error. This object doesn`t exist.");
                        Environment.Exit(0);
                    }
                }
            }
            DrawBridgeOrTreasure("bridge", bridgePosition);

            Console.CursorVisible = false;
            Console.ReadKey();
        }

        static void CheckConsoleSize(int width, int heigth)
        {
            if (width > Console.WindowWidth)
            {
                Console.SetWindowSize(width + 1, Console.WindowHeight + 1);
                Console.SetBufferSize(width + 2, Console.WindowHeight + 2);
            }

            if (heigth > Console.WindowHeight)
            {
                Console.SetWindowSize(Console.WindowWidth + 1, heigth + 1);
                Console.SetBufferSize(Console.WindowWidth + 2, heigth + 2);
            }
        }

        static int[] FindBridgeOrTreasurePosition(string positionLine)
        {
            CultureInfo provider = new CultureInfo("en-US");
            NumberStyles styles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

            int[] position = new int[2];
            string[] pair = positionLine.Split(',');

            bool convertSuccess;

            for (int i = 0; i < 2; i++)
            {
                convertSuccess = int.TryParse(pair[i], styles, provider, out position[i]);
                if (!convertSuccess)
                {
                    Console.Clear();
                    Console.WriteLine("Error. Incorrect format of the string!");
                    Environment.Exit(0);
                }
            }

            return position;
        }

        static int[] FindWaterPosition(string positionLine)
        {
            string[] coordinates = positionLine.Split('>');
            int length = 2 * coordinates.Length;

            int[] position = new int[length];
            int i = 0;

            bool convertSuccess;
            CultureInfo provider = new CultureInfo("en-US");
            NumberStyles styles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

            foreach (string pairString in coordinates)
            {

                string tmpString = pairString.Replace('-', ' ');
                string[] pair = tmpString.Split(',');

                for (int j = 0; j < 2; j++)
                {
                    convertSuccess = int.TryParse(pair[j], styles, provider, out position[i++]);
                    if (!convertSuccess)
                    {
                        Console.Clear();
                        Console.WriteLine("Error. Incorrect format of the string!");
                        Environment.Exit(0);
                    }
                }
            }

            return position;
        }

        static int[] FindBasePosition(string positionLine)
        {
            int[] position = new int[4];
            int i = 0;

            bool convertSuccess;
            CultureInfo provider = new CultureInfo("en-US");
            NumberStyles styles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

            string[] coordinates = positionLine.Split(':');
            foreach (string pairString in coordinates)
            {
                string[] pair = pairString.Split(',');

                for (int j = 0; j < 2; j++)
                {
                    convertSuccess = int.TryParse(pair[j], styles, provider, out position[i++]);
                    if (!convertSuccess)
                    {
                        Console.Clear();
                        Console.WriteLine("Error. Incorrect format of the string!");
                        Environment.Exit(0);
                    }
                }
            }
            return position;
        }

        static void DrawBridgeOrTreasure(string keyWord, int[] position)
        {
            CheckConsoleSize(position[0], position[1]);
            Console.SetCursorPosition(position[0], position[1]);
            if (keyWord == "treasure")
                Console.Write('+');
            else
                Console.Write('#');
        }

        static void DrawWater(int[] position)
        {
            int length = position.Length;
            for (int k = 0; k <= length - 4; k += 2)
            {
                double dX = position[k + 2] - position[k],
                       dY = position[k + 3] - position[k + 1];

                CheckConsoleSize(position[k + 2], position[k + 3]);
                CheckConsoleSize(position[k], position[k + 1]);

                double maxDelta = Math.Max(Math.Abs(dX), Math.Abs(dY));

                dX /= maxDelta;
                dY /= maxDelta;

                double x = position[k], y = position[k + 1];

                int pointerX = position[k],
                    pointerY = position[k + 1];

                while (pointerX != position[k + 2] || pointerY != position[k + 3])
                {
                    Console.SetCursorPosition(pointerX, pointerY);
                    Console.Write('~');

                    x += dX;
                    y += dY;

                    pointerX = (int)Math.Round(x);
                    pointerY = (int)Math.Round(y);
                }
                Console.SetCursorPosition(pointerX, pointerY);
                Console.Write('~');
            }
        }


        static void DrawBase(int[] position)
        {
            int width = position[2] - position[0] + 1;
            int heigth = position[3] - position[1] + 1;

            CheckConsoleSize(position[2], position[3]);

            for (int w = 0; w < heigth; w++)
            {
                Console.SetCursorPosition(position[0], position[1] + w);
                for (int l = 0; l < width; l++)
                {
                    Console.SetCursorPosition(position[0] + l, position[1] + w);
                    Console.Write('@');
                }
            }
        }
    }
}
