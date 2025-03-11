using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    // A n�v�nyek �s a hozz�juk tartoz� mennyis�gek t�rol�sa
    public Dictionary<PlantDatabase.Item, int> items = new Dictionary<PlantDatabase.Item, int>();

    // T�rgy hozz�ad�sa az inventoryhoz
    public void AddItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Ha m�r van ilyen t�rgy az inventoryban, n�velj�k a mennyis�get
            items[item] += quantity;
        } else
        {
            // Ha nincs, hozz�adjuk az inventoryhoz
            items.Add(item, quantity);
        }

        Debug.Log($"Item added: {item.englishName}, Quantity: {quantity}");
    }

    // T�rgy elt�vol�t�sa az inventoryb�l
    public void RemoveItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Cs�kkentj�k a mennyis�get
            items[item] -= quantity;

            // Ha a mennyis�g 0 vagy ann�l kisebb, elt�vol�tjuk a t�rgyat
            if (items[item] <= 0)
            {
                items.Remove(item);
                Debug.Log($"{item.englishName} elt�vol�tva az inventoryb�l.");
            } else
            {
                Debug.Log($"{item.englishName} mennyis�ge cs�kkentve. �j mennyis�g: {items[item]}");
            }
        } else
        {
            Debug.LogWarning($"{item.englishName} nem tal�lhat� az inventoryban.");
        }
    }

    // Inventory tartalm�nak ki�r�sa (debug c�lokra)
    public void PrintInventory()
    {
        Debug.Log("Inventory contents:");
        foreach (var entry in items)
        {
            Debug.Log($"{entry.Key.englishName}: {entry.Value} db");
        }
    }
}