using System;
using System.Threading;

namespace SimpleRPG
{
    class UI
    {
        public static void ShowTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"

╭──────────────────── ⟡ RPG Battle ⟡ ────────────────────╮
    ║              ██████╗  ██████╗  ██████╗                 
    ║              ██╔══██╗██╔══██╗██╔════╝                  
    ║              ██████╔╝██████╔╝██║  ███╗                 
    ║              ██╔══██╗██╔═══╝ ██║   ██║                 
    ║              ██║  ██║██║     ╚██████╔╝                 
    ║              ╚═╝  ╚═╝╚═╝      ╚═════╝                  
    ║                                             
    ║  ██████╗  █████╗ ████████╗████████╗██╗     ███████╗  
    ║  ██╔══██╗██╔══██╗╚══██╔══╝╚══██╔══╝██║     ██╔════╝  
    ║  ██████╔╝███████║   ██║      ██║   ██║     █████╗    
    ║  ██╔══██╗██╔══██║   ██║      ██║   ██║     ██╔══╝    
    ║  ██████╔╝██║  ██║   ██║      ██║   ███████╗███████╗  
    ║  ╚═════╝ ╚═╝  ╚═╝   ╚═╝      ╚═╝   ╚══════╝╚══════╝  
    ║                                                
    ║
    ║      /  \    /\        /  \     /\      /  \    /\
      /\  /    \  /  \  /\  /    \   /  \    /    \  /  \
     /  \/      \/    \/  \/      \ /    \  /      \/    \
                  [Welcome to the Adventure!! ]
    ╰───────────────────────────────────────────────────────────╯");
            Console.ResetColor();
            Console.WriteLine("\n          ⟡━━━━━⟡ Press any key to begin your journey ⟡━━━━━⟡");
            Console.ReadKey(true);
        }

        public static void ShowCharacterSelection()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    ╔══════════════════[ SELECT YOUR HERO ]══════════════════╗
    ║                                                        ║
    ║  [1] WARRIOR ⚔️                                        ║
    ║      HP: 100  │  ATK: 15                              ║
    ║      Special: Power Strike (1.5x damage)              ║
    ║                                                        ║
    ║  [2] ARCHER 🏹                                        ║
    ║      HP: 80   │  ATK: 20                              ║
    ║      Special: Quick Shot (1.5x damage)                ║
    ║                                                        ║
    ║  [3] MAGE 🔮                                          ║
    ║      HP: 70   │  ATK: 25                              ║
    ║      Special: Fireball (1.5x damage)                  ║
    ║                                                        ║
    ╚════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public static void ShowEnemySelection()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
    ╔══════════════════[ CHOOSE YOUR ENEMY ]═════════════════╗
    ║                                                        ║
    ║  [1] SKELETON 💀                                       ║
    ║      HP: 50   │  ATK: 10                              ║
    ║                                                        ║
    ║  [2] GIANT CRAB 🦀                                    ║
    ║      HP: 80   │  ATK: 12                              ║
    ║                                                        ║
    ╚════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public static void ShowHealthBar(string name, int health, int maxHealth, bool isPlayer)
        {
            Console.Write($"    {name}: ");
            Console.Write("[");
            
            int barLength = 20;
            int filledLength = (int)((health / (float)maxHealth) * barLength);
            
            Console.ForegroundColor = isPlayer ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(new string('█', filledLength));
            Console.Write(new string('░', barLength - filledLength));
            Console.ResetColor();
            
            Console.WriteLine($"] {health}/{maxHealth} HP");
        }

