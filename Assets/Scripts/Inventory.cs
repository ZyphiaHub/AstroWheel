using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    // A növények és a hozzájuk tartozó mennyiségek tárolása
    public Dictionary<PlantDatabase.Item, int> items = new Dictionary<PlantDatabase.Item, int>();

    // Tárgy hozzáadása az inventoryhoz
    public void AddItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Ha már van ilyen tárgy az inventoryban, növeljük a mennyiséget
            items[item] += quantity;
        } else
        {
            // Ha nincs, hozzáadjuk az inventoryhoz
            items.Add(item, quantity);
        }

        Debug.Log($"Item added: {item.englishName}, Quantity: {quantity}");
    }

    // Tárgy eltávolítása az inventoryból
    public void RemoveItem(PlantDatabase.Item item, int quantity)
    {
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

    // Inventory tartalmának kiírása (debug célokra)
    public void PrintInventory()
    {
        Debug.Log("Inventory contents:");
        foreach (var entry in items)
        {
            Debug.Log($"{entry.Key.englishName}: {entry.Value} db");
        }
    }
}