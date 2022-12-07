namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public bool Edible;

        public int HealthGain;

        public int Hungergain;

        public Item(string name, string lookDescription, string inventoryDescription, bool edible, int healthGain, int hungerGain)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Edible = edible;
            HealthGain = healthGain;
            Hungergain = hungerGain;
        }

        public override string ToString() => Name;
    }
}