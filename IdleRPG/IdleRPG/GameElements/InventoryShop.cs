namespace IdleRPG.GameElements
{

    public class BuyInventoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SellInventoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int GoldReceived { get; set; }
    }


    public class BuyInventoryRequest
    {
        public Inventory Inventory { get; set; }
        public Character Character { get; set; }
    }

    public class SellInventoryRequest
    {
        public Inventory Inventory { get; set; }
        public Character Character { get; set; }
    }
    public class InventoryShop
    {
        private MessageBroker broker;
        private Dictionary<string, InventoryItem> itemsForSale;

        public InventoryShop(MessageBroker broker)
        {
            this.broker = broker;
            itemsForSale = new Dictionary<string, InventoryItem>();

            //this.broker.Subscribe<BuyInventoryRequest>(OnBuyInventoryRequest);
            //this.broker.Subscribe<SellInventoryRequest>(OnSellInventoryRequest);
        }

        //public void OnBuyInventoryRequest(BuyInventoryRequest request)
        //{
        //    Character character = request.Character;
        //    string itemName = request.ItemName;

        //    if (itemsForSale.ContainsKey(itemName))
        //    {
        //        InventoryItem item = itemsForSale[itemName];

        //        if (character.Gold >= item.Price)
        //        {
        //            character.Gold -= item.Price;
        //            character.Inventory.AddItem(item);

        //            broker.Publish(new BuyInventoryResponse(character, item, true));
        //        }
        //        else
        //        {
        //            broker.Publish(new BuyInventoryResponse(character, item, false));
        //        }
        //    }
        //    else
        //    {
        //        broker.Publish(new BuyInventoryResponse(character, null, false));
        //    }
        //}

        //public void OnSellInventoryRequest(SellInventoryRequest request)
        //{
        //    Character character = request.Character;
        //    InventoryItem item = request.Item;

        //    if (character.Inventory.RemoveItem(item))
        //    {
        //        character.Gold += item.Price;

        //        broker.Publish(new SellInventoryResponse(character, item, true));
        //    }
        //    else
        //    {
        //        broker.Publish(new SellInventoryResponse(character, item, false));
        //    }
        //}

        public void AddItemForSale(InventoryItem item)
        {
            itemsForSale[item.Name] = item;
        }
    }

}