using System;
using System.Runtime.InteropServices;

class Program
{
    // Direction constants
    static int LEFT = 0;
    static int RIGHT = 1;
    static int UP = 2;
    static int DOWN = 3;

    // Player 1 declarations
    static string Player1Name = "";
    static int Player1Score = 0;
    static int Player1Direction = RIGHT;
    static int Player1Column = 0;
    static int Player1Row = 0;

    // Player 2 declarations
    static string Player2Name = "";
    static int Player2Score = 0;
    static int Player2Direction = LEFT;
    static int Player2Column = 40;
    static int Player2Row = 5;

    static bool[,]? isUsed;
    
    // Default game dimensions
    static int gameWidth = 60;
    static int gameHeight = 20;

    static void GetPlayerNames()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter Player 1 (Yellow) name: ");
        Player1Name = Console.ReadLine()?.Trim() ?? "Player 1";
        if (string.IsNullOrEmpty(Player1Name)) Player1Name = "Player 1";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Enter Player 2 (Blue) name: ");
        Player2Name = Console.ReadLine()?.Trim() ?? "Player 2";
        if (string.IsNullOrEmpty(Player2Name)) Player2Name = "Player 2";

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

    static void SetupGame()
    {
        Console.Clear();
        
        Player1Column = 0;
        Player1Row = gameHeight / 2;

        Player2Column = gameWidth - 1;
        Player2Row = gameHeight / 2;
    }

