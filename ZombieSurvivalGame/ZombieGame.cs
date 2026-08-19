namespace ZombieSurvivalGame;

public class ZombieGame
{
    public static readonly Random Rng = new Random();
    private static int _gamesStarted;
    public static int GamesStarted => _gamesStarted;

    public static void PrintBanner()
    {
        Console.WriteLine("======================================");
        Console.WriteLine("      ZOMBIE SURVIVAL - THE OUTBREAK   ");
        Console.WriteLine("======================================");
    }

    private Player _player;
    private readonly Shelter _shelter;
    private readonly int _totalWaves;
    private int _currentWave;

    public ZombieGame() : this(3)
    {
    }

    public ZombieGame(int totalWaves)
    {
        _totalWaves = totalWaves < 1 ? 1 : totalWaves;
        _shelter = new Shelter("Old Cabin", 8);
        _currentWave = 0;
    }

    public void Run()
    {
        _gamesStarted++;
        PrintBanner();
        SelectPlayer();
        StartingLoadout();

        for (_currentWave = 1; _currentWave <= _totalWaves; _currentWave++)
        {
            Console.WriteLine();
            Console.WriteLine($"--- Wave {_currentWave} of {_totalWaves} ---");
            List<Zombie> wave = SpawnWave(_currentWave);
            bool survived = RunWave(wave);
            if (!survived)
            {
                PrintDefeat();
                return;
            }
            _player++;
            _shelter.Fortify();
            AwardLoot();
        }

        Console.WriteLine();
        Console.WriteLine("--- FINAL WAVE: the source zombie appears! ---");
        Player checkpoint = new Player(_player); 
        Console.WriteLine($"(Checkpoint saved: {checkpoint.Name}, Lv.{checkpoint.Level}, {checkpoint.Health} HP)");

        List<Zombie> finalWave = new List<Zombie> { CreateFinalBoss() };
        bool won = RunWave(finalWave);

        if (won)
            PrintVictory();
        else
            PrintDefeat();
    }

    private void SelectPlayer()
    {
        Console.WriteLine();
        Console.Write("Name your survivor: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) name = "Survivor";

        Console.WriteLine("Choose your survivor type:");
        Console.WriteLine("  1) Ranger  - HP 120, Energy 80  (balanced)");
        Console.WriteLine("  2) Medic   - HP 90,  Energy 110 (starts with extra medicine)");
        Console.WriteLine("  3) Brawler - HP 140, Energy 70  (starts with a melee weapon)");
        Console.Write("> ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "2":
                _player = new Player(name, 90, 110);
                _player.CollectItem(new MedicineItem("Med-kit", "A sturdy first aid kit", 40));
                break;
            case "3":
                _player = new Player(name, 140, 70);
                _player.EquipWeapon(Weapon.CreateBaseballBat());
                break;
            default:
                _player = new Player(name, 120, 80);
                break;
        }

        Console.WriteLine();
        Console.WriteLine(_player.GetStatus());
    }

    private void StartingLoadout()
    {
        if (_player.EquippedWeapon == null)
        {
            _player.EquipWeapon(Weapon.CreateKnife());
        }
        _player.CollectItem(new FoodItem("Canned beans", "Basic rations", 20));
        _player.CollectItem(new MedicineItem("Bandage", "Stops the bleeding", 20));
    }

    private List<Zombie> SpawnWave(int waveNumber)
    {
        var walkerTemplate = new WalkerZombie("Walker", 30 + waveNumber * 5, 8 + waveNumber, 20);
        var runnerTemplate = new RunnerZombie("Runner", 25 + waveNumber * 4, 10 + waveNumber, 30);

        var wave = new List<Zombie>();

        int walkerCount = 1 + waveNumber;
        for (int i = 0; i < walkerCount; i++)
        {
            wave.Add(new WalkerZombie(walkerTemplate)); 
        }

        if (waveNumber >= 2)
        {
            int runnerCount = waveNumber - 1;
            for (int i = 0; i < runnerCount; i++)
            {
                wave.Add(new RunnerZombie(runnerTemplate));
            }
        }

        return wave;
    }

    private Zombie CreateFinalBoss() => new BossZombie("The Source", 150, 18, 200, 5);

    private void AwardLoot()
    {
        int roll = Rng.Next(4);
        ICollectible loot;
        switch (roll)
        {
            case 0:
                loot = new FoodItem("Energy bar", "Quick calories", 15);
                break;
            case 1:
                loot = new MedicineItem("Bandage", "Basic first aid", 15);
                break;
            case 2:
                loot = Weapon.CreatePistol();
                break;
            default:
                loot = Weapon.CreateShotgun();
                break;
        }
        _player.CollectItem(loot);
    }

    private bool RunWave(List<Zombie> zombies)
    {
        while (_player.IsAlive && zombies.Any(z => z.IsAlive))
        {
            Console.WriteLine();
            Console.WriteLine(_player.GetStatus());
            Console.WriteLine("Zombies remaining:");
            foreach (Zombie z in zombies.Where(z => z.IsAlive))
            {
                Console.WriteLine("  " + z.GetStatus());
            }

            Console.WriteLine();
            Console.WriteLine("1) Attack   2) Use item   3) Check inventory   4) Take refuge in shelter");
            Console.Write("> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Zombie target = zombies.FirstOrDefault(z => z.IsAlive);
                    if (target != null)
                    {
                        _player.AttackTarget(target);
                    }
                    break;
                case "2":
                    Console.Write("Item name: ");
                    string itemName = Console.ReadLine();
                    _player.UseItem(itemName);
                    break;
                case "3":
                    _player.Inventory.PrintContents();
                    break;
                case "4":
                    Console.WriteLine(_shelter.TakeRefuge(_player));
                    break;
                default:
                    Console.WriteLine("Not a valid choice.");
                    break;
            }

            foreach (Zombie z in zombies.Where(z => z.IsAlive))
            {
                if (!_player.IsAlive) break;
                int dmg = z.PerformAttack();
                Console.WriteLine($"{z.Name} attacks {_player.Name}!");
                _player.TakeDamage(dmg);
            }

            _player.LeaveShelter();

            if (!_player.IsAlive) return false;

            if (zombies.All(z => !z.IsAlive))
            {
                foreach (Zombie z in zombies)
                {
                    _player.GainXp(z.XpReward);
                }
                return true;
            }
        }

        return _player.IsAlive;
    }

    private void PrintVictory()
    {
        Console.WriteLine();
        Console.WriteLine("======================================");
        Console.WriteLine("   YOU SURVIVED THE OUTBREAK - YOU WIN ");
        Console.WriteLine("======================================");
        Console.WriteLine(_player.GetStatus());
        Console.WriteLine($"Zombies defeated across the game: {Zombie.TotalZombiesDefeated}");
    }

    private void PrintDefeat()
    {
        Console.WriteLine();
        Console.WriteLine("======================================");
        Console.WriteLine("        YOU HAVE BEEN OVERRUN          ");
        Console.WriteLine("======================================");
        Console.WriteLine($"{_player.Name} fell during wave {_currentWave}, at level {_player.Level}.");
        Console.WriteLine($"Zombies defeated before falling: {Zombie.TotalZombiesDefeated}");
    }
}
