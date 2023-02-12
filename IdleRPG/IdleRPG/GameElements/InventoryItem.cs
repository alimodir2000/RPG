namespace IdleRPG.GameElements
{
    public class InventoryItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        public InventoryItem(string name, int value, string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }
    }

}