using System;
using System.Threading;

namespace RPGGame
{
    // Player class definition
    public class Player
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
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} takes {damage} damage! HP: {Health}");
        }

        public bool IsAlive() => Health > 0;

        public virtual void UseSkill(Enemy enemy)
        {
            if (ClassName == "Mage" && Mana >= 20)
            {
                int skillDamage = 5 * AttackPower;
                Mana -= 20;
                Console.WriteLine($"{Name} uses Lightning Bolt for {skillDamage} damage!");
                enemy.TakeDamage(skillDamage);
            }
            else Attack(enemy);
        }
    }

    // Enemy base class
    public class Enemy
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
            Console.WriteLine($"{Name} attacks for {AttackPower} damage!");
            player.TakeDamage(AttackPower);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} takes {damage} damage! HP: {Health}");
        }

        public bool IsAlive() => Health > 0;
    }

    // Enemy types
    public class Skeleton : Enemy
    {
        public Skeleton() : base("Skeleton", 150, 5) { }
    }

    public class GiantCrab : Enemy
    {
        public GiantCrab() : base("Giant Crab", 80, 12) { }
    }

    public class Dragon : Enemy
    {
        public Dragon() : base("Dragon", 500, 75) { }
    }

    public class LordOfDeath : Enemy
    {
        private Random random = new Random();

        public LordOfDeath() : base("Lord of Death", 1000, 50) { }

        public override void Attack(Player player)
        {
            if (random.Next(100) < 5)
            {
                Console.WriteLine($"{Name} unleashes a fatal attack!");
                player.TakeDamage(999999);
            }
            else
            {
                base.Attack(player);
            }
        }
    }

    // Battle system
    public class Encounter
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
            while (player.IsAlive() && enemy.IsAlive())
            {
                player.Attack(enemy);
                Thread.Sleep(1000);

                if (enemy.IsAlive())
                {
                    enemy.Attack(player);
                    Thread.Sleep(1000);
                }
            }
        }
    }

    // Treasure system
    public class TreasureBox
    {
        private int[] code = { 1, 2, 3, 4 };

        public void Open(Player player)
        {
            Console.WriteLine("Enter four numbers (1-9):");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Number {i + 1}: ");
                if (!int.TryParse(Console.ReadLine(), out int input) || input != code[i])
                {
                    Console.WriteLine("Incorrect code! Box remains locked.");
                    return;
                }
            }
            Console.WriteLine("Box opened! Gained EXP!");
            player.Exp += 20;
        }
    }
    
    [Rest of your Program class remains the same]
}