namespace ZombieSurvivalGame;

public class FoodItem : Item
{
    public int EnergyRestore { get; private set; }

    public FoodItem(string name, string description, int energyRestore) : base(name, description)
    {
        EnergyRestore = energyRestore;
    }

    public FoodItem(FoodItem other) : base(other)
    {
        EnergyRestore = other.EnergyRestore;
    }

    public override string Use(Player player)
    {
        player.RestoreEnergy(EnergyRestore);
        return $"{player.Name} eats {Name} and restores {EnergyRestore} energy.";
    }

    public override string GetDisplayInfo() => $"{Name} (Food) - restores {EnergyRestore} energy";

        public override ICollectible Clone() => new FoodItem(this);
}

public class MedicineItem : Item
{
    public int HealthRestore { get; private set; }

    public MedicineItem(string name, string description, int healthRestore) : base(name, description)
    {
        HealthRestore = healthRestore;
    }

    public MedicineItem(MedicineItem other) : base(other)
    {
        HealthRestore = other.HealthRestore;
    }

    public override string Use(Player player)
    {
        player.Heal(HealthRestore);
        return $"{player.Name} uses {Name} and recovers {HealthRestore} HP.";
    }

    public override string GetDisplayInfo() => $"{Name} (Medicine) - restores {HealthRestore} HP";

    public override ICollectible Clone() => new MedicineItem(this);
}
