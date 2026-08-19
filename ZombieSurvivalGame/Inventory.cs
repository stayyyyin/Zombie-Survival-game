namespace ZombieSurvivalGame;

public class Inventory
{
    private readonly List<ICollectible> _items;

    public int Capacity { get; private set; }
    public int Count => _items.Count;
    public bool IsFull => Count >= Capacity;

    public Inventory() : this(10)
    {
    }

    public Inventory(int capacity)
    {
        Capacity = capacity < 1 ? 1 : capacity;
        _items = new List<ICollectible>();
    }

    public Inventory(Inventory other)
    {
        Capacity = other.Capacity;
        _items = other._items.Select(i => i.Clone()).ToList();
    }

    public bool AddItem(ICollectible item)
    {
        if (item == null || IsFull) return false;
        _items.Add(item);
        return true;
    }

    public int AddItem(IEnumerable<ICollectible> items)
    {
        int added = 0;
        foreach (ICollectible item in items)
        {
            if (!AddItem(item)) break;
            added++;
        }
        return added;
    }

    public bool RemoveItem(ICollectible item) => _items.Remove(item);

    public ICollectible FindByName(string name)
    {
        foreach (ICollectible item in _items)
        {
            if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
                return item;
        }
        return null;
    }

    public List<Weapon> GetWeapons() => _items.OfType<Weapon>().ToList();

    public void PrintContents()
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("  (empty)");
            return;
        }
        foreach (ICollectible item in _items)
        {
            Console.WriteLine($"  - {item.GetDisplayInfo()}");
        }
    }
}
