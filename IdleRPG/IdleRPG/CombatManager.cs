using IdleRPG.GameElements;
using IdleRPG.GameEvents;
using System.Reactive.Linq;

namespace IdleRPG
{
    /// <summary>
    /// This class represents and simulate fights, initiate the player and the attackers
    /// the fighit is designed to simulate combat between main player and set of attackers which generates based on the level
    /// it is also a singleton
    /// </summary>
    public class CombatManager
    {
        private static CombatManager _instance;
        
        private IEnumerable<Character> _enemies;
        private static CancellationTokenSource _cts;


        private const int _mainPlayerInitialSpellPower = 10;
        private const int _mainPlayerInitialMaxHealth = 100;
        private const int _mainPlayerInitialLevel = 1;
        private const int _mainPlayerInitialStrength = 30;
        private const int _mainPlayerInitialDefense = 15;
        private const int _mainPlayerInitialDexterity = 1;
        private const int _mainPlayerInitialIntelligence = 1;
        private const int _mainPlayerInitialAttackSpeed = 5;


        private const int _attackerInitialSpellPower = 0;
        private const int _attackerInitialMaxHealth = 40;
        private const int _attackerInitialLevel = 1;
        private const int _attackerInitialStrength = 25;
        private const int _attackerInitialDefense = 15;
        private const int _attackerInitialDexterity = 1;
        private const int _attackerInitialIntelligence = 1;
        private const int _attackerInitialAttackSpeed = 2;

        private const int _mainPlayerInventoryLimit = 10;



        public int Level => MainPlayer.Level;
        public Character MainPlayer { get; private set; }

        private CombatManager()
        {
            MainPlayer = InitiateMainPlayer("Sub-Zerro");
            EventBroker.Instance.OfType<FightLostEvent>().Subscribe(FightLostEventHanlder);
            EventBroker.Instance.OfType<StartFightingEvent>().Subscribe(StartFightingEventHandler);
        }
        public static CombatManager Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new CombatManager();
                return _instance;
            }
        }

        private Character InitiateMainPlayer(string name)
        {

            return new Character(
                name,                                      //Name
                _mainPlayerInitialLevel,                  //Level
                GameInventoryManager.Instance.GetFreeWeapon(),   //Weapon
                GameInventoryManager.Instance.GetFreeArmor(),    //Armor
                _mainPlayerInitialStrength,               //Strength
                _mainPlayerInitialDefense,                //Defence
                _mainPlayerInitialDexterity,              //Dexterity
                _mainPlayerInitialIntelligence,           //Intelligence
                _mainPlayerInitialAttackSpeed,            //AttackSpeed
                _mainPlayerInitialSpellPower,             //SpellPower
                _mainPlayerInitialMaxHealth,              //MaxHealth
                _mainPlayerInventoryLimit,                //InventoryLimit
                Class.Main);                              //Class   
        }

        /// <summary>
        /// Generates the enemy based on the level
        /// </summary>
        /// <returns></returns>
        IEnumerable<Character> GetEnemis()
        {

            int hardness = Level / 10;

            var enemies = new List<Character>();
            for (int i = 0; i < Level; i++)
            {
                enemies.Add(new Character(
                    $"Scorpion[{i}]",                                    //Name
                    _attackerInitialLevel,                       //Level
                    new Weapon("Samurai Sword", 5),                      //Weapon
                    new Armor("Fire Sheild", 0),                         //Armor
                    _attackerInitialStrength + hardness,         //Strength
                    _attackerInitialDefense + hardness / 2,      //Defence
                    _attackerInitialDexterity,                   //Dexterity
                    _attackerInitialIntelligence,                //Intelligence
                    _attackerInitialAttackSpeed,                 //AttackSpeed
                    _attackerInitialSpellPower,                  //SpellPower
                    _attackerInitialMaxHealth,                   //MaxHealth
                    0,                                           //InventoryLimit
                    Class.Enemy));
            }
            return enemies;
        }

        /// <summary>
        /// Simulate combat
        /// </summary>
        public void SimulateCombate()
        {
            _cts = new CancellationTokenSource();
            Console.WriteLine("------------------Combat Initating-----------------------");
            Console.WriteLine($"------------------Level: {Level}-------------------------");
            Console.WriteLine(this.MainPlayer.ToString());
            Console.WriteLine($"------------------Combat Started-------------------------");

            _enemies = GetEnemis();
            ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = System.Environment.ProcessorCount };
            po.CancellationToken = _cts.Token;


            try
            {
                Parallel.ForEach(_enemies, po, scorpion =>
                {
                    while (MainPlayer.Health > 0 && scorpion.Health > 0)
                    {
                        Random rand = new Random();
                        var randomNumber = rand.Next(10);
                        var subzeroAttack = randomNumber % 3 == 1 || randomNumber % 3 == 2 ? true : false;
                        if (subzeroAttack)
                        {
                            MainPlayer.Attack(scorpion);
                        }
                        else
                        {
                            scorpion.Attack(MainPlayer);
                        }

                    }
                });
            }
            catch (OperationCanceledException e)
            {
                //Console.WriteLine(e.Message);
                //_cts = null;
            }
            finally
            {
                //_cts.Dispose();
            }




            var alives = _enemies.Count(x => x.IsDead == false);
            if (alives == 0)
            {
                Console.WriteLine($"*************************{"Level Completed"}*************************");
                Random random = new Random();
                var loot = random.Next(Level, Level * Level + 1);
                //EventBroker.Instance.Publish(new LevelupEvent() { Loot = loot });
                MainPlayer.LevelUp(loot);

                if(MainPlayer.Inventory.Count == MainPlayer.Inventory.Limit )
                {
                    Console.WriteLine("It is Time to Sell Items");
                    GameInventoryManager.Instance.SellInventories(MainPlayer);
                    
                }
                else
                {
                    EventBroker.Instance.Publish(new StartFightingEvent() { Character = this.MainPlayer });
                    
                }
                

            }

        }       


        /// <summary>
        /// Restore player health for new level and combat
        /// </summary>

        private void RestoreHealth() => MainPlayer.Health = _mainPlayerInitialMaxHealth;

        /// <summary>
        /// Chracter die event handler, game manager will notice from chracter class throw game event broker
        /// </summary>
        /// <param name="lostFightEventData"></param>
        private void FightLostEventHanlder(FightLostEvent lostFightEventData)
        {
            _cts?.Cancel();
            Console.WriteLine("-------------------Level Result--------------");
            Console.WriteLine(lostFightEventData.Character.ToString());
            Console.WriteLine($"{lostFightEventData.Message}");
            RestoreHealth();
            SimulateCombate();

        }

        /// <summary>
        /// This event handler initate fight from other components
        /// </summary>
        /// <param name="startFightingEventData"></param>
       
        private void StartFightingEventHandler(StartFightingEvent startFightingEventData)
        {
            RestoreHealth();
            SimulateCombate();
        }
    }

}