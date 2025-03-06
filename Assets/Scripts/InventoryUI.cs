using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public InventoryManager inventoryManager;
    public TextMeshProUGUI[] itemTexts; // T�mb a Text elemeknek

    void Start()
    {
        // P�lda: T�rgyak megjelen�t�se
        for (int i = 0; i < itemTexts.Length; i++)
        {
            DisplayItem(i + 1, itemTexts[i]); // T�rgy ID: 1-t�l kezdve
        }
    }

    void DisplayItem(int itemId, TextMeshProUGUI itemText)
    {
        string itemName = inventoryManager.GetItemEnglishName(itemId);
        int quantity = inventoryManager.LoadItemQuantity(itemId);
        itemText.text = $"{itemName}: {quantity}";
    }
}