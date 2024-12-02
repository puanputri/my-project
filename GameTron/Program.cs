using System;
using System.Runtime.InteropServices;

class Program
{
    static int kiri = 0;
    static int kanan = 1;
    static int atas = 2;
    static int bawah = 3;

    // Player declarations
    static int SkorPemain1 = 0;
    static int ArahPemain1 = kanan;
    static int KolomPemain1 = 0;
    static int BarisPemain1 = 0;

    static int SkorPemain2 = 0;
    static int ArahPemain2 = kiri;
    static int KolomPemain2 = 40;
    static int BarisPemain2 = 5;

    static bool[,]? isUsed;
    
    // Default game dimensions - smaller size for better compatibility
    static int gameWidth = 60;
    static int gameHeight = 20;

    static void AturLayarPermainan()
    {
        // Simply clear the screen and initialize player positions
        Console.Clear();
        
        KolomPemain1 = 0;
        BarisPemain1 = gameHeight / 2;

        KolomPemain2 = gameWidth - 1;
        BarisPemain2 = gameHeight / 2;
    }

    static void LayarAwal()
    {
        Console.Clear();
        string heading = "Game Tron";
        
        // Center the heading approximately
        Console.WriteLine(new string(' ', gameWidth / 2 - heading.Length / 2) + heading + "\n");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Kontrol Pemain 1:\n");
        Console.WriteLine("W - atas");
        Console.WriteLine("A - kiri");
        Console.WriteLine("S - bawah");
        Console.WriteLine("D - kanan\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Kontrol Pemain 2:\n");
        Console.WriteLine("Panah Atas - atas");
        Console.WriteLine("Panah Kiri - kiri");
        Console.WriteLine("Panah Bawah - bawah");
        Console.WriteLine("Panah Kanan - kanan\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Tekan sembarang tombol untuk mulai...");
        Console.ReadKey(true);
        Console.Clear();
    }

    static void UbahGerakanPemain(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.W && ArahPemain1 != bawah)
            ArahPemain1 = atas;
        if (key.Key == ConsoleKey.A && ArahPemain1 != kanan)
            ArahPemain1 = kiri;
        if (key.Key == ConsoleKey.D && ArahPemain1 != kiri)
            ArahPemain1 = kanan;
        if (key.Key == ConsoleKey.S && ArahPemain1 != atas)
            ArahPemain1 = bawah;
        if (key.Key == ConsoleKey.UpArrow && ArahPemain2 != bawah)
            ArahPemain2 = atas;
        if (key.Key == ConsoleKey.LeftArrow && ArahPemain2 != kanan)
            ArahPemain2 = kiri;
        if (key.Key == ConsoleKey.RightArrow && ArahPemain2 != kiri)
            ArahPemain2 = kanan;
        if (key.Key == ConsoleKey.DownArrow && ArahPemain2 != atas)
            ArahPemain2 = bawah;
    }

    static void GerakanPemain()
    {
        if (ArahPemain1 == kanan) KolomPemain1++;
        if (ArahPemain1 == kiri) KolomPemain1--;
        if (ArahPemain1 == atas) BarisPemain1--;
        if (ArahPemain1 == bawah) BarisPemain1++;
        if (ArahPemain2 == kanan) KolomPemain2++;
        if (ArahPemain2 == kiri) KolomPemain2--;
        if (ArahPemain2 == atas) BarisPemain2--;
        if (ArahPemain2 == bawah) BarisPemain2++;
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

    static bool CekKalah(int baris, int kolom)
    {
        if (baris < 0 || kolom < 0 || baris >= gameHeight || kolom >= gameWidth)
            return true;

        if (isUsed?[kolom, baris] == true)
            return true;

        return false;
    }

    static void ResetGame()
    {
        isUsed = new bool[gameWidth, gameHeight];
        AturLayarPermainan();
        ArahPemain1 = kanan;
        ArahPemain2 = kiri;
        Console.WriteLine("\nTekan sembarang key untuk melanjutkan...");
        Console.ReadKey();
        Console.Clear();
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
        AturLayarPermainan();
        LayarAwal();

        isUsed = new bool[gameWidth, gameHeight];

        while (true)
        {
            DrawBorder();  // Add border to visualize game boundaries

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                UbahGerakanPemain(key);
            }

            GerakanPemain();
            
            bool pemain1Kalah = CekKalah(BarisPemain1, KolomPemain1);
            bool pemain2Kalah = CekKalah(BarisPemain2, KolomPemain2);

            if (pemain1Kalah && pemain2Kalah)
            {
                SkorPemain1++;
                SkorPemain2++;
                Console.WriteLine("\nPermainan Berakhir");
                Console.WriteLine("Seri!!!");
                Console.WriteLine($"Skor: {SkorPemain1} - {SkorPemain2}");
                ResetGame();
                continue;
            }

            if (pemain1Kalah)
            {
                SkorPemain2++;
                Console.WriteLine("\nPermainan Berakhir");
                Console.WriteLine("Pemain 2 Menang!!!");
                Console.WriteLine($"Skor: {SkorPemain1} - {SkorPemain2}");
                ResetGame();
                continue;
            }

            if (pemain2Kalah)
            {
                SkorPemain1++;
                Console.WriteLine("\nPermainan Berakhir");
                Console.WriteLine("Pemain 1 Menang!!!");
                Console.WriteLine($"Skor: {SkorPemain1} - {SkorPemain2}");
                ResetGame();
                continue;
            }

            if (isUsed != null)
            {
                isUsed[KolomPemain1, BarisPemain1] = true;
                isUsed[KolomPemain2, BarisPemain2] = true;
            }

            WriteOnPosition(KolomPemain1, BarisPemain1, '*', ConsoleColor.Yellow);
            WriteOnPosition(KolomPemain2, BarisPemain2, '*', ConsoleColor.Cyan);

            Thread.Sleep(100);
        }
    }
}