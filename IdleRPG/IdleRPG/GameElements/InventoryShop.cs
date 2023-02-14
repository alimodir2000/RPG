using IdleRPG.GameEvents;

namespace IdleRPG.GameElements
{


    public class InventoryShop
    {

        private static InventoryShop _instance;
        private Random _random;


        private List<InventoryItem> _rewardItems;
        private List<Weapon> _weapons;
        private List<Armor> _armors;

        public static InventoryShop Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InventoryShop();
                return _instance;
            }
        }


        public InventoryItem Rewarde(int Level)
        {

            var ind = _random.Next(Level + 1) % _rewardItems.Count;
            return _rewardItems[ind];
        }

        public void SellInventories(Character player)
        {
            Console.WriteLine($"Inventory Shop: Welcome {player.Name}! How can I help you?");
            Console.WriteLine($"{player.Name}: I wanna sell my rewards!");
            foreach (var item in player.Inventory.Items)
            {
                player.Gold += item.Value;
                Console.WriteLine($"{player.Name} sold the {item.Name} for {item.Value} Golds!, Total Gold: {player.TotalGold}");
            }


            player.ResetInventory();

            if(_random.Next(100)%2 == 0)
            {
                BuyWeapon(player);
                BuyArmor(player);
            }
            else
            {
                BuyArmor(player);
                BuyWeapon(player);                
            }




            Console.WriteLine($"Inventory Shop: Have a good time {player.Name}!");
            EventBroker.Instance.Publish(new StartFightingEvent() { Character = player });

        }

        public void BuyWeapon(Character player)
        {
            var avaliableWeapons = _weapons.Where(x => x.Id != player.Weapon.Id && x.Cost != 0).ToList();
            var newWeapon = avaliableWeapons.OrderBy(x => x.Cost).FirstOrDefault(x => x.Cost <= player.TotalGold);
            if (newWeapon != null)
            {
                Console.WriteLine($"Inventory Shop: Anything else?");
                Console.WriteLine($"{player.Name}: I wanna buy {newWeapon.Name}!");
                Console.WriteLine($"Inventory Shop: it costs you {newWeapon.Cost}?");
                Console.WriteLine($"{player.Name}: here!");
                Console.WriteLine($"{player.Name} weapon upgraded to {newWeapon.Name}");
                player.Gold -= newWeapon.Cost;
                if (player.Gold < 0)
                    player.Gold = 0;
                Console.WriteLine($"{player.Name} Gold is : {player.Gold}");

            }
        }

        public void BuyArmor(Character player)
        {
            var avaliableArmors = _armors.Where(x => x.Id != player.Armor.Id && x.Cost != 0).ToList();
            var newArmors = avaliableArmors.OrderBy(x => x.Cost).FirstOrDefault(x => x.Cost <= player.TotalGold);
            if (newArmors != null)
            {
                Console.WriteLine($"Inventory Shop: Anything else?");
                Console.WriteLine($"{player.Name}: I wanna buy {newArmors.Name}!");
                Console.WriteLine($"Inventory Shop: it costs you {newArmors.Cost}?");
                Console.WriteLine($"{player.Name}: here!");
                Console.WriteLine($"{player.Name} armor upgraded to {newArmors.Name}");
                player.Gold -= newArmors.Cost;
                if (player.Gold < 0)
                    player.Gold = 0;
                Console.WriteLine($"{player.Name} Gold is : {player.Gold}");

            }
        }


        private InventoryShop()
        {
            _random = new Random();
            _rewardItems = new List<InventoryItem>()
            {
                new InventoryItem("Small Gold Axe",200,"Small Golden Axe"),
                new InventoryItem("Big Gold Axe",400,"Big Golden Axe"),
                new InventoryItem("Silver Soward",100,"Silver Soward"),
                new InventoryItem("Jade",400,"Jade Stone"),
                new InventoryItem("Idle RPG Badge",800,"Idle RPG Badge"),
                new InventoryItem("Diamond",1000,"Diamond Stone"),
                new InventoryItem("Golden Belt",300,"Golden Belt"),
            };

            _weapons = new List<Weapon>()
            {
                new Weapon("Frezing Axe", 15),
                new Weapon("Enhanced Axe",35,10000),
                new Weapon("Frozen Kunai",50,20000),
                new Weapon("Laser Sword",70,30000),
                new Weapon("Ultimate Rope knives",80,40000),
            };

            _armors = new List<Armor>()
            {
                new Armor("Ice Sheild", 30),
                new Armor("Enhanced Icy Sheild",50,10000),
                new Armor("Frozen Cloud",60,20000),
                new Armor("Lasic Suite",70,30000),
                new Armor("Ultimate Dimonds Shield",80,40000),
            };
        }

        public Weapon GetFreeWeapon() => _weapons.FirstOrDefault(x => x.Cost == 0);
        public Armor GetFreeArmor() => _armors.FirstOrDefault(x => x.Cost == 0);



        public void AddItemForSale(InventoryItem item)
        {
            _rewardItems.Add(item);
        }
    }

}