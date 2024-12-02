using System;
using System.Threading;
using System.Text;

namespace DrivingGame;

class Program
{
    static int width = 50;
    static int height = 30;

    static int windowWidth;
    static int windowHeight;
    static Random random = new();

    static char[,] scene;
    static int score = 0;
    static int highScore = 0;
    static int carPosition;
    static int carSpeed;
    static bool isPlaying;
    static bool continueGame = true;
    static bool consoleError = false;
    static int updateRoad = 0;

    // Unicode characters for better visuals
    const char CAR_LEFT = '◄';
    const char CAR_RIGHT = '►';
    const char CAR_STRAIGHT = '▲';
    const char CAR_CRASHED = '✕';
    const char ROAD_BORDER = '█';
    const char WALL_VERTICAL = '║';
    const char WALL_HORIZONTAL = '═';
    const char WALL_TOP_LEFT = '╔';
    const char WALL_TOP_RIGHT = '╗';
    const char WALL_BOTTOM_LEFT = '╚';
    const char WALL_BOTTOM_RIGHT = '╝';

    static void Init()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
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
            Console.BufferHeight = windowHeight;
        }
    }

    static void DrawBox(int x, int y, int w, int h, string title = "")
    {
        // Draw corners
        Console.SetCursorPosition(x, y);
        Console.Write(WALL_TOP_LEFT);
        Console.SetCursorPosition(x + w - 1, y);
        Console.Write(WALL_TOP_RIGHT);
        Console.SetCursorPosition(x, y + h - 1);
        Console.Write(WALL_BOTTOM_LEFT);
        Console.SetCursorPosition(x + w - 1, y + h - 1);
        Console.Write(WALL_BOTTOM_RIGHT);

        // Draw horizontal walls
        for (int i = 1; i < w - 1; i++)
        {
            Console.SetCursorPosition(x + i, y);
            Console.Write(WALL_HORIZONTAL);
            Console.SetCursorPosition(x + i, y + h - 1);
            Console.Write(WALL_HORIZONTAL);
        }

        // Draw vertical walls
        for (int i = 1; i < h - 1; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.Write(WALL_VERTICAL);
            Console.SetCursorPosition(x + w - 1, y + i);
            Console.Write(WALL_VERTICAL);
        }

        // Draw title if provided
        if (!string.IsNullOrEmpty(title))
        {
            Console.SetCursorPosition(x + (w - title.Length) / 2, y);
            Console.Write(title);
        }
    }

    static void PressEnterToContinue()
    {
    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.Enter:
                break;
            case ConsoleKey.Escape:
                continueGame = false;
                break;
            default: goto GetInput;
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
                    stringBuilder.Append(
                        !isPlaying ? CAR_CRASHED :
                        carSpeed < 0 ? CAR_LEFT :
                        carSpeed > 0 ? CAR_RIGHT :
                        CAR_STRAIGHT
                    );
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

        // Display score in a box
        DrawBox(width + 2, 0, 20, 5, " SCORE ");
        Console.SetCursorPosition(width + 4, 2);
        Console.Write($"Current: {score}");
        Console.SetCursorPosition(width + 4, 3);
        Console.Write($"Best: {highScore}");
    }

    static void UserInput()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.A or ConsoleKey.LeftArrow:
                    carSpeed = -1;
                    break;
                case ConsoleKey.D or ConsoleKey.RightArrow:
                    carSpeed = +1;
                    break;
                case ConsoleKey.W or ConsoleKey.UpArrow or ConsoleKey.Spacebar:
                    carSpeed = 0;
                    break;
                case ConsoleKey.Escape:
                    isPlaying = false;
                    continueGame = false;
                    break;
            }
        }
    }

    static void GameOverScreen()
    {
        Console.Clear();
        if (score > highScore)
        {
            highScore = score;
            DrawBox(10, 5, 40, 3, " NEW HIGH SCORE! ");
            Console.SetCursorPosition(15, 6);
            Console.Write($"Score: {score}");
            Thread.Sleep(2000);
        }

        DrawBox(10, 8, 40, 10, " GAME OVER ");
        
        Console.SetCursorPosition(15, 10);
        Console.WriteLine($"Final Score: {score}");
        Console.SetCursorPosition(15, 11);
        Console.WriteLine($"High Score: {highScore}");
        Console.SetCursorPosition(15, 13);
        Console.WriteLine("1. Play Again");
        Console.SetCursorPosition(15, 14);
        Console.WriteLine("2. Reset Score and Play");
        Console.SetCursorPosition(15, 15);
        Console.WriteLine("3. Quit");
        
        Console.SetCursorPosition(15, 16);
        Console.Write("Select option (1-3): ");

    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.D1:
                continueGame = true;
                break;
            case ConsoleKey.D2:
                score = 0;
                highScore = 0;
                continueGame = true;
                break;
            case ConsoleKey.D3 or ConsoleKey.Escape:
                continueGame = false;
                break;
            default:
                goto GetInput;
        }
    }

    static void Update()
    {
        for (int i = 0; i < height - 1; i++)
        {
            for (int j = 0; j < width; j++)
            {
                scene[i, j] = scene[i + 1, j];
            }
        }
        int roadUpdate =
            random.Next(5) < 4 ? updateRoad :
            random.Next(3) - 1;
        if (roadUpdate is -1 && scene[height - 1, 0] is ' ') roadUpdate = 0;
        if (roadUpdate is 1 && scene[height - 1, width - 1] is ' ') roadUpdate = width - 1;
        switch (roadUpdate)
        {
            case -1:
                for (int i = 0; i < width - 1; i++)
                {
                    scene[height - 1, i] = scene[height - 1, i + 1];
                }
                scene[height - 1, width - 1] = ROAD_BORDER;
                break;
            case 1:
                for (int i = width - 1; i > 0; i--)
                {
                    scene[height - 1, i] = scene[height - 1, i - 1];
                }
                scene[height - 1, 0] = ROAD_BORDER;
                break;
        }
        carPosition += carSpeed;
        if (carPosition < 1) carPosition = 1;
        if (carPosition > width - 2) carPosition = width - 2;

        if (scene[1, carPosition] == ROAD_BORDER)
        {
            isPlaying = false;
            return;
        }

        score++;
    }

    static void InitScene()
    {
        const int roadWidth = 10;
        isPlaying = true;
        carPosition = width / 2;
        int leftBorder = (width - roadWidth) / 2;
        int rightBorder = leftBorder + roadWidth + 1;
        scene = new char[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j < leftBorder || j > rightBorder)
                {
                    scene[i, j] = ROAD_BORDER;
                }
                else
                {
                    scene[i, j] = ' ';
                }
            }
        }
    }

    static void ShowIntro()
    {
        Console.Clear();
        
       string title = @"
   ____       _       _             ____                      
  |  _ \ _ __(_)_   _(_)_ __   __ / ___| __ _ _ __ ___   ___ 
  | | | | '__| \ \ / / | '_ \ / _ \___ \/ _` | '_ ` _ \ / _ \
  | |_| | |  | |\ V /| | | | |  __/___) | (_| | | | | | |  __/
  |____/|_|  |_| \_/ |_|_| |_|\___|____/ \__,_|_| |_| |_|\___|
                                                             
";
        Console.WriteLine(title);
        
        DrawBox(5, 8, 50, 12, " HOW TO PLAY ");
        
        Console.SetCursorPosition(8, 10);
        Console.WriteLine("Guide your car to stay on the road and survive");
        Console.SetCursorPosition(8, 12);
        Console.WriteLine("Controls:");
        Console.SetCursorPosition(8, 13);
        Console.WriteLine($"◄ A or Left Arrow  - Move Left");
        Console.SetCursorPosition(8, 14);
        Console.WriteLine($"► D or Right Arrow - Move Right");
        Console.SetCursorPosition(8, 15);
        Console.WriteLine($"▲ W or Up Arrow    - Stay Straight");
        Console.SetCursorPosition(8, 16);
        Console.WriteLine("ESC               - Quit Game");
        
        Console.SetCursorPosition(8, 18);
        Console.WriteLine("Press [Enter] to start...");
        PressEnterToContinue();
    }

    static void Main()
    {
        Console.CursorVisible = false;
        try
        {
            Init();
            ShowIntro();
            while (continueGame)
            {
                InitScene();
                while (isPlaying)
                {
                    if (Console.WindowHeight < height || Console.WindowWidth < width)
                    {
                        consoleError = true;
                        continueGame = false;
                        break;
                    }
                    UserInput();
                    Update();
                    Render();
                    if (isPlaying)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(33));
                    }
                }
                if (continueGame)
                {
                    GameOverScreen();
                }
            }
            Console.Clear();
            if (consoleError)
            {
                DrawBox(5, 5, 50, 5, " ERROR ");
                Console.SetCursorPosition(7, 7);
                Console.WriteLine("Console/Terminal window too small");
                Console.SetCursorPosition(7, 8);
                Console.WriteLine($"Minimum size required: {width} x {height}");
            }
            else
            {
                DrawBox(5, 5, 50, 7, " GAME OVER ");
                Console.SetCursorPosition(7, 7);
                Console.WriteLine("Thanks for playing!");
                Console.SetCursorPosition(7, 8);
                Console.WriteLine($"Final Score: {score}");
                Console.SetCursorPosition(7, 9);
                Console.WriteLine($"High Score: {highScore}");
            }
            Console.SetCursorPosition(0, 13);
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }
}