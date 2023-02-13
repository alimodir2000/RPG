using IdleRPG.GameElements;
using IdleRPG.GameEvents;
using System;
using System.Reactive.Linq;

namespace IdleRPG
{
    internal class Program
    {
        static int level = 40;
        static IEnumerable<Character> enemies;
        static CancellationTokenSource cts;

        static Program()
        {
            EventBroker.Instance.OfType<FightLost>().Subscribe(lostCharacter =>
            {
                Console.WriteLine("-------------------Level Result--------------");
                Console.WriteLine(lostCharacter.Character.ToString());
                foreach (var enemy in enemies)
                {
                    string isDead = enemy.IsDead ? "Dead" : enemy.OverAllHealth.ToString();
                    Console.WriteLine($"{enemy.Name} : {isDead}");
                }
                cts?.Cancel();

            });

            EventBroker.Instance.OfType<DieEvent>().Where(x => x.Character.Class == Class.Enemy).Subscribe(deadPlayer =>
            {



            });
        }
        static void Main(string[] args)
        {
            cts = new CancellationTokenSource();

            var subZerro = new Character(
                "Sub-Zerro",                                //Name
                level,                                      //Level
                new Weapon("Frezing Axe", 15),              //Weapon
                new Armor("Ice Sheild", 80),                //Armor
                30,                                         //Strength
                5,                                          //Defence
                1,                                          //Dexterity
                1,                                          //Intelligence
                5,                                          //AttackSpeed
                10,                                         //SpellPower
                100,                                        //MaxHealth
                Class.Main);                                //Class       


            enemies = GetEnemis(level);
            ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = System.Environment.ProcessorCount  };
            po.CancellationToken = cts.Token;


            try
            {
                Parallel.ForEach(enemies, po, scorpion =>
                {
                    while (subZerro.Health > 0 && scorpion.Health > 0)
                    {
                        Random rand = new Random();
                        var randomNumber = rand.Next(10);
                        var subzeroAttack = randomNumber % 3 == 1 || randomNumber % 3 == 2 ? true : false;
                        if (subzeroAttack)
                        {
                            subZerro.Attack(scorpion);

                        }
                        else
                        {
                            scorpion.Attack(subZerro);
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
                cts.Dispose();
            }

            var alives = enemies.Count(x => x.IsDead == false);
            if (alives == 0)
                Console.WriteLine($"*************************{"Level Completed"}*************************");













            Console.WriteLine("Hello, World!");
        }


        

    }



    public class GameManager
    {
        private static GameManager _instance;

        public int Level { get; private set; }

        private GameManager()
        {
            Level = 1;
        }


        IEnumerable<Character> GetEnemis()
        {

            int hardness = Level / 5;

            var enemies = new List<Character>();
            for (int i = 0; i < Level; i++)
            {
                enemies.Add(new Character(
                    $"Scorpion[{i}]",                           //Name
                    1,                                          //Level
                    new Weapon("Samurai Sword", 5),            //Weapon
                    new Armor("Fire Sheild", 0),               //Armor
                    5 + hardness,                               //Strength
                    5,                                          //Defence
                    1,                                          //Dexterity
                    1,                                          //Intelligence
                    5,                                          //AttackSpeed
                    10,                                         //SpellPower
                    50,                                         //MaxHealth
                    Class.Enemy));
            }
            return enemies;
        }

        public void SimulateCombate()
        {
            Console.WriteLine("------------------Combate Initating-----------------------");
            Console.WriteLine($"------------------Level: {Level}--------------------------");
        }


        public GameManager Instance
        {
            get
            {
               
                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }
    }

}