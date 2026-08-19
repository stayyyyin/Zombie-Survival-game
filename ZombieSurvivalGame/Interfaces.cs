namespace ZombieSurvivalGame;

public interface IDamageable
{
    int Health { get; }
    bool IsAlive { get; }
    void TakeDamage(int amount);
    void Heal(int amount);
}

public interface ICollectible
{
    string Name { get; }
    int SlotsRequired { get; }
    string GetDisplayInfo();
    ICollectible Clone();
}

public interface IUsable
{
    string Use(Player player);
}
