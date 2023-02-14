using IdleRPG.GameElements;

namespace IdleRPG.GameEvents
{
    public class AttackEvent : GameEvent
    {
        public Character Opponent { get; set; }
        public int Damage { get; set; }
    }


}
