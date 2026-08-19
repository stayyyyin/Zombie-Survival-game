namespace ZombieSurvivalGame;

public abstract class Item : ICollectible, IUsable
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int SlotsRequired => 1;

    protected Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    protected Item(Item other)
    {
        Name = other.Name;
        Description = other.Description;
    }

    public abstract string Use(Player player);

    public virtual string GetDisplayInfo() => $"{Name} - {Description}";

    public abstract ICollectible Clone();
}
