using IdleRPG.GameElements;
using IdleRPG.GameEvents;
using System.Reactive.Linq;

namespace IdleRPG
{
    public class GameManager
    {
        private static GameManager _instance;
        
        private IEnumerable<Character> _enemies;
        private CancellationTokenSource _cts;


        private const int _mainPlayerInitialSpellPower = 10;
        private const int _mainPlayerInitialMaxHealth = 100;
        private const int _mainPlayerInitialLevel = 1;
        private const int _mainPlayerInitialStrength = 30;
        private const int _mainPlayerInitialDefense = 15;
        private const int _mainPlayerInitialDexterity = 1;
        private const int _mainPlayerInitialIntelligence = 1;
        private const int _mainPlayerInitialAttackSpeed = 5;


        private const int _attackerInitialSpellPower = 0;
        private const int _attackerInitialMaxHealth = 25;
        private const int _attackerInitialLevel = 1;
        private const int _attackerInitialStrength = 15;
        private const int _attackerInitialDefense = 5;
        private const int _attackerInitialDexterity = 1;
        private const int _attackerInitialIntelligence = 1;
        private const int _attackerInitialAttackSpeed = 2;

        private const int _mainPlayerInventoryLimit = 10;



        public int Level => MainPlayer.Level;
        public Character MainPlayer { get; private set; }

        private GameManager()
        {
            MainPlayer = InitiateMainPlayer("Sub-Zerro");
            EventBroker.Instance.OfType<FightLostEvent>().Subscribe(FightLostEventHanlder);
            EventBroker.Instance.OfType<StartFightingEvent>().Subscribe(StartFightingEventHandler);
        }
        public static GameManager Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }

        private Character InitiateMainPlayer(string name)
        {

            return new Character(
                name,                                      //Name
                _mainPlayerInitialLevel,                  //Level
                InventoryShop.Instance.GetFreeWeapon(),   //Weapon
                InventoryShop.Instance.GetFreeArmor(),    //Armor
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
            }
            finally
            {
                _cts.Dispose();
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
                    InventoryShop.Instance.SellInventories(MainPlayer);
                    
                }
                else
                {
                    EventBroker.Instance.Publish(new StartFightingEvent() { Character = this.MainPlayer });
                }
                

            }

        }       

        private void FightLostEventHanlder(FightLostEvent lostFightEventData)
        {
            Console.WriteLine("-------------------Level Result--------------");
            Console.WriteLine(lostFightEventData.Character.ToString());
            foreach (var enemy in _enemies)
            {
                string isDead = enemy.IsDead ? "Dead" : enemy.OverAllHealth.ToString();
                Console.WriteLine($"{enemy.Name} : {isDead}");
            }
            _cts?.Cancel();
            SimulateCombate();



        }

        private void LevelUpEventHandler(LevelupEvent levelupEventData)
        {
             
            //MainPlayer.LevelUp(levelupEventData.Loot);
            //if(MainPlayer.)
            
        }
        private void StartFightingEventHandler(StartFightingEvent startFightingEventData)
        {
            SimulateCombate();
        }
    }

}