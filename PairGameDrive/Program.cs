using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairGameDrive
{
    class Program
    {
        static readonly int width = 50;
        static readonly int height = 30;

        static int windowWidth;
        static int windowHeight;
        static readonly Random random = new();
        static char[,] scene;
        static int score;
        static int carPosition;
        static int carVelocity;
        static bool gameRunning;
        static bool keepPlaying = true;
        static bool consoleSizeError = false;
        static int previousRoadUpdate;

        static void Main()
        {
            Console.CursorVisible = false;
            try
            {
                Initialize();
                LaunchScreen();
                while (keepPlaying)
                {
                    InitializeScene();
                    while (gameRunning)
                    {
                        if (Console.WindowHeight < height || Console.WindowWidth < width)
                        {
                            consoleSizeError = true;
                            keepPlaying = false;
                            break;
                        }
                        HandleInput();
                        Update();
                        Render();
                        if (gameRunning)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(33));
                        }
                    }
                    if (keepPlaying)
                    {
                        GameOverScreen();
                    }
                }
                Console.Clear();
                if (consoleSizeError)
                {
                    Console.WriteLine("To play 'Drive' please increase the size of your Terminal.");
                    Console.WriteLine($"Minimum size is {width} width x {height} height.");
                    Console.WriteLine("Increase the size of the console.");
                }
                Console.WriteLine("'Drive' was closed.");
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}