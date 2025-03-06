using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public ItemDatabase itemDatabase; // ScriptableObject referenci�ja

    // T�rgy mennyis�g�nek ment�se PlayerPrefs-be
    public void SaveItemQuantity(int itemId, int quantity)
    {
        PlayerPrefs.SetInt("ItemQuantity_" + itemId, quantity);
        PlayerPrefs.Save();
        Debug.Log($"Item {itemId} quantity saved: {quantity}");
    }

    // T�rgy mennyis�g�nek bet�lt�se PlayerPrefs-b�l
    public int LoadItemQuantity(int itemId)
    {
        return PlayerPrefs.GetInt("ItemQuantity_" + itemId, 0); // Alap�rtelmezett �rt�k: 0
    }

    // T�rgy nev�nek lek�r�se
    public string GetItemEnglishName(int itemId)
    {
        if (itemId >= 0 && itemId < itemDatabase.items.Length)
        {
            return itemDatabase.items[itemId].englishName;
        }
        return "Unknown Item";
    }
}