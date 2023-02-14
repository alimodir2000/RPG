namespace IdleRPG.GameElements
{
    public class Inventory
    {


        private List<InventoryItem> _items;
        public int Limit { get; private set; }

        public int Count => _items.Count;

        public Inventory(int limit)
        {
            _items = new List<InventoryItem>();
            this.Limit = limit;
        }

        public bool AddItem(InventoryItem item)
        {
            if (_items.Count < Limit)
            {
                _items.Add(item);
                return true;
            }

            return false;
        }

        public bool RemoveItem(InventoryItem item)
        {
            return _items.Remove(item);
        }

        public void RestItems()
        {
            _items.Clear();
        }

        public List<InventoryItem> Items => _items;
       
    }

}