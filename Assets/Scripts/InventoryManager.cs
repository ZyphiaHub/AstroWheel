using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public ItemDatabase itemDatabase; // ScriptableObject referenciája

    // Tárgy mennyiségének mentése PlayerPrefs-be
    public void SaveItemQuantity(int itemId, int quantity)
    {
        PlayerPrefs.SetInt("ItemQuantity_" + itemId, quantity);
        PlayerPrefs.Save();
        Debug.Log($"Item {itemId} quantity saved: {quantity}");
    }

    // Tárgy mennyiségének betöltése PlayerPrefs-bõl
    public int LoadItemQuantity(int itemId)
    {
        return PlayerPrefs.GetInt("ItemQuantity_" + itemId, 0); // Alapértelmezett érték: 0
    }

    // Tárgy nevének lekérése
    public string GetItemEnglishName(int itemId)
    {
        if (itemId >= 0 && itemId < itemDatabase.items.Length)
        {
            return itemDatabase.items[itemId].englishName;
        }
        return "Unknown Item";
    }
}