namespace IdleRPG.GameElements
{
    public class Inventory
    {
        private List<InventoryItem> items;
        private int limit;

        public Inventory(int limit)
        {
            items = new List<InventoryItem>();
            this.limit = limit;
        }

        public bool AddItem(InventoryItem item)
        {
            if (items.Count < limit)
            {
                items.Add(item);
                return true;
            }

            return false;
        }

        public bool RemoveItem(InventoryItem item)
        {
            return items.Remove(item);
        }

        public List<InventoryItem> GetItems()
        {
            return items;
        }
    }

}