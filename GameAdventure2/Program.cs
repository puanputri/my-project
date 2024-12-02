using System;
using System.Threading;

namespace RPGGame
{
    // User Interface Helper
    static class UI
    {
        public static void DisplayTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔══════════════════════════════════════════════════════════╗
    ║  ███████╗██████╗ ██╗ ██████╗    ██████╗ ██████╗  ██████╗║
    ║  ██╔════╝██╔══██╗██║██╔════╝    ██╔══██╗██╔══██╗██╔════╝║
    ║  █████╗  ██████╔╝██║██║         ██████╔╝██████╔╝██║  ███║
    ║  ██╔══╝  ██╔═══╝ ██║██║         ██╔══██╗██╔═══╝ ██║   ██║
    ║  ███████╗██║     ██║╚██████╗    ██║  ██║██║     ╚██████╔║
    ║  ╚══════╝╚═╝     ╚═╝ ╚═════╝    ╚═╝  ╚═╝╚═╝      ╚═════╝║
    ╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public static void DisplayBattleStatus(string playerName, int playerHealth, string enemyName, int enemyHealth)
        {
            Console.WriteLine($@"
    ╔═════════════════[ BATTLE STATUS ]════════════════╗
    ║  {playerName,-15} HP: {GetHealthBar(playerHealth, 100)} {playerHealth,3}/100 ║
    ║  {enemyName,-15} HP: {GetHealthBar(enemyHealth, 100)} {enemyHealth,3}/100 ║
    ╚════════════════════════════════════════════════════╝");
        }

        private static string GetHealthBar(int current, int max, int length = 20)
        {
            int filled = (int)((float)current / max * length);
            return $"[{new string('█', filled)}{new string('░', length - filled)}]";
        }

        public static void PrintBattleMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"    ⚔️  {message}");
            Console.ResetColor();
            Thread.Sleep(1000);
        }
    }

    class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        protected Character(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        public bool IsAlive() => Health > 0;
    }

    class Player : Character
    {
        public string ClassName { get; private set; }

        public Player(string name, int health, int attackPower) 
            : base(name, health, attackPower)
        {
            ClassName = GetType().Name;
        }

        public virtual void Attack(Enemy enemy)
        {
            UI.PrintBattleMessage($"{Name} attacks with {AttackPower} damage!", ConsoleColor.Yellow);
            enemy.TakeDamage(AttackPower);
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
            UI.PrintBattleMessage($"{Name} takes {damage} damage! HP: {Health}", ConsoleColor.Red);
        }

        public virtual void UseSkill(Enemy enemy)
        {
            Attack(enemy);
        }
    }

    class Swordsman : Player
    {
        public Swordsman(string name) : base(name, 100, 15) { }

        public override void UseSkill(Enemy enemy)
        {
            int skillDamage = AttackPower + 10;
            UI.PrintBattleMessage($"{Name} unleashes Blade Slash for {skillDamage} damage!", ConsoleColor.Yellow);
            enemy.TakeDamage(skillDamage);
        }
    }

    class Archer : Player
    {
        public Archer(string name) : base(name, 80, 20) { }

        public override void UseSkill(Enemy enemy)
        {
            int skillDamage = AttackPower + 5;
            UI.PrintBattleMessage($"{Name} launches Arrow Barrage for {skillDamage} damage!", ConsoleColor.Green);
            enemy.TakeDamage(skillDamage);
        }
    }

    class Mage : Player
    {
        public Mage(string name) : base(name, 70, 25) { }

        public override void UseSkill(Enemy enemy)
        {
            int skillDamage = AttackPower * 2;
            UI.PrintBattleMessage($"{Name} casts Fireball for {skillDamage} damage!", ConsoleColor.Magenta);
            enemy.TakeDamage(skillDamage);
        }
    }

    class Enemy : Character
    {
        protected Enemy(string name, int health, int attackPower) 
            : base(name, health, attackPower) { }

        public virtual void Attack(Player player)
        {
            UI.PrintBattleMessage($"{Name} attacks for {AttackPower} damage!", ConsoleColor.Red);
            player.TakeDamage(AttackPower);
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
            UI.PrintBattleMessage($"{Name} takes {damage} damage! HP: {Health}", ConsoleColor.Green);
        }

        public virtual void UseSkill(Player player)
        {
            Attack(player);
        }
    }

    class Skeleton : Enemy
    {
        public Skeleton() : base("Skeleton", 50, 10) { }

        public override void UseSkill(Player player)
        {
            int skillDamage = AttackPower + 5;
            UI.PrintBattleMessage($"{Name} uses Bone Crush for {skillDamage} damage!", ConsoleColor.Red);
            player.TakeDamage(skillDamage);
        }
    }

    class GiantCrab : Enemy
    {
        public GiantCrab() : base("Giant Crab", 80, 12) { }

        public override void UseSkill(Player player)
        {
            int skillDamage = AttackPower + 8;
            UI.PrintBattleMessage($"{Name} uses Pincer Grip for {skillDamage} damage!", ConsoleColor.Red);
            player.TakeDamage(skillDamage);
        }
    }

    class Battle
    {
        private readonly Player player;
        private readonly Enemy enemy;
        private readonly Random random = new Random();

        public Battle(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public void Start()
        {
            Console.WriteLine($@"
    ╔════════════════[ BATTLE START ]═══════════════════╗
    ║  {player.Name} ({player.ClassName}) VS {enemy.Name}
    ╚═══════════════════════════════════════════════════╝");

            while (player.IsAlive() && enemy.IsAlive())
            {
                UI.DisplayBattleStatus(player.Name, player.Health, enemy.Name, enemy.Health);

                if (random.Next(2) == 0)
                    player.Attack(enemy);
                else
                    player.UseSkill(enemy);

                if (enemy.IsAlive())
                {
                    Thread.Sleep(1000);
                    if (random.Next(2) == 0)
                        enemy.Attack(player);
                    else
                        enemy.UseSkill(player);
                }
            }

            Console.WriteLine(player.IsAlive() 
                ? $"\n    ✨ {player.Name} has emerged victorious! ✨"
                : $"\n    💀 {player.Name} has been defeated... 💀");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Epic RPG Battle";
            UI.DisplayTitle();

            Console.Write("\n    Enter your hero's name: ");
            string playerName = Console.ReadLine();

            Console.WriteLine(@"
    Choose your class:
    1. Swordsman - Balanced warrior with high defense
    2. Archer    - Agile fighter with consistent damage
    3. Mage      - Glass cannon with powerful spells");

            Player player = Console.ReadKey().KeyChar switch
            {
                '1' => new Swordsman(playerName),
                '2' => new Archer(playerName),
                '3' => new Mage(playerName),
                _ => new Swordsman(playerName)
            };

            Enemy enemy = new GiantCrab();
            Battle battle = new Battle(player, enemy);
            battle.Start();

            Console.WriteLine("\n    Press any key to exit...");
            Console.ReadKey();
        }
    }
}