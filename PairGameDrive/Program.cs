using System;
using System.Text;

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
        static void Initialize()
        {
            windowWidth = Console.WindowWidth;
            windowHeight = Console.WindowHeight;
            if (OperatingSystem.IsWindows())
            {
                if (windowWidth < width && OperatingSystem.IsWindows())
                {
                    windowWidth = Console.WindowWidth = width + 1;
                }
                if (windowHeight < height && OperatingSystem.IsWindows())
                {
                    windowHeight = Console.WindowHeight = height + 1;
                }
                Console.BufferWidth = windowWidth;
                Console.BufferWidth = windowHeight;
            }
        }
        static void InitializeScene()
        {
            const int roadWidth = 10;
            gameRunning = true;
            carPosition = width / 2;
            carVelocity = 0;
            int leftEdge = (width - roadWidth) / 2;
            int rightEdge = leftEdge + roadWidth + 1;
            scene = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j < leftEdge || j > rightEdge)
                    {
                        scene[i, j] = '.';
                    }
                    else
                    {
                        scene[i, j] = ' ';
                    }
                }
            }
        }
        static void Render()
        {
            StringBuilder stringBuilder = new(width * height);
            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 1 && j == carPosition)
                    {
                        stringBuilder.Append(!gameRunning ? 'X' :
                            carVelocity < 0 ? '<' :
                            carVelocity > 0 ? '>' :
                            '^');
                    }
                    else
                    {
                        stringBuilder.Append(scene[i, j]);
                    }
                }
                if (i > 0)
                {
                    stringBuilder.AppendLine();
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder);
        }
        static void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.A or ConsoleKey.LeftArrow:
                        carVelocity = -1;
                        break;
                    case ConsoleKey.D or ConsoleKey.RightArrow:
                        carVelocity = +1;
                        break;
                    case ConsoleKey.W or ConsoleKey.UpArrow or ConsoleKey.S or ConsoleKey.DownArrow:
                        carVelocity = 0;
                        break;
                    case ConsoleKey.Escape:
                        gameRunning = false;
                        keepPlaying = false;
                        break;
                    case ConsoleKey.Enter:
                        Console.ReadLine();
                        break;
                }
            }
        }

    }
}