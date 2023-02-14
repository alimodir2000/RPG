namespace IdleRPG.GameElements
{
    public class Armor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Defense { get; set; }
        public int Cost { get; set; }

        public Armor(string name, int defense, int cost = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            Defense = defense;
            Cost = cost;
        }
    }

}