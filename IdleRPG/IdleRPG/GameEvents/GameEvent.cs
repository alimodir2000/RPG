using IdleRPG.GameElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleRPG.GameEvents
{
    public class GameEvent
    {
        public Character Character { get; set; }
    }

    public class AttackEvent : GameEvent
    {
        public Character Opponent { get; set; }
    }

    public class LevelupEvent : GameEvent
    {
        public int Level { get; set; }
    }

    public class BuyInventoryEvent : GameEvent
    {
        public InventoryItem Item { get; set; }
        public int Count { get; set; }
        public int SpentGold { get; set; }
    }

    public class SellInventoryEvent : GameEvent
    {
        public InventoryItem Item { get; set; }
        public int Count { get; set; }
        public int RecievedGold { get; set; }
    }


}
