using System.Collections.Generic;

public class Inventory {
    public Dictionary<ItemDatabase.Item, int> items = new Dictionary<ItemDatabase.Item, int>();

    public void Initialize(ItemDatabase itemDatabase)
    {
        // Példa: Hozzáadunk néhány tárgyat az inventoryhoz
        AddItem(itemDatabase.items[0], 3); // 3 darab Shepherd's purse
        AddItem(itemDatabase.items[1], 5); // 5 darab Narrowleaf plantain
        AddItem(itemDatabase.items[2], 1); // 1 darab Crystal
        AddItem(itemDatabase.items[3], 1);
        AddItem(itemDatabase.items[4], 1);

        AddItem(itemDatabase.items[5], 1);
        AddItem(itemDatabase.items[6], 1);
        AddItem(itemDatabase.items[7], 1);
        AddItem(itemDatabase.items[8], 1);
        AddItem(itemDatabase.items[9], 1);

        AddItem(itemDatabase.items[10], 1);
        AddItem(itemDatabase.items[11], 1);
    }

    public void AddItem(ItemDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        } else
        {
            items.Add(item, quantity);
        }
    }
}