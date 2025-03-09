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

    // �j met�dus: V�letlenszer�en hozz�ad egy t�rgyat az inventoryhoz
    public void AddRandomItem(PlantDatabase plantDatabase, int quantity)
    {
        // Ellen�rizz�k, hogy van-e el�rhet� t�rgy a PlantDatabase-ben
        if (plantDatabase.items == null || plantDatabase.items.Length == 0)
        {
            Debug.LogError("Nincsenek t�rgyak a PlantDatabase-ben!");
            return;
        }

        // V�letlenszer�en kiv�lasztunk egy t�rgyat
        int randomIndex = Random.Range(0, plantDatabase.items.Length);
        PlantDatabase.Item randomItem = plantDatabase.items[randomIndex];

        // Hozz�adjuk a kiv�lasztott t�rgyat az inventoryhoz
        AddItem(randomItem, quantity);

        // Debug �zenet
        Debug.Log($"V�letlenszer�en hozz�adva: {randomItem.name} ({quantity} db)");
    }

    public void RemoveItem(PlantDatabase.Item item, int quantity)
    {
        // Ellen�rizz�k, hogy a t�rgy szerepel-e az inventoryban
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

    public void RemoveMultipleItems(List<PlantDatabase.Item> itemsToRemove, List<int> quantities)
    {
        // Ellen�rizz�k, hogy a list�k �rv�nyesek-e
        if (itemsToRemove == null || quantities == null)
        {
            Debug.LogError("A list�k nem lehetnek null �rt�k�ek!");
            return;
        }

        if (itemsToRemove.Count != quantities.Count)
        {
            Debug.LogError("A t�rgyak �s a mennyis�gek list�j�nak hossza nem egyezik!");
            return;
        }

        // V�gigmegy�nk az �sszes t�rgyon �s mennyis�gen
        for (int i = 0; i < itemsToRemove.Count; i++)
        {
            PlantDatabase.Item item = itemsToRemove[i];
            int quantity = quantities[i];

            // Megh�vjuk a RemoveItem met�dust minden t�rgyhoz
            RemoveItem(item, quantity);
        }
    }
}