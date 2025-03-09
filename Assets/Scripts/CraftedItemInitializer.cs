using UnityEngine;
using System.Collections.Generic;

public class CraftedItemInitializer : MonoBehaviour {
    public List<CraftedItem> craftedItems; // A craftolt t�rgyak list�ja

    void Start()
    {
        // Inicializ�ljuk a craftolt t�rgyakat
        InitializeCraftedItems();

        // Ki�rjuk a craftolt t�rgyakat (tesztel�shez)
        foreach (var craftedItem in craftedItems)
        {
            Debug.Log($"Craftolt t�rgy l�trehozva: {craftedItem.craftedItemName}");
        }
    }

    void InitializeCraftedItems()
    {
        craftedItems = new List<CraftedItem> {
            new CraftedItem {
                craftedItemName = "Var�zskrist�ly",
                icon = LoadSprite("Assets/Art/OtherMats/varazskristaly_icon.png"),
                description = "Egy var�zslatos krist�ly, amely er�teljes var�zslatokhoz sz�ks�ges."
            },
            new CraftedItem {
                craftedItemName = "Er�s�tett Var�zskrist�ly",
                icon = LoadSprite("Assets/Art/OtherMats/erositett_varazskristaly_icon.png"),
                description = "Egy er�s�tett var�zskrist�ly, amely nagyobb var�zser�vel rendelkezik."
            },
            // Tov�bbi craftolt t�rgyak hozz�ad�sa...
        };
    }

    // Sprite bet�lt�se (seg�dmet�dus)
    private Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }
}