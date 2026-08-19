namespace ZombieSurvivalGame;


public class Shelter
{
    public string Name { get; private set; }
    public int DefenseBonus { get; private set; }
    public bool IsFortified { get; private set; }

    
    public Shelter(string name, int defenseBonus) : this(name, defenseBonus, false)
    {
    }

    public Shelter(string name, int defenseBonus, bool isFortified)
    {
        Name = name;
        DefenseBonus = defenseBonus;
        IsFortified = isFortified;
    }


    public Shelter(Shelter other)
    {
        Name = other.Name;
        DefenseBonus = other.DefenseBonus;
        IsFortified = other.IsFortified;
    }

    public string TakeRefuge(Player player)
    {
        int healAmount = DefenseBonus + (IsFortified ? 10 : 0);
        player.EnterShelter();
        player.Heal(healAmount);
        return $"{player.Name} takes refuge in {Name} and feels safer.";
    }

    public void Fortify() => Fortify(10);

    public void Fortify(int amount)
    {
        DefenseBonus += amount;
        IsFortified = true;
        Console.WriteLine($"{Name} has been fortified! Defense bonus is now {DefenseBonus}.");
    }
}
