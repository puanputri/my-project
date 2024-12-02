using System;

namespace RPGGame
{
    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Mana { get; set; }
        public int Exp { get; set; }
        public string ClassName { get; set; }

        public Player(string name)
        {
            Name = name;
            Health = 100;
            AttackPower = 40;
            Mana = 0;
            Exp = 0;
            ClassName = "Novice";
            if (Health < 0) Health = 0;
        }

        public virtual void Attack(Enemy enemy)
        {
            int damage = (int)(1.5 * AttackPower);
            Console.WriteLine($"{Name} attacks {enemy.Name} dealing {damage} damage!");
            enemy.TakeDamage(damage);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"{Name} takes {damage} damage! HP remaining: {Health}");
            if (Health < 0) Health = 0;
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public virtual void UseSkill(Enemy enemy)
        {
            if (ClassName == "Mage")
            {
                if (Mana >= 20)
                {
                    int skillDamage = 5 * AttackPower;
                    Mana -= 20;
                    Console.WriteLine($"{Name} uses Lightning Bolt, dealing {skillDamage} damage!");
                    enemy.TakeDamage(skillDamage);
                }
                else
                {
                    Console.WriteLine($"{Name} doesn't have enough mana for Lightning Bolt!");
                }
            }
            else
            {
                int damage = (int)(1.5 * AttackPower);
                Console.WriteLine($"{Name} attacks {enemy.Name} dealing {damage} damage!");
                enemy.TakeDamage(damage);
            }
        }

        public void GainExp(int amount)
        {
            Exp += amount;
            Console.WriteLine($"{Name} gains {amount} EXP. Total EXP: {Exp}");

            if (Exp == 50)
            {
                EvolveToMage();
            }
            if (Exp == 100)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Console.WriteLine($"{Name} reaches a new level! Stats boosted!");
            Health = 210;
            AttackPower = 95;
            Mana = 120;
        }

        public void EvolveToMage()
        {
            Console.WriteLine($"{Name} reaches a new level! You are now a Mage!");
            ClassName = "Mage";
            Health = 100;
            AttackPower = 70;
            Mana = 90;
            Console.WriteLine($"{Name} now has Lightning Bolt skill!");
        }
    }

    class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        public Enemy(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        public virtual void Attack(Player player)
        {
            Console.WriteLine($"{Name} attacks {player.Name} dealing {AttackPower} damage!");
            player.TakeDamage(AttackPower);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} takes {damage} damage! HP remaining: {Health}");
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public virtual void UseSkill(Player player)
        {
            Console.WriteLine($"{Name} uses a basic attack!");
        }
    }

    class Skeleton : Enemy
    {
        public Skeleton() : base("Skeleton", 150, 5) { }

        public override void UseSkill(Player player)
        {
            int skillDamage = AttackPower + 5;
            Console.WriteLine($"{Name} uses Bone Smash, dealing {skillDamage} damage!");
            player.TakeDamage(skillDamage);
        }
    }

    class GiantCrab : Enemy
    {
        public GiantCrab() : base("Giant Crab", 80, 12) { }

        public override void UseSkill(Player player)
        {
            int skillDamage = AttackPower + 8;
            Console.WriteLine($"{Name} uses Pinch Attack, dealing {skillDamage} damage!");
            player.TakeDamage(skillDamage);
        }
    }

    class Dragon : Enemy
    {
        public Dragon() : base("Dragon", 500, 75) { }

        public override void UseSkill(Player player)
        {
            int skillDamage = AttackPower + 15;
            Console.WriteLine($"{Name} uses Fire Breath, dealing {skillDamage} damage!");
            player.TakeDamage(skillDamage);
        }
    }

    class LordOfDeath : Enemy
    {
        private Random random = new Random();

        public LordOfDeath() : base("Lord of Death", 1000, 50) { }

        public override void UseSkill(Player player)
        {
            Console.WriteLine($"{Name} uses Death Breath!");
            if (random.Next(100) < 5)
            {
                Console.WriteLine($"{Name} unleashes a fatal attack! {player.Name} dies instantly!");
                player.TakeDamage(999999);
            }
            else
            {
                int skillDamage = AttackPower + (int)(0.33 * player.Health);
                Console.WriteLine($"{Name} attacks dealing {skillDamage} damage!");
                player.TakeDamage(skillDamage);
            }
        }
    }

    class TreasureBox
    {
        private int[] combination = { 1, 2, 3, 4 };
        private string userInput = "";

        public void Open(Player player)
        {
            Console.WriteLine("Try to open the treasure box with 4 numbers (1-9):");

            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Enter number {i + 1}: ");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int input))
                {
                    if (input != combination[i])
                    {
                        Console.WriteLine("Wrong combination! Box cannot be opened.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Must be a number.");
                    return;
                }
            }
            Console.WriteLine("Box opened! You gain EXP as a reward.");
            player.GainExp(20);
        }
    }

    class Encounter
    {
        private Player player;
        private Enemy enemy;
        private Random random = new Random();

        public Encounter(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public void Start()
        {
            Console.WriteLine($"{player.Name} battles against {enemy.Name}!");

            while (player.IsAlive() && enemy.IsAlive())
            {
                if (random.Next(2) == 0)
                    player.Attack(enemy);
                else
                    player.UseSkill(enemy);

                System.Threading.Thread.Sleep(2000);

                if (enemy.IsAlive())
                {
                    if (random.Next(2) == 0)
                        enemy.Attack(player);
                    else
                        enemy.UseSkill(player);
                    System.Threading.Thread.Sleep(2000);
                }
            }

            if (player.IsAlive())
            {
                Console.WriteLine($"{player.Name} has defeated {enemy.Name}!");
                player.GainExp(50);
            }
            else
            {
                Console.WriteLine($"{enemy.Name} has defeated {player.Name}...");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("Hero");

            Console.WriteLine("Game Started!");
            Console.WriteLine($"Welcome, {player.Name} to the RPG Adventure!");

            while (player.IsAlive())
            {
                Console.WriteLine("\nChoose your action:");
                Console.WriteLine("1. Fight Skeleton");
                Console.WriteLine("2. Fight Giant Crab");
                Console.WriteLine("3. Exit");
                Console.Write("Your choice: ");
                string firstChoice = Console.ReadLine();

                // [Rest of the game logic with English text...]
                // Continue with the same logic structure but with English text
            }
        }
    }
}