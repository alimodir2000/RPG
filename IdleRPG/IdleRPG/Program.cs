using IdleRPG.GameEvents;
using System;
using System.Reflection.Emit;

namespace IdleRPG
{
    internal class Program
    {      

        
        static void Main(string[] args)
        {
            var instance = CombatManager.Instance;            
            EventBroker.Instance.Publish(new StartFightingEvent() { Character = null});
        }

    }

}