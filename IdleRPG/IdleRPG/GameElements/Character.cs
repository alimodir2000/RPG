namespace IdleRPG.GameElements
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public Stats Stats { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public int Health { get; set; }

        public Inventory Inventory { get; set; }

        public Character(string name, int level, Stats stats, Weapon weapon, Armor armor)
        {
            Name = name;
            Level = level;
            Stats = stats;
            Weapon = weapon;
            Armor = armor;
            Health = stats.MaxHealth;
        }

        public void Attack(Character enemy)
        {
            int damage = Weapon.Damage + Stats.Strength;
            //MessageBroker.Broadcast(new AttackEvent(this, enemy, damage));
        }

        public void TakeDamage(int damage)
        {
            int finalDamage = damage - Stats.Defense;
            Health -= finalDamage;
            //Broadcast(new DamageTakenEvent(this, finalDamage));
        }
    }


}