    static void ShowStartScreen()
    {
        Console.Clear();
        string heading = "TRON GAME";
        
        Console.WriteLine(new string(' ', gameWidth / 2 - heading.Length / 2) + heading + "\n");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Player1Name}'s Controls:\n");
        Console.WriteLine("W - Up");
        Console.WriteLine("A - Left");
        Console.WriteLine("S - Down");
        Console.WriteLine("D - Right\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{Player2Name}'s Controls:\n");
        Console.WriteLine("Up Arrow - Up");
        Console.WriteLine("Left Arrow - Left");
        Console.WriteLine("Down Arrow - Down");
        Console.WriteLine("Right Arrow - Right\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Press any key to start...");
        Console.ReadKey(true);
        Console.Clear();
    }

    static void DisplayScore()
    {
        try
        {
            Console.SetCursorPosition(0, gameHeight + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{Player1Name}: {Player1Score}  ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{Player2Name}: {Player2Score}");
        }
        catch (Exception)
        {
            // Ignore any console errors
        }
    }

    static void UpdatePlayerMovement(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.W && Player1Direction != DOWN)
            Player1Direction = UP;
        if (key.Key == ConsoleKey.A && Player1Direction != RIGHT)
            Player1Direction = LEFT;
        if (key.Key == ConsoleKey.D && Player1Direction != LEFT)
            Player1Direction = RIGHT;
        if (key.Key == ConsoleKey.S && Player1Direction != UP)
            Player1Direction = DOWN;
        if (key.Key == ConsoleKey.UpArrow && Player2Direction != DOWN)
            Player2Direction = UP;
        if (key.Key == ConsoleKey.LeftArrow && Player2Direction != RIGHT)
            Player2Direction = LEFT;
        if (key.Key == ConsoleKey.RightArrow && Player2Direction != LEFT)
            Player2Direction = RIGHT;
        if (key.Key == ConsoleKey.DownArrow && Player2Direction != UP)
            Player2Direction = DOWN;
    }

    static void MovePlayers()
    {
        if (Player1Direction == RIGHT) Player1Column++;
        if (Player1Direction == LEFT) Player1Column--;
        if (Player1Direction == UP) Player1Row--;
        if (Player1Direction == DOWN) Player1Row++;
        if (Player2Direction == RIGHT) Player2Column++;
        if (Player2Direction == LEFT) Player2Column--;
        if (Player2Direction == UP) Player2Row--;
        if (Player2Direction == DOWN) Player2Row++;
    }

    static void WriteOnPosition(int x, int y, char ch, ConsoleColor color)
    {
        try
        {
            if (x >= 0 && x < gameWidth && y >= 0 && y < gameHeight)
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(x, y);
                Console.Write(ch);
            }
        }
        catch (Exception)
        {
            // Ignore any console errors
        }
    }

    static bool CheckCollision(int row, int column)
    {
        if (row < 0 || column < 0 || row >= gameHeight || column >= gameWidth)
            return true;

        if (isUsed?[column, row] == true)
            return true;

        return false;
    }

    static bool ResetGame()
    {
        Console.WriteLine("\nOptions:");
        Console.WriteLine("1. Play Again");
        Console.WriteLine("2. Quit");
        Console.Write("\nEnter your choice (1-2): ");
        
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.KeyChar == '1')
            {
                isUsed = new bool[gameWidth, gameHeight];
                SetupGame();
                Player1Direction = RIGHT;
                Player2Direction = LEFT;
                Console.Clear();
                return true;
            }
            else if (key.KeyChar == '2')
            {
                return false;
            }
        }
    }

    static void DrawBorder()
    {
        Console.ForegroundColor = ConsoleColor.White;
        // Draw top and bottom borders
        for (int i = 0; i < gameWidth; i++)
        {
            WriteOnPosition(i, 0, '-', ConsoleColor.White);
            WriteOnPosition(i, gameHeight - 1, '-', ConsoleColor.White);
        }
        // Draw side borders
        for (int i = 0; i < gameHeight; i++)
        {
            WriteOnPosition(0, i, '|', ConsoleColor.White);
            WriteOnPosition(gameWidth - 1, i, '|', ConsoleColor.White);
        }
    }

    static void Main(string[] args)
    {
        GetPlayerNames();
        SetupGame();
        ShowStartScreen();

        isUsed = new bool[gameWidth, gameHeight];
        bool continueGame = true;

        while (continueGame)
        {
            DrawBorder();
            DisplayScore();

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                UpdatePlayerMovement(key);
            }

            MovePlayers();
            
            bool player1Lost = CheckCollision(Player1Row, Player1Column);
            bool player2Lost = CheckCollision(Player2Row, Player2Column);

            if (player1Lost && player2Lost)
            {
                Player1Score++;
                Player2Score++;
                Console.WriteLine("\nGame Over!");
                Console.WriteLine("It's a tie!");
                Console.WriteLine($"Score: {Player1Name} {Player1Score} - {Player2Name} {Player2Score}");
                continueGame = ResetGame();
                continue;
            }

            if (player1Lost)
            {
                Player2Score++;
                Console.WriteLine("\nGame Over!");
                Console.WriteLine($"{Player2Name} Wins!");
                Console.WriteLine($"Score: {Player1Name} {Player1Score} - {Player2Name} {Player2Score}");
                continueGame = ResetGame();
                continue;
            }

            if (player2Lost)
            {
                Player1Score++;
                Console.WriteLine("\nGame Over!");
                Console.WriteLine($"{Player1Name} Wins!");
                Console.WriteLine($"Score: {Player1Name} {Player1Score} - {Player2Name} {Player2Score}");
                continueGame = ResetGame();
                continue;
            }

            if (isUsed != null)
            {
                isUsed[Player1Column, Player1Row] = true;
                isUsed[Player2Column, Player2Row] = true;
            }

            WriteOnPosition(Player1Column, Player1Row, '.', ConsoleColor.Yellow);
            WriteOnPosition(Player2Column, Player2Row, '.', ConsoleColor.Cyan);

            Thread.Sleep(100);
        }

        Console.Clear();
        Console.WriteLine("Thanks for playing!");
        Console.WriteLine($"Final Score: {Player1Score} - {Player2Score}");
        Console.WriteLine($"{Player1Name} : {Player1Score}");
        Console.WriteLine($"{Player2Name} : {Player2Score}");
    }
}