        public static void ShowBattleScreen(Player player, Enemy enemy)
        {
            Console.Clear();
            Console.WriteLine(@"
    ╔══════════════════════[ BATTLE ]══════════════════════╗");
            
            ShowHealthBar(player.Name, player.Health, player.MaxHealth, true);
            ShowHealthBar(enemy.Name, enemy.Health, enemy.MaxHealth, false);

            Console.WriteLine(@"    ╚══════════════════════════════════════════════════════╝");
        }

        public static void ShowBattleOptions(Player player)
        {
            Console.WriteLine(@"
    ╔══════════════════════[ ACTIONS ]══════════════════════╗
    ║                                                       ║
    ║  [1] Attack                                           ║
    ║  [2] " + player.SpecialMove + new string(' ', 43 - player.SpecialMove.Length) + "      ║" + @"
    ║                                                       ║
    ╚═══════════════════════════════════════════════════════╝");
        }

        public static void ShowBattleMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n    ► {message}");
            Console.ResetColor();
            Thread.Sleep(1000);
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public string SpecialMove { get; set; }

        public Player(string name, int health, int attack, string specialMove)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Attack = attack;
            SpecialMove = specialMove;
        }
    }

    class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }

        public Enemy(string name, int health, int attack)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Attack = attack;
        }
    }

    class Game
    {
        private static Player CreatePlayer(char choice, string name)
        {
            return choice switch
            {
                '1' => new Player(name, 100, 15, "Power Strike"),
                '2' => new Player(name, 80, 20, "Quick Shot"),
                '3' => new Player(name, 70, 25, "Fireball"),
                _ => null
            };
        }

        private static Enemy CreateEnemy(char choice)
        {
            return choice switch
            {
                '1' => new Enemy("Skeleton", 50, 10),
                '2' => new Enemy("Giant Crab", 80, 12),
                _ => null
            };
        }

        public static void Start()
        {
            Console.CursorVisible = false;
            bool playAgain = true;
            Random random = new Random();

            while (playAgain)
            {
                UI.ShowTitle();
                
                // Character Selection
                UI.ShowCharacterSelection();
                Player player = null;
                while (player == null)
                {
                    Console.Write("\n    Select your hero (1-3): ");
                    char choice = Console.ReadKey().KeyChar;
                    Console.Write("\n    Enter hero name: ");
                    string name = Console.ReadLine();
                    player = CreatePlayer(choice, name);
                }

                // Enemy Selection
                UI.ShowEnemySelection();
                Enemy enemy = null;
                while (enemy == null)
                {
                    Console.Write("\n    Select enemy (1-2): ");
                    enemy = CreateEnemy(Console.ReadKey().KeyChar);
                }

                // Battle Loop
                while (player.Health > 0 && enemy.Health > 0)
                {
                    UI.ShowBattleScreen(player, enemy);
                    UI.ShowBattleOptions(player);
                    
                    Console.Write("\n    Choose your action (1-2): ");
                    int damage = player.Attack;
                    if (Console.ReadKey().KeyChar == '2')
                    {
                        damage = (int)(damage * 1.5);
                        UI.ShowBattleMessage($"{player.Name} uses {player.SpecialMove} for {damage} damage!", ConsoleColor.Yellow);
                    }
                    else
                    {
                        UI.ShowBattleMessage($"{player.Name} attacks for {damage} damage!", ConsoleColor.Yellow);
                    }
                    
                    enemy.Health = Math.Max(0, enemy.Health - damage);

                    if (enemy.Health > 0)
                    {
                        player.Health = Math.Max(0, player.Health - enemy.Attack);
                        UI.ShowBattleMessage($"{enemy.Name} attacks for {enemy.Attack} damage!", ConsoleColor.Red);
                    }
                }

                // Battle Result
                Console.ForegroundColor = player.Health > 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(@"
    ╔════════════════════[ BATTLE RESULT ]═══════════════════╗
    ║                                                        ║");
                Console.WriteLine(player.Health > 0 ? 
                    "    ║                     VICTORY!                           ║" :
                    "    ║                    GAME OVER!                        ║");
                Console.WriteLine(@"    ║                                                        ║
    ╚════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.Write("\n    Play again? (y/n): ");
                playAgain = Console.ReadKey().KeyChar.ToString().ToLower() == "y";
            }

            Console.Clear();
            Console.WriteLine(@"
    ╔════════════════════[ THANK YOU! ]════════════════════╗
    ║                                                      ║
    ║           Thanks for playing RPG Battle!             ║
    ║                                                      ║
    ╚══════════════════════════════════════════════════════╝");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RPG Battle";
            Game.Start();
        }
    }
}