using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public InventoryManager inventoryManager;
    public TextMeshProUGUI[] itemTexts; // Tömb a Text elemeknek

    void Start()
    {
        // Példa: Tárgyak megjelenítése
        for (int i = 0; i < itemTexts.Length; i++)
        {
            DisplayItem(i + 1, itemTexts[i]); // Tárgy ID: 1-tõl kezdve
        }
    }

    void DisplayItem(int itemId, TextMeshProUGUI itemText)
    {
        string itemName = inventoryManager.GetItemEnglishName(itemId);
        int quantity = inventoryManager.LoadItemQuantity(itemId);
        itemText.text = $"{itemName}: {quantity}";
    }
}