namespace ZombieSurvivalGame;

public abstract class Character : IDamageable
{
    private int _health;
    private int _maxHealth;

    public string Name { get; protected set; }
    public int Health => _health;
    public int MaxHealth => _maxHealth;
    public bool IsAlive => _health > 0;

    protected Character(string name, int maxHealth)
    {
        Name = name;
        _maxHealth = maxHealth;
        _health = maxHealth;
    }

    protected Character(string name, int maxHealth, int currentHealth)
    {
        Name = name;
        _maxHealth = maxHealth;
        _health = currentHealth < 0 ? 0 : (currentHealth > maxHealth ? maxHealth : currentHealth);
    }

    public virtual void TakeDamage(int amount)
    {
        if (amount < 0) amount = 0;
        _health -= amount;
        if (_health < 0) _health = 0;
        Console.WriteLine($"{Name} takes {amount} damage. ({_health}/{_maxHealth} HP)");
    }

    public virtual void Heal(int amount)
    {
        if (amount < 0) amount = 0;
        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
        Console.WriteLine($"{Name} heals {amount} HP. ({_health}/{_maxHealth} HP)");
    }

    public abstract string GetStatus();

    protected void SetMaxHealth(int newMax)
    {
        _maxHealth = newMax < 1 ? 1 : newMax;
        if (_health > _maxHealth) _health = _maxHealth;
    }
}
