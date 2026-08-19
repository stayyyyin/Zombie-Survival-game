namespace ZombieSurvivalGame;

public abstract class Zombie : Character
{
    private static int _totalZombiesDefeated;
    public static int TotalZombiesDefeated => _totalZombiesDefeated;
    public static void RegisterDefeat() => _totalZombiesDefeated++;

    public int AttackDamage { get; protected set; }
    public int XpReward { get; protected set; }

    protected Zombie(string name, int health, int attackDamage, int xpReward) : base(name, health)
    {
        AttackDamage = attackDamage;
        XpReward = xpReward;
    }

    protected Zombie(Zombie other) : base(other.Name, other.MaxHealth)
    {
        AttackDamage = other.AttackDamage;
        XpReward = other.XpReward;
    }

    public abstract int PerformAttack();

    public override void TakeDamage(int amount)
    {
        bool wasAlive = IsAlive;
        base.TakeDamage(amount);
        if (wasAlive && !IsAlive)
        {
            RegisterDefeat();
            Console.WriteLine($"{Name} has been defeated!");
        }
    }

    public override string GetStatus()
    {
        return $"{Name} - HP: {Health}/{MaxHealth} | Attack: {AttackDamage}";
    }
}
