namespace IdleRPG.GameElements
{
    public class Weapon
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Cost { get; set; }       

        public Weapon(string name, int damage, int cost = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            Damage = damage;
            Cost = cost;
        }
    }

}