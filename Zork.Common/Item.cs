namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public bool Edible;

        public bool BadEdible;

        public Item(string name, string lookDescription, string inventoryDescription, bool edible, bool goodEdible, bool badEdible)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Edible = edible;
            BadEdible = badEdible;
        }

        public override string ToString() => Name;
    }
}