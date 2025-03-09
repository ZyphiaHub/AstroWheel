using UnityEngine;
using System.Collections.Generic;

public class CraftedItemInitializer : MonoBehaviour {
    public List<CraftedItem> craftedItems; // A craftolt tárgyak listája

    void Start()
    {
        // Inicializáljuk a craftolt tárgyakat
        InitializeCraftedItems();

        // Kiírjuk a craftolt tárgyakat (teszteléshez)
        foreach (var craftedItem in craftedItems)
        {
            Debug.Log($"Craftolt tárgy létrehozva: {craftedItem.craftedItemName}");
        }
    }

    void InitializeCraftedItems()
    {
        craftedItems = new List<CraftedItem> {
            new CraftedItem {
                craftedItemName = "Varázskristály",
                icon = LoadSprite("Assets/Art/OtherMats/varazskristaly_icon.png"),
                description = "Egy varázslatos kristály, amely erõteljes varázslatokhoz szükséges."
            },
            new CraftedItem {
                craftedItemName = "Erõsített Varázskristály",
                icon = LoadSprite("Assets/Art/OtherMats/erositett_varazskristaly_icon.png"),
                description = "Egy erõsített varázskristály, amely nagyobb varázserõvel rendelkezik."
            },
            // További craftolt tárgyak hozzáadása...
        };
    }

    // Sprite betöltése (segédmetódus)
    private Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }
}