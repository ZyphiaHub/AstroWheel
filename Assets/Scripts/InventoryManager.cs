using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    public Dictionary<PlantDatabase.Item, int> items = new Dictionary<PlantDatabase.Item, int>();

    public void Initialize(PlantDatabase plantDatabase)
    {
        
        AddItem(plantDatabase.items[0], 3); 
        AddItem(plantDatabase.items[1], 5); 
        AddItem(plantDatabase.items[2], 1); 
        AddItem(plantDatabase.items[3], 1);
        AddItem(plantDatabase.items[4], 1);

        AddItem(plantDatabase.items[5], 1);
        AddItem(plantDatabase.items[6], 1);
        AddItem(plantDatabase.items[7], 1);
        AddItem(plantDatabase.items[8], 1);
        AddItem(plantDatabase.items[9], 1);

        AddItem(plantDatabase.items[10], 3);
        AddItem(plantDatabase.items[11], 1);
        AddItem(plantDatabase.items[12], 4);
        AddItem(plantDatabase.items[13], 1);
        AddItem(plantDatabase.items[14], 1);

        AddItem(plantDatabase.items[15], 1);
    }

    public void AddItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        } else
        {
            items.Add(item, quantity);
        }
    }

    // Új metódus: Véletlenszerûen hozzáad egy tárgyat az inventoryhoz
    public void AddRandomItem(PlantDatabase plantDatabase, int quantity)
    {
        // Ellenõrizzük, hogy van-e elérhetõ tárgy a PlantDatabase-ben
        if (plantDatabase.items == null || plantDatabase.items.Length == 0)
        {
            Debug.LogError("Nincsenek tárgyak a PlantDatabase-ben!");
            return;
        }

        // Véletlenszerûen kiválasztunk egy tárgyat
        int randomIndex = Random.Range(0, plantDatabase.items.Length);
        PlantDatabase.Item randomItem = plantDatabase.items[randomIndex];

        // Hozzáadjuk a kiválasztott tárgyat az inventoryhoz
        AddItem(randomItem, quantity);

        // Debug üzenet
        Debug.Log($"Véletlenszerûen hozzáadva: {randomItem.name} ({quantity} db)");
    }

    public void RemoveItem(PlantDatabase.Item item, int quantity)
    {
        // Ellenõrizzük, hogy a tárgy szerepel-e az inventoryban
        if (items.ContainsKey(item))
        {
            // Csökkentjük a mennyiséget
            items[item] -= quantity;

            // Ha a mennyiség 0 vagy annál kisebb, eltávolítjuk a tárgyat
            if (items[item] <= 0)
            {
                items.Remove(item);
                Debug.Log($"{item.englishName} eltávolítva az inventoryból.");
            } else
            {
                Debug.Log($"{item.englishName} mennyisége csökkentve. Új mennyiség: {items[item]}");
            }
        } else
        {
            Debug.LogWarning($"{item.englishName} nem található az inventoryban.");
        }
    }

    public void RemoveMultipleItems(List<PlantDatabase.Item> itemsToRemove, List<int> quantities)
    {
        // Ellenõrizzük, hogy a listák érvényesek-e
        if (itemsToRemove == null || quantities == null)
        {
            Debug.LogError("A listák nem lehetnek null értékûek!");
            return;
        }

        if (itemsToRemove.Count != quantities.Count)
        {
            Debug.LogError("A tárgyak és a mennyiségek listájának hossza nem egyezik!");
            return;
        }

        // Végigmegyünk az összes tárgyon és mennyiségen
        for (int i = 0; i < itemsToRemove.Count; i++)
        {
            PlantDatabase.Item item = itemsToRemove[i];
            int quantity = quantities[i];

            // Meghívjuk a RemoveItem metódust minden tárgyhoz
            RemoveItem(item, quantity);
        }
    }
}