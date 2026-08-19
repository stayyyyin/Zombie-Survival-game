namespace ZombieSurvivalGame;


public class WalkerZombie : Zombie
{
    public WalkerZombie(string name, int health, int attackDamage, int xpReward)
        : base(name, health, attackDamage, xpReward)
    {
    }

    public WalkerZombie(WalkerZombie other) : base(other)
    {
    }

    public override int PerformAttack() => AttackDamage;
}

public class RunnerZombie : Zombie
{
    public RunnerZombie(string name, int health, int attackDamage, int xpReward)
        : base(name, health, attackDamage, xpReward)
    {
    }

    public RunnerZombie(RunnerZombie other) : base(other)
    {
    }

    public override int PerformAttack()
    {
        int damage = AttackDamage;
        if (ZombieGame.Rng.Next(100) < 30)
        {
            damage += AttackDamage / 2;
            Console.WriteLine($"{Name} lunges for an extra hit!");
        }
        return damage;
    }
}

public class BossZombie : Zombie
{
    public int DamageResistance { get; private set; }

    public BossZombie(string name, int health, int attackDamage, int xpReward, int damageResistance)
        : base(name, health, attackDamage, xpReward)
    {
        DamageResistance = damageResistance;
    }

    public BossZombie(BossZombie other) : base(other)
    {
        DamageResistance = other.DamageResistance;
    }

    public override int PerformAttack() => AttackDamage + ZombieGame.Rng.Next(0, 6);

    public override void TakeDamage(int amount)
    {
        int reduced = amount - DamageResistance;
        if (reduced < 1) reduced = 1;
        Console.WriteLine($"{Name} resists {DamageResistance} damage from the hit!");
        base.TakeDamage(reduced);
    }

    public override string GetStatus()
    {
        return "[FINAL BOSS] " + base.GetStatus() + $" | Resistance: {DamageResistance}";
    }
}
