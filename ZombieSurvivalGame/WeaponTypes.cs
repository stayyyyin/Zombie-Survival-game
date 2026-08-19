namespace ZombieSurvivalGame;

public class MeleeWeapon : Weapon
{
    public MeleeWeapon(string name, int damage, int durability) : base(name, damage, durability)
    {
    }

    public MeleeWeapon(MeleeWeapon other) : base(other)
    {
    }

    public override int Attack()
    {
        if (IsBroken)
        {
            Console.WriteLine($"{Name} is too worn out to swing!");
            return 0;
        }
        ReduceDurability();
        return Damage;
    }

    protected override Weapon Upgrade(int bonusDamage)
    {
        return new MeleeWeapon(Name + "+", Damage + bonusDamage, MaxDurability);
    }

    public override string GetDisplayInfo() => base.GetDisplayInfo() + " (Melee)";

    public override ICollectible Clone() => new MeleeWeapon(this);
}

public class RangedWeapon : Weapon
{
    public int Ammo { get; private set; }

    public RangedWeapon(string name, int damage, int durability, int ammo) : base(name, damage, durability)
    {
        Ammo = ammo;
    }

    public RangedWeapon(RangedWeapon other) : base(other)
    {
        Ammo = other.Ammo;
    }

    public override int Attack()
    {
        if (Ammo <= 0)
        {
            Console.WriteLine($"{Name} is out of ammo!");
            return 0;
        }
        if (IsBroken)
        {
            Console.WriteLine($"{Name} has jammed for good!");
            return 0;
        }
        Ammo--;
        ReduceDurability();
        bool hit = ZombieGame.Rng.Next(100) < 85;
        if (!hit)
        {
            Console.WriteLine($"{Name} misses the shot!");
            return 0;
        }
        return Damage;
    }

    public void Reload(int amount) => Ammo += amount;

    protected override Weapon Upgrade(int bonusDamage)
    {
        return new RangedWeapon(Name + "+", Damage + bonusDamage, MaxDurability, Ammo);
    }

    public override string GetDisplayInfo() => base.GetDisplayInfo() + $" (Ranged, Ammo: {Ammo})";

    public override ICollectible Clone() => new RangedWeapon(this);
}
