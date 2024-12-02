using System;
using System.Collections.Generic;
using System.Threading;

namespace RPGGame
{
    // Interface untuk entitas yang dapat diserang
    public interface IAttackable
    {
        string Name { get; }
        int Health { get; set; }
        bool IsAlive();
        void TakeDamage(int damage);
    }

    // Utilitas Visual untuk menambahkan warna dan banner
    static class GameUI
    {
        public static void Clear() => Console.Clear();

        public static void DisplayTitle()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔══════════════════════════════════════════════════════════╗
    ║  ███████╗██████╗ ██╗ ██████╗     ██████╗  ██████╗  ██████╗║
    ║  ██╔════╝██╔══██╗██║██╔════╝    ██╔══██╗██╔══██╗██╔════╝║
    ║  █████╗  ██████╔╝██║██║         ██████╔╝██████╔╝██║  ███║
    ║  ██╔══╝  ██╔═══╝ ██║██║         ██╔══██╗██╔═══╝ ██║   ██║
    ║  ███████╗██║     ██║╚██████╗    ██║  ██║██║     ╚██████╔║
    ║  ╚══════╝╚═╝     ╚═╝ ╚═════╝    ╚═╝  ╚═╝╚═╝      ╚═════╝║
    ║                                                          ║
    ║               Epic Fantasy Adventure                     ║
    ╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        public static void DisplayBattleScreen(Player player, Enemy enemy)
        {
            Console.WriteLine($@"
    ╔═══════════════════════[ BATTLE ]═══════════════════════╗
    ║ {GetClassIcon(player.ClassName)} {player.Name,-15} VS {enemy.Name,-15} {GetEnemyIcon(enemy.Name)} ║
    ╟───────────────────────────────────────────────────────────╢
    ║ HP: {GetHealthBar(player.Health, player.MaxHealth)} {player.Health,4}/{player.MaxHealth,-4} ║
    ║ MP: {GetManaBar(player.Mana, player.MaxMana)} {player.Mana,4}/{player.MaxMana,-4} ║
    ╟───────────────────────────────────────────────────────────╢
    ║ Enemy HP: {GetHealthBar(enemy.Health, enemy.MaxHealth)} {enemy.Health,4}/{enemy.MaxHealth,-4} ║
    ╚═══════════════════════════════════════════════════════════╝");
        }

        private static string GetClassIcon(string className) => className.ToLower() switch
        {
            "warrior" => "⚔️",
            "mage" => "🔮",
            "archer" => "🏹",
            _ => "👤"
        };

        private static string GetEnemyIcon(string enemyName) => enemyName.ToLower() switch
        {
            "skeleton" => "💀",
            "giant crab" => "🦀",
            "ogre" => "👹",
            "dragon" => "🐉",
            "lord of death" => "☠️",
            _ => "👾"
        };

        public static string GetHealthBar(int current, int max, int length = 20)
        {
            int filled = (int)((float)current / max * length);
            ConsoleColor color = current > max * 0.5 ? ConsoleColor.Green :
                               current > max * 0.2 ? ConsoleColor.Yellow :
                               ConsoleColor.Red;

            Console.ForegroundColor = color;
            string bar = $"[{new string('█', filled)}{new string('░', length - filled)}]";
            Console.ResetColor();
            return bar;
        }

        public static string GetManaBar(int current, int max, int length = 20)
        {
            int filled = (int)((float)current / max * length);
            Console.ForegroundColor = ConsoleColor.Blue;
            string bar = $"[{new string('■', filled)}{new string('□', length - filled)}]";
            Console.ResetColor();
            return bar;
        }

        public static void DisplayMainMenu()
        {
            Console.WriteLine(@"
    ╔═════════════[ MAIN MENU ]═════════════╗
    ║                                       ║
    ║  1. ⚔️  Battle Monster                ║
    ║  2. 🎁  Open Treasure                ║
    ║  3. 📊  View Stats                   ║
    ║  4. 🚪  Exit Game                    ║
    ║                                       ║
    ╚═══════════════════════════════════════╝");
        }

        public static void DisplayBattleMenu()
        {
            Console.WriteLine(@"
    ╔════════════[ BATTLE MENU ]════════════╗
    ║                                       ║
    ║  1. ⚔️  Attack                        ║
    ║  2. ✨  Use Skill                     ║
    ║  3. 🧪  Use Item                      ║
    ║  4. 🏃  Flee                         ║
    ║                                       ║
    ╚═══════════════════════════════════════╝");
        }

        public static void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"  ⚔️ {message}");
            Console.ResetColor();
            Thread.Sleep(500);
        }

        public static void DisplayStats(Player player)
        {
            Console.WriteLine($@"
    ╔═══════════════[ CHARACTER STATUS ]═══════════════╗
    ║ {GetClassIcon(player.ClassName)} {player.Name} - Level {player.Level} {player.ClassName,-12} ║
    ╟───────────────────────────────────────────────────╢
    ║ Health: {player.Health,3}/{player.MaxHealth,-3}  Mana: {player.Mana,3}/{player.MaxMana,-3}        ║
    ║ Attack: {player.AttackPower,-3}     EXP: {player.Exp,4}/{player.Level * 50,-4}    ║
    ║                                                   ║
    ║ Inventory:                                        ║
{GetInventoryDisplay(player.Inventory)}
    ╚═══════════════════════════════════════════════════╝");
        }

        private static string GetInventoryDisplay(List<Item> inventory)
        {
            if (inventory.Count == 0)
                return "    ║  (Empty)                                          ║";

            string result = "";
            foreach (var item in inventory)
            {
                string itemIcon = item is HealthPotion ? "❤️" : "💧";
                result += $"    ║  {itemIcon} {item.Name,-40} ║\n";
            }
            return result.TrimEnd('\n');
        }

        public static void DisplayVictory(string enemyName, int expGained)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($@"
    ╔═════════════[ VICTORY! ]═════════════╗
    ║                                      ║
    ║    🎉 Defeated {enemyName}!           ║
    ║    Gained {expGained} experience!    ║
    ║                                      ║
    ╚══════════════════════════════════════╝");
            Console.ResetColor();
            Thread.Sleep(1500);
        }

        public static void DisplayGameOver()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
    ╔════════════[ GAME OVER ]════════════╗
    ║                                     ║
    ║      You have been defeated...      ║
    ║      Better luck next time!         ║
    ║                                     ║
    ╚═════════════════════════════════════╝");
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        public static void ShowTreasureAnimation()
        {
            string[] frames = {
                "  🎁  ",
                " 📦   ",
                "✨🎁✨",
                "  📦  "
            };

            Console.WriteLine("\n    Opening treasure box...");
            for (int i = 0; i < 3; i++)
            {
                foreach (string frame in frames)
                {
                    Console.Write($"\r    {frame}");
                    Thread.Sleep(200);
                }
            }
            Console.WriteLine("\n");
        }
    }

    // Player Class
    public class Player : IAttackable
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int AttackPower { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int Exp { get; set; }
        public string ClassName { get; set; }
        public List<Item> Inventory { get; set; }

        public Player(string name, string className)
        {
            Name = name;
            ClassName = className;
            Level = 1;
            Exp = 0;
            Inventory = new List<Item>();

            // Initialize stats berdasarkan kelas
            switch (ClassName.ToLower())
            {
                case "mage":
                    MaxHealth = 80;
                    Health = MaxHealth;
                    AttackPower = 60;
                    MaxMana = 120;
                    Mana = MaxMana;
                    break;
                case "warrior":
                    MaxHealth = 150;
                    Health = MaxHealth;
                    AttackPower = 80;
                    MaxMana = 50;
                    Mana = MaxMana;
                    break;
                case "archer":
                    MaxHealth = 100;
                    Health = MaxHealth;
                    AttackPower = 70;
                    MaxMana = 80;
                    Mana = MaxMana;
                    break;
                default:
                    MaxHealth = 100;
                    Health = MaxHealth;
                    AttackPower = 50;
                    MaxMana = 100;
                    Mana = MaxMana;
                    break;
            }
        }

        public void Attack(IAttackable target)
        {
            int damage = (int)(1.5 * AttackPower);
            GameUI.PrintMessage($"{Name} attacks {target.Name}, dealing {damage} damage!", ConsoleColor.Yellow);
            target.TakeDamage(damage);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            GameUI.PrintMessage($"{Name} takes {damage} damage! Remaining HP: {Health}/{MaxHealth}", ConsoleColor.Red);
            if (Health <= 0)
            {
                Health = 0;
                GameUI.PrintMessage($"{Name} has been defeated...", ConsoleColor.DarkRed);
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public void UseSkill(IAttackable target)
        {
            switch (ClassName.ToLower())
            {
                case "mage":
                    MageSkill(target);
                    break;
                case "warrior":
                    WarriorSkill(target);
                    break;
                case "archer":
                    ArcherSkill(target);
                    break;
                default:
                    Attack(target);
                    break;
            }
        }

        private void MageSkill(IAttackable target)
        {
            if (Mana >= 20)
            {
                int skillDamage = 5 * AttackPower;
                Mana -= 20;
                GameUI.PrintMessage($"{Name} uses Fireball, dealing {skillDamage} damage!", ConsoleColor.Magenta);
                target.TakeDamage(skillDamage);
            }
            else
            {
                GameUI.PrintMessage($"{Name} doesn't have enough mana to use Fireball.", ConsoleColor.DarkGray);
            }
        }

        private void WarriorSkill(IAttackable target)
        {
            if (Mana >= 15)
            {
                int skillDamage = 2 * AttackPower;
                Mana -= 15;
                GameUI.PrintMessage($"{Name} uses Power Strike, dealing {skillDamage} damage!", ConsoleColor.DarkYellow);
                target.TakeDamage(skillDamage);
            }
            else
            {
                GameUI.PrintMessage($"{Name} doesn't have enough stamina to use Power Strike.", ConsoleColor.DarkGray);
            }
        }

        private void ArcherSkill(IAttackable target)
        {
            if (Mana >= 10)
            {
                int skillDamage = (int)(AttackPower * 1.75);
                Mana -= 10;
                GameUI.PrintMessage($"{Name} uses Arrow Storm, dealing {skillDamage} damage!", ConsoleColor.Green);
                target.TakeDamage(skillDamage);
            }
            else
            {
                GameUI.PrintMessage($"{Name} doesn't have enough stamina to use Arrow Storm.", ConsoleColor.DarkGray);
            }
        }

        public void UseItem()
        {
            if (Inventory.Count == 0)
            {
                GameUI.PrintMessage("Your inventory is empty!", ConsoleColor.DarkGray);
                return;
            }

            GameUI.PrintMessage("Choose an item to use:", ConsoleColor.Cyan);
            for (int i = 0; i < Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Inventory[i].Name} - {Inventory[i].Description}");
            }
            Console.Write("Your choice: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= Inventory.Count)
            {
                Inventory[choice - 1].Use(this);
                Inventory.RemoveAt(choice - 1);
            }
            else
            {
                GameUI.PrintMessage("Invalid choice.", ConsoleColor.DarkRed);
            }
        }

        public void GainExp(int amount)
        {
            Exp += amount;
            GameUI.PrintMessage($"{Name} gains {amount} EXP. Total EXP: {Exp}", ConsoleColor.Green);
            while (Exp >= ExpToNextLevel())
            {
                LevelUp();
            }
        }

        private int ExpToNextLevel()
        {
            return 50 * Level;
        }

        private void LevelUp()
        {
            Exp -= ExpToNextLevel();
            Level++;
            GameUI.PrintMessage($"{Name} has reached Level {Level}! Stats have increased.", ConsoleColor.Yellow);

            // Increase stats based on class
            switch (ClassName.ToLower())
            {
                case "mage":
                    MaxHealth += 20;
                    Health = MaxHealth;
                    AttackPower += 10;
                    MaxMana += 30;
                    Mana = MaxMana;
                    break;
                case "warrior":
                    MaxHealth += 30;
                    Health = MaxHealth;
                    AttackPower += 15;
                    MaxMana += 10;
                    Mana = MaxMana;
                    break;
                case "archer":
                    MaxHealth += 25;
                    Health = MaxHealth;
                    AttackPower += 12;
                    MaxMana += 20;
                    Mana = MaxMana;
                    break;
                default:
                    MaxHealth += 25;
                    Health = MaxHealth;
                    AttackPower += 10;
                    MaxMana += 20;
                    Mana = MaxMana;
                    break;
            }
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
            GameUI.PrintMessage($"{Name} obtained {item.Name}!", ConsoleColor.Cyan);
        }

        public void ShowStats()
        {
            GameUI.DisplayStats(this);
        }
    }

    // Base Class Item
    public abstract class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public abstract void Use(Player player);
    }

    public class HealthPotion : Item
    {
        public int HealAmount { get; set; }

        public HealthPotion()
        {
            HealAmount = 50;
            Name = "Health Potion";
            Description = $"Restores {HealAmount} HP.";
        }

        public override void Use(Player player)
        {
            player.Health += HealAmount;
            if (player.Health > player.MaxHealth)
                player.Health = player.MaxHealth;
            GameUI.PrintMessage($"{player.Name} uses {Name} and restores {HealAmount} HP! Current HP: {player.Health}/{player.MaxHealth}", ConsoleColor.Green);
        }
    }

    public class ManaPotion : Item
    {
        public int RestoreAmount { get; set; }

        public ManaPotion()
        {
            RestoreAmount = 30;
            Name = "Mana Potion";
            Description = $"Restores {RestoreAmount} Mana.";
        }

        public override void Use(Player player)
        {
            player.Mana += RestoreAmount;
            if (player.Mana > player.MaxMana)
                player.Mana = player.MaxMana;
            GameUI.PrintMessage($"{player.Name} uses {Name} and restores {RestoreAmount} Mana! Current Mana: {player.Mana}/{player.MaxMana}", ConsoleColor.Blue);
        }
    }

    // Base Class Enemy
    public abstract class Enemy : IAttackable
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int AttackPower { get; set; }

        protected Enemy(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            AttackPower = attackPower;
        }

        public virtual void Attack(IAttackable target)
        {
            GameUI.PrintMessage($"{Name} attacks {target.Name}, dealing {AttackPower} damage!", ConsoleColor.Red);
            target.TakeDamage(AttackPower);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            GameUI.PrintMessage($"{Name} takes {damage} damage! Remaining HP: {Health}/{MaxHealth}", ConsoleColor.DarkRed);
            if (Health <= 0)
            {
                Health = 0;
                GameUI.PrintMessage($"{Name} has been defeated!", ConsoleColor.DarkRed);
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public abstract void UseSkill(IAttackable target);
    }

    // Enemy Subclasses
    public class Ogre : Enemy
    {
        public Ogre() : base("Ogre", 200, 20) { }

        public override void UseSkill(IAttackable target)
        {
            int skillDamage = AttackPower + 10;
            GameUI.PrintMessage($"{Name} uses Smash, dealing {skillDamage} damage!", ConsoleColor.DarkMagenta);
            target.TakeDamage(skillDamage);
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton() : base("Skeleton", 120, 10) { }

        public override void UseSkill(IAttackable target)
        {
            int skillDamage = AttackPower + 5;
            GameUI.PrintMessage($"{Name} uses Bone Crush, dealing {skillDamage} damage!", ConsoleColor.DarkMagenta);
            target.TakeDamage(skillDamage);
        }
    }

    public class GiantCrab : Enemy
    {
        public GiantCrab() : base("Giant Crab", 100, 15) { }

        public override void UseSkill(IAttackable target)
        {
            int skillDamage = AttackPower + 8;
            GameUI.PrintMessage($"{Name} uses Pinch Attack, dealing {skillDamage} damage!", ConsoleColor.DarkMagenta);
            target.TakeDamage(skillDamage);
        }
    }

    public class Dragon : Enemy
    {
        public Dragon() : base("Dragon", 500, 50) { }

        public override void UseSkill(IAttackable target)
        {
            int skillDamage = AttackPower + 20;
            GameUI.PrintMessage($"{Name} uses Fire Breath, dealing {skillDamage} damage!", ConsoleColor.DarkMagenta);
            target.TakeDamage(skillDamage);
        }
    }

    public class LordOfDeath : Enemy
    {
        private Random random = new Random();

        public LordOfDeath() : base("Lord of Death", 1000, 75) { }

        public override void UseSkill(IAttackable target)
        {
            GameUI.PrintMessage($"{Name} uses Dark Resurrection!", ConsoleColor.DarkRed);
            if (random.Next(100) < 10) // 10% chance
            {
                GameUI.PrintMessage($"{Name} unleashes a deadly attack! {(target as Player).Name} is slain instantly!", ConsoleColor.DarkRed);
                target.TakeDamage(999999); // Instant kill
            }
            else
            {
                int skillDamage = AttackPower + (int)(0.5 * ((Player)target).Health);
                GameUI.PrintMessage($"{Name} attacks fiercely, dealing {skillDamage} damage!", ConsoleColor.DarkRed);
                target.TakeDamage(skillDamage);
            }
        }
    }

    // Treasure Box Class dengan Rarity dan Loot yang Lebih Dinamis
    public class TreasureBox
    {
        private readonly int[] combination = { 1, 2, 3, 4 };
        private readonly Random random = new Random();

        public void Open(Player player)
        {
            GameUI.PrintMessage("Attempt to open the treasure box with a 4-number combination (1-9):", ConsoleColor.Cyan);

            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Enter number {i + 1}: ");
                string inputUser = Console.ReadLine();

                if (int.TryParse(inputUser, out int userInput))
                {
                    if (userInput != combination[i])
                    {
                        GameUI.PrintMessage("Incorrect combination! The box remains closed.", ConsoleColor.DarkRed);
                        return;
                    }
                }
                else
                {
                    GameUI.PrintMessage("Invalid input! Please enter a number.", ConsoleColor.DarkRed);
                    return;
                }
            }

            GameUI.PrintMessage("The treasure box opens! You receive a random reward.", ConsoleColor.Green);
            GenerateReward(player);
        }

        private void GenerateReward(Player player)
        {
            int rewardType = random.Next(100);
            if (rewardType < 50)
            {
                player.GainExp(30);
            }
            else if (rewardType < 80)
            {
                player.AddItem(new HealthPotion());
            }
            else if (rewardType < 95)
            {
                player.AddItem(new ManaPotion());
            }
            else
            {
                // Rare item atau bonus EXP
                player.AddItem(new ManaPotion()); // Bisa diganti dengan item langka lainnya
                GameUI.PrintMessage("You have obtained a rare Mana Potion!", ConsoleColor.Magenta);
            }
        }
    }

    // Encounter Class dengan Peningkatan Sistem Pertempuran
    public class Encounter
    {
        private readonly Player player;
        private readonly Enemy enemy;
        private readonly Random random = new Random();

        public Encounter(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public void Start()
        {
            GameUI.PrintMessage($"A wild {enemy.Name} appears!", ConsoleColor.Yellow);
            Thread.Sleep(1000);

            while (player.IsAlive() && enemy.IsAlive())
            {
                GameUI.DisplayBattleScreen(player, enemy);
                GameUI.DisplayBattleMenu();

                Console.Write("\nYour choice: ");
                string choice = Console.ReadLine();
                HandlePlayerTurn(choice);

                if (enemy.IsAlive())
                {
                    Thread.Sleep(1000);
                    EnemyTurn();
                }

                Thread.Sleep(800);
            }

            HandleBattleEnd();
        }

        private void HandlePlayerTurn(string choice)
        {
            switch (choice)
            {
                case "1":
                    player.Attack(enemy);
                    break;
                case "2":
                    player.UseSkill(enemy);
                    break;
                case "3":
                    player.UseItem();
                    break;
                case "4":
                    if (AttemptFlee())
                    {
                        GameUI.PrintMessage("Successfully fled from battle!", ConsoleColor.Green);
                        enemy.Health = 0; // End battle
                    }
                    else
                    {
                        GameUI.PrintMessage("Failed to flee!", ConsoleColor.Red);
                    }
                    break;
                default:
                    GameUI.PrintMessage("Invalid choice! Turn skipped!", ConsoleColor.Red);
                    break;
            }
        }

        private void EnemyTurn()
        {
            if (random.Next(2) == 0)
                enemy.Attack(player);
            else
                enemy.UseSkill(player);
        }

        private bool AttemptFlee() => random.Next(100) < 40; // 40% chance to flee

        private void HandleBattleEnd()
        {
            if (player.IsAlive())
            {
                int expGained = enemy is LordOfDeath ? 500 : 100;
                GameUI.DisplayVictory(enemy.Name, expGained);
                player.GainExp(expGained);

                if (!(enemy is LordOfDeath) && random.Next(100) < 70)
                {
                    GameUI.ShowTreasureAnimation();
                    if (random.Next(2) == 0)
                    {
                        player.AddItem(new HealthPotion());
                        GameUI.PrintMessage("Found a Health Potion!", ConsoleColor.Green);
                    }
                    else
                    {
                        player.AddItem(new ManaPotion());
                        GameUI.PrintMessage("Found a Mana Potion!", ConsoleColor.Blue);
                    }
                }
            }
            else
            {
                GameUI.DisplayGameOver();
            }
        }
    }

    // Program Class
    public class Program
    {
        static void Main(string[] args)
        {
            GameUI.Clear();
            GameUI.DisplayTitle();

            Console.Write("\nEnter your character's name: ");
            string playerName = Console.ReadLine() ?? "Hero";

            Console.WriteLine("\nChoose your class:");
            Console.WriteLine(@"
    ╔═════════════[ CLASS SELECTION ]════════════╗
    ║                                            ║
    ║  1. 👤 Novice - Balanced Starter          ║
    ║  2. 🔮 Mage   - High Magic Power          ║
    ║  3. ⚔️  Warrior - High HP & Attack         ║
    ║  4. 🏹 Archer - Fast & Agile              ║
    ║                                            ║
    ╚════════════════════════════════════════════╝");

            Console.Write("Your choice: ");
            string classChoice = Console.ReadLine();
            string className = classChoice switch
            {
                "1" => "Novice",
                "2" => "Mage",
                "3" => "Warrior",
                "4" => "Archer",
                _ => "Novice"
            };

            Player player = new Player(playerName, className);
            GameUI.PrintMessage($"Welcome, {player.Name} the {player.ClassName}!", ConsoleColor.Cyan);

            bool gameRunning = true;
            while (gameRunning && player.IsAlive())
            {
                GameUI.DisplayMainMenu();
                Console.Write("\nYour choice: ");
                string action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        BattleChoice(player);
                        break;
                    case "2":
                        TreasureBox box = new TreasureBox();
                        box.Open(player);
                        break;
                    case "3":
                        player.ShowStats();
                        break;
                    case "4":
                        GameUI.PrintMessage("Thank you for playing! Goodbye!", ConsoleColor.Yellow);
                        gameRunning = false;
                        break;
                    default:
                        GameUI.PrintMessage("Invalid choice. Please try again.", ConsoleColor.Red);
                        break;
                }

                Thread.Sleep(800);
            }

            if (!player.IsAlive())
            {
                GameUI.DisplayGameOver();
            }
        }

        static void BattleChoice(Player player)
        {
            Console.WriteLine(@"
    ╔═════════════[ CHOOSE ENEMY ]════════════╗
    ║                                         ║
    ║  1. 💀 Skeleton    - Easy              ║
    ║  2. 🦀 Giant Crab  - Normal            ║
    ║  3. 👹 Ogre        - Hard              ║
    ║  4. 🐉 Dragon      - Very Hard         ║
    ║  5. ☠️  Lord of Death - Boss            ║
    ║                                         ║
    ╚═════════════════════════════════════════╝");

            Console.Write("Your choice: ");
            string enemyChoice = Console.ReadLine();

            Enemy enemy = enemyChoice switch
            {
                "1" => new Skeleton(),
                "2" => new GiantCrab(),
                "3" => new Ogre(),
                "4" => new Dragon(),
                "5" => new LordOfDeath(),
                _ => null
            };

            if (enemy != null)
            {
                var encounter = new Encounter(player, enemy);
                encounter.Start();
            }
            else
            {
                GameUI.PrintMessage("Invalid choice. Returning to main menu.", ConsoleColor.Red);
            }
        }
    }
}
