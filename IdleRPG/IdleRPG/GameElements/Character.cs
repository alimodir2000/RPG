using IdleRPG.GameEvents;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;

namespace IdleRPG.GameElements
{

    public enum Class
    {
        Main,
        Enemy
    }
    public class Character
    {
        public Guid Id { get; set; }
        public Class Class { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int AttackSpeed { get; set; }
        public int SpellPower { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public int Health { get; set; }
        public Inventory Inventory { get; set; }
        public bool IsDead { get; set; } = false;
        public int Gold { get; set; }
        public int FightingPoint { get; set; }
        public int TotalGold => Gold + FightingPoint / 5;


        public int OverAllHealth => Health + Armor.Defense;

        public void SubscribeEvents()
        {
            EventBroker.Instance.OfType<AttackEvent>().
             Where(x => Id == x.Opponent.Id).Subscribe(attack =>
             {
                 Random random = new Random();
                 bool isDefensing = random.Next(1, 10) % 3 == 0 ? true : false;

                 attack.Damage -= Defense;
                 if (attack.Damage <= 0)
                 {
                     Console.WriteLine($"{Name} completely defended attacker {attack.Character.Name}");
                     return;
                 }
                 else
                 {
                     Console.WriteLine($"{Name} defended attacker {attack.Character.Name} by {Defense} points");
                 }

                 if (Armor.Defense > 0)
                 {
                     var armorHealth = Armor.Defense - attack.Damage;
                     if (armorHealth < 0)
                     {
                         Armor.Defense = 0;
                         Console.WriteLine($"{Name}'s armor destroyed!");
                     }
                     else
                     {
                         Armor.Defense = armorHealth;
                     }
                 }
                 else
                 {
                     Health -= attack.Damage;
                     if (Health <= 0)
                     {
                         EventBroker.Instance.Publish(new DieEvent() { Character = this });
                         //Console.WriteLine($"{Name} Died!");
                         return;
                     }
                 }

                 Console.WriteLine($"{Name} damaged by {attack.Damage} points from {attack.Character.Name}, Health: {OverAllHealth}");

             });


            EventBroker.Instance.OfType<DieEvent>().Where(x => Id == x.Character.Id).Subscribe(deadPlayer =>
               {
                   IsDead = true;
                   if(Class == Class.Main)
                   {
                       
                       Console.WriteLine($"GameOver!");
                       EventBroker.Instance.Publish(new FightLost() { Character = this });
                   }
                   else
                   {
                       Console.WriteLine($"{deadPlayer.Character.Name} is Dead!");
                   }

               });
        }

        public Character(string name, int level, Weapon weapon, Armor armor, int strength, int defense, int dexterity, int intelligence, int attackSpeed, int spellPower, int health, Class @class)
        {
            Name = name;
            Level = level;
            Weapon = weapon;
            Armor = armor;
            Strength = strength;
            Defense = defense;
            Dexterity = dexterity;
            Intelligence = intelligence;
            AttackSpeed = attackSpeed;
            SpellPower = spellPower;
            Health = health;
            Id = Guid.NewGuid();
            Class = @class;

            SubscribeEvents();
            
        }

        public void Attack(Character enemy)
        {
            int damage = Weapon.Damage + Strength;
            FightingPoint++;
            Console.WriteLine($"{Name} is attaking {enemy.Name} by {Weapon.Name}, Strength:{damage}");
            EventBroker.Instance.Publish(new AttackEvent() { Character = this, Damage = damage, Opponent = enemy });
        }

        public void TakeDamage(int damage)
        {
            int finalDamage = damage - Defense;
            Health -= finalDamage;
            //Broadcast(new DamageTakenEvent(this, finalDamage));
        }

        public override string ToString()
        {
            var str = string.Empty;
            str += Name + Environment.NewLine;
            str += $"Class:{Class} {Environment.NewLine}";
            str += $"Health:{Health} {Environment.NewLine}";
            str += $"Level:{Level} {Environment.NewLine}";
            str += $"Strength:{Strength} {Environment.NewLine}";
            str += $"Dexterity:{Dexterity} {Environment.NewLine}";
            str += $"Defense:{Defense} {Environment.NewLine}";
            str += $"Spell Power:{SpellPower} {Environment.NewLine}";
            str += $"Attack Speed:{AttackSpeed} {Environment.NewLine}";
            str += $"Fighting Points:{FightingPoint} {Environment.NewLine}";
            str += $"Gold:{TotalGold} {Environment.NewLine}";
            str += $"Weapon:{Weapon.Name} {Environment.NewLine}";
            str += $"Armor:{Armor.Name} {Environment.NewLine}";
            return str;
        }
    }


}