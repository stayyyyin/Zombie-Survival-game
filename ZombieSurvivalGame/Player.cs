namespace ZombieSurvivalGame;

public class Player : Character
{
    private static int _nextId = 1;
    public static int TotalPlayersCreated { get; private set; } = 0;

    public static Player CreateDefault(string name) => new Player(name);

    private int _energy;
    private int _maxEnergy;
    private int _xp;
    private bool _isSheltered;

    public int Id { get; }
    public int Energy => _energy;
    public int MaxEnergy => _maxEnergy;
    public int Level { get; private set; }
    public int Xp => _xp;
    public int XpToNextLevel { get; private set; }
    public Inventory Inventory { get; private set; }
    public Weapon EquippedWeapon { get; private set; }

    public Player(string name) : this(name, 100, 100)
    {
    }

    public Player(string name, int health, int energy) : base(name, health)
    {
        _energy = energy;
        _maxEnergy = energy;
        Level = 1;
        _xp = 0;
        XpToNextLevel = 100;
        Inventory = new Inventory();
        Id = _nextId++;
        TotalPlayersCreated++;
    }

    public Player(Player other) : base(other.Name, other.MaxHealth, other.Health)
    {
        _energy = other._energy;
        _maxEnergy = other._maxEnergy;
        Level = other.Level;
        _xp = other._xp;
        XpToNextLevel = other.XpToNextLevel;
        Inventory = new Inventory(other.Inventory);
        EquippedWeapon = other.EquippedWeapon == null ? null : (Weapon)other.EquippedWeapon.Clone();
        Id = _nextId++;
        TotalPlayersCreated++;
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        int energyBoost = amount / 4;
        _energy += energyBoost;
        if (_energy > _maxEnergy) _energy = _maxEnergy;
    }

    public string Heal(MedicineItem medicine)
    {
        if (medicine == null) return $"{Name} has no medicine to use.";
        return medicine.Use(this);
    }

    public void RestoreEnergy(int amount)
    {
        if (amount < 0) amount = 0;
        _energy += amount;
        if (_energy > _maxEnergy) _energy = _maxEnergy;
    }

    public bool AttackTarget(Character target)
    {
        return AttackTarget(target, EquippedWeapon);
    }

    public bool AttackTarget(Character target, Weapon weapon)
    {
        if (target == null) return false;

        if (weapon == null)
        {
            Console.WriteLine($"{Name} has no weapon equipped and swings bare fists for 2 damage!");
            target.TakeDamage(2);
            return true;
        }
        if (weapon.IsBroken)
        {
            Console.WriteLine($"{weapon.Name} is broken and can't be used!");
            return false;
        }

        int damageDealt = weapon.Attack(target);
        _energy -= 5;
        if (_energy < 0) _energy = 0;
        return damageDealt > 0;
    }

    public void GainXp(int amount)
    {
        if (amount <= 0) return;
        _xp += amount;
        Console.WriteLine($"{Name} gains {amount} XP.");
        while (_xp >= XpToNextLevel)
        {
            _xp -= XpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        SetMaxHealth(MaxHealth + 20);
        Heal(20);
        _maxEnergy += 10;
        _energy = _maxEnergy;
        XpToNextLevel = (int)(XpToNextLevel * 1.25);
        Console.WriteLine($"*** {Name} reached level {Level}! Max HP is now {MaxHealth}. ***");
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        Console.WriteLine($"{Name} equips {weapon?.Name ?? "nothing"}.");
    }

    public bool CollectItem(ICollectible item)
    {
        if (item == null) return false;
        bool added = Inventory.AddItem(item);
        if (added)
            Console.WriteLine($"{Name} picked up {item.Name}.");
        else
            Console.WriteLine($"{Name}'s inventory is full - couldn't pick up {item.Name}.");
        return added;
    }

    public bool UseItem(string itemName)
    {
        ICollectible found = Inventory.FindByName(itemName);
        if (found is IUsable usable)
        {
            string message = usable.Use(this);
            Console.WriteLine(message);
            Inventory.RemoveItem(found);
            return true;
        }
        Console.WriteLine($"{Name} can't use \"{itemName}\" right now.");
        return false;
    }

    public void EnterShelter() => _isSheltered = true;
    public void LeaveShelter() => _isSheltered = false;

    public override void TakeDamage(int amount)
    {
        int actualAmount = amount;
        if (_isSheltered)
        {
            actualAmount = amount / 2;
            Console.WriteLine($"{Name} is sheltered and braces for the hit!");
        }
        base.TakeDamage(actualAmount);
    }

        public override string GetStatus()
    {
        string weaponName = EquippedWeapon == null ? "none" : EquippedWeapon.Name;
        return $"{Name} (Lv.{Level}) - HP: {Health}/{MaxHealth} | Energy: {Energy}/{MaxEnergy} | " +
               $"XP: {Xp}/{XpToNextLevel} | Weapon: {weaponName} | Inventory: {Inventory.Count}/{Inventory.Capacity}";
    }

    public static Player operator ++(Player player)
    {
        player?.GainXp(25);
        return player;
    }
}
