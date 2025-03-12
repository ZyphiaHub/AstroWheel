using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro n�vt�r import�l�sa

public class InventoryUI : MonoBehaviour {
    [Header("Plant Inventory References")]
    public PlantDatabase plantDatabase; // ItemDatabase referenci�ja
    public GameObject plantSlotPrefab;     // Slot prefab referenci�ja
    public Transform plantPanelParent;

    [Header("Crafted Inventory References")]
    public ItemDatabase itemDatabase;   // ItemDatabase referenci�ja
    public GameObject craftedSlotPrefab; // Crafted slot prefab referenci�ja
    public Transform craftedPanelParent;

    [Header("Crafting UI References")]
    public GameObject craftPanel;       // Craft panel referenci�ja
    public Transform recipeListParent;  // Recept lista panel referenci�ja
    public GameObject recipeButtonPrefab; // Recept gomb prefab referenci�ja
    public TextMeshProUGUI ingredientText; // Alapanyagok sz�vege
    public Button craftButton;          // Craft gomb referenci�ja

    private CraftingRecipe selectedRecipe; // Kiv�lasztott recept


    private void Start()
    {
        
        // Friss�tj�k az UI-t az inventory tartalma alapj�n
        RefreshInventoryUI();
        InitializeCraftPanel();
    }

    private void RefreshInventoryUI()
    {
        // T�r�lj�k a kor�bbi slotokat a plant inventoryb�l
        foreach (Transform child in plantPanelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat a plant inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.inventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.englishName, entry.Value.ToString(), plantSlotPrefab, plantPanelParent);
        }

        // T�r�lj�k a kor�bbi slotokat a crafted inventoryb�l
        foreach (Transform child in craftedPanelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat a crafted inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.craftedInventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.itemName, entry.Value.ToString(), craftedSlotPrefab, craftedPanelParent);
        }
    }

    // Egy �j slot l�trehoz�sa �s be�ll�t�sa
    private void CreateSlot(Sprite icon, string itemName, string quantity, GameObject slotPrefab, Transform parent)
    {
        // L�trehozunk egy �j slotot
        GameObject slot = Instantiate(slotPrefab, parent);

        // Be�ll�tjuk az ikont
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        } else
        {
            Debug.LogError("Icon object not found in slot prefab!");
        }

        // Be�ll�tjuk a t�rgy nev�t (TMP komponens haszn�lata)
        Transform nameTransform = slot.transform.Find("Label/Name");
        if (nameTransform != null)
        {
            TextMeshProUGUI nameLabel = nameTransform.GetComponent<TextMeshProUGUI>();
            if (nameLabel != null)
            {
                nameLabel.text = itemName;
            } else
            {
                Debug.LogError("TextMeshProUGUI component not found on Name object!");
            }
        } else
        {
            Debug.LogError("Name object not found in slot prefab!");
        }

        // Be�ll�tjuk a mennyis�get (TMP komponens haszn�lata)
        Transform stackTransform = slot.transform.Find("Stack");
        if (stackTransform != null)
        {
            TextMeshProUGUI quantityText = stackTransform.GetComponent<TextMeshProUGUI>();
            if (quantityText != null)
            {
                quantityText.text = quantity;
            } else
            {
                Debug.LogError("TextMeshProUGUI component not found on Stack object!");
            }
        } else
        {
            Debug.LogError("Stack object not found in slot prefab!");
        }
    }

    /*InventoryUI.Instance.RefreshInventoryUI(); // Friss�ti az UI-t*/



    // Craft panel inicializ�l�sa
    private void InitializeCraftPanel()
    {
        // T�r�lj�k a kor�bbi recept gombokat
        foreach (Transform child in recipeListParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a recept gombokat
        foreach (var recipe in InventoryManager.Instance.craftingRecipe)
        {
            GameObject recipeButton = Instantiate(recipeButtonPrefab, recipeListParent);
            TextMeshProUGUI buttonText = recipeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = recipe.outputItem.itemName; // Recept neve
            }

            // Gomb esem�nykezel�je
            Button button = recipeButton.GetComponent<Button>();
            button.onClick.AddListener(() => SelectRecipe(recipe));
        }

        // Craft gomb esem�nykezel�je
        craftButton.onClick.AddListener(CraftSelectedRecipe);
    }

    // Recept kiv�laszt�sa
    private void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateIngredientText(recipe);
    }

    // Alapanyagok sz�veg�nek friss�t�se
    private void UpdateIngredientText(CraftingRecipe recipe)
    {
        string ingredientList = "Sz�ks�ges alapanyagok:\n";
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) // N�v�ny alapanyag
            {
                int availableQuantity = InventoryManager.Instance.inventory.items.ContainsKey(ingredient.plantItem)
                    ? InventoryManager.Instance.inventory.items[ingredient.plantItem]
                    : 0;
                ingredientList += $"{ingredient.plantItem.englishName}: {ingredient.quantity} (Rendelkez�sre �ll: {availableQuantity})\n";
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                int availableQuantity = InventoryManager.Instance.craftedInventory.items.ContainsKey(ingredient.craftedItem)
                    ? InventoryManager.Instance.craftedInventory.items[ingredient.craftedItem]
                    : 0;
                ingredientList += $"{ingredient.craftedItem.itemName}: {ingredient.quantity} (Rendelkez�sre �ll: {availableQuantity})\n";
            }
        }
        ingredientText.text = ingredientList;
    }

    // Craftol�s ind�t�sa
    private void CraftSelectedRecipe()
    {
        if (selectedRecipe != null)
        {
            bool success = InventoryManager.Instance.CraftItem(selectedRecipe);
            if (success)
            {
                Debug.Log("Craftol�s sikeres!");
                RefreshInventoryUI(); // Friss�tj�k az UI-t
                UpdateIngredientText(selectedRecipe); // Friss�tj�k az alapanyagok list�j�t
            } else
            {
                Debug.Log("Craftol�s sikertelen: nincs elegend� alapanyag.");
            }
        } else
        {
            Debug.LogWarning("Nincs kiv�lasztva recept!");
        }
    }

    


}