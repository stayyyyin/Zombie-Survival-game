namespace ZombieSurvivalGame;

public abstract class Weapon : ICollectible
{
    public string Name { get; protected set; }
    public int Damage { get; protected set; }
    public int Durability { get; protected set; }
    public int MaxDurability { get; protected set; }
    public bool IsBroken => Durability <= 0;
    public int SlotsRequired => 1;

    protected Weapon(string name, int damage, int durability)
    {
        Name = name;
        Damage = damage;
        Durability = durability;
        MaxDurability = durability;
    }

    protected Weapon(Weapon other)
    {
        Name = other.Name;
        Damage = other.Damage;
        Durability = other.Durability;
        MaxDurability = other.MaxDurability;
    }

    public abstract int Attack();

    public virtual int Attack(Character target)
    {
        int damageDealt = Attack();
        if (damageDealt > 0)
        {
            target.TakeDamage(damageDealt);
        }
        return damageDealt;
    }

    protected void ReduceDurability()
    {
        Durability--;
        if (Durability < 0) Durability = 0;
    }

    protected abstract Weapon Upgrade(int bonusDamage);

    public virtual string GetDisplayInfo() =>
        $"{Name} - Damage: {Damage} | Durability: {Durability}/{MaxDurability}";

    public abstract ICollectible Clone();

    public static Weapon operator +(Weapon a, Weapon b)
    {
        if (a == null) return b;
        if (b == null) return a;
        int bonus = b.Damage / 2;
        return a.Upgrade(bonus);
    }

    public static bool operator >(Weapon a, Weapon b)
    {
        if (a == null || b == null) return false;
        return a.Damage > b.Damage;
    }

    public static bool operator <(Weapon a, Weapon b)
    {
        if (a == null || b == null) return false;
        return a.Damage < b.Damage;
    }

    public static Weapon CreateKnife() => new MeleeWeapon("Knife", 10, 60);
    public static Weapon CreateBaseballBat() => new MeleeWeapon("Baseball bat", 16, 40);
    public static Weapon CreatePistol() => new RangedWeapon("Pistol", 22, 80, 12);
    public static Weapon CreateShotgun() => new RangedWeapon("Shotgun", 38, 50, 6);
}
