using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro névtér importálása

public class InventoryUI : MonoBehaviour {
    [Header("Plant Inventory References")]
    public PlantDatabase plantDatabase; // ItemDatabase referenciája
    public GameObject plantSlotPrefab;     // Slot prefab referenciája
    public Transform plantPanelParent;

    [Header("Crafted Inventory References")]
    public ItemDatabase itemDatabase;   // ItemDatabase referenciája
    public GameObject craftedSlotPrefab; // Crafted slot prefab referenciája
    public Transform craftedPanelParent;

    [Header("Crafting UI References")]
    public GameObject craftPanel;       // Craft panel referenciája
    public Transform recipeListParent;  // Recept lista panel referenciája
    public GameObject recipeButtonPrefab; // Recept gomb prefab referenciája
    public TextMeshProUGUI ingredientText; // Alapanyagok szövege
    public Button craftButton;          // Craft gomb referenciája

    private CraftingRecipe selectedRecipe; // Kiválasztott recept


    private void Start()
    {
        
        // Frissítjük az UI-t az inventory tartalma alapján
        RefreshInventoryUI();
        InitializeCraftPanel();
    }

    private void RefreshInventoryUI()
    {
        // Töröljük a korábbi slotokat a plant inventorybõl
        foreach (Transform child in plantPanelParent)
        {
            Destroy(child.gameObject);
        }

        // Létrehozzuk a slotokat a plant inventory tartalma alapján
        foreach (var entry in InventoryManager.Instance.inventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.englishName, entry.Value.ToString(), plantSlotPrefab, plantPanelParent);
        }

        // Töröljük a korábbi slotokat a crafted inventorybõl
        foreach (Transform child in craftedPanelParent)
        {
            Destroy(child.gameObject);
        }

        // Létrehozzuk a slotokat a crafted inventory tartalma alapján
        foreach (var entry in InventoryManager.Instance.craftedInventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.itemName, entry.Value.ToString(), craftedSlotPrefab, craftedPanelParent);
        }
    }

    // Egy új slot létrehozása és beállítása
    private void CreateSlot(Sprite icon, string itemName, string quantity, GameObject slotPrefab, Transform parent)
    {
        // Létrehozunk egy új slotot
        GameObject slot = Instantiate(slotPrefab, parent);

        // Beállítjuk az ikont
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        } else
        {
            Debug.LogError("Icon object not found in slot prefab!");
        }

        // Beállítjuk a tárgy nevét (TMP komponens használata)
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

        // Beállítjuk a mennyiséget (TMP komponens használata)
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

    /*InventoryUI.Instance.RefreshInventoryUI(); // Frissíti az UI-t*/



    // Craft panel inicializálása
    private void InitializeCraftPanel()
    {
        // Töröljük a korábbi recept gombokat
        foreach (Transform child in recipeListParent)
        {
            Destroy(child.gameObject);
        }

        // Létrehozzuk a recept gombokat
        foreach (var recipe in InventoryManager.Instance.craftingRecipe)
        {
            GameObject recipeButton = Instantiate(recipeButtonPrefab, recipeListParent);
            TextMeshProUGUI buttonText = recipeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = recipe.outputItem.itemName; // Recept neve
            }

            // Gomb eseménykezelõje
            Button button = recipeButton.GetComponent<Button>();
            button.onClick.AddListener(() => SelectRecipe(recipe));
        }

        // Craft gomb eseménykezelõje
        craftButton.onClick.AddListener(CraftSelectedRecipe);
    }

    // Recept kiválasztása
    private void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateIngredientText(recipe);
    }

    // Alapanyagok szövegének frissítése
    private void UpdateIngredientText(CraftingRecipe recipe)
    {
        string ingredientList = "Szükséges alapanyagok:\n";
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) // Növény alapanyag
            {
                int availableQuantity = InventoryManager.Instance.inventory.items.ContainsKey(ingredient.plantItem)
                    ? InventoryManager.Instance.inventory.items[ingredient.plantItem]
                    : 0;
                ingredientList += $"{ingredient.plantItem.englishName}: {ingredient.quantity} (Rendelkezésre áll: {availableQuantity})\n";
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                int availableQuantity = InventoryManager.Instance.craftedInventory.items.ContainsKey(ingredient.craftedItem)
                    ? InventoryManager.Instance.craftedInventory.items[ingredient.craftedItem]
                    : 0;
                ingredientList += $"{ingredient.craftedItem.itemName}: {ingredient.quantity} (Rendelkezésre áll: {availableQuantity})\n";
            }
        }
        ingredientText.text = ingredientList;
    }

    // Craftolás indítása
    private void CraftSelectedRecipe()
    {
        if (selectedRecipe != null)
        {
            bool success = InventoryManager.Instance.CraftItem(selectedRecipe);
            if (success)
            {
                Debug.Log("Craftolás sikeres!");
                RefreshInventoryUI(); // Frissítjük az UI-t
                UpdateIngredientText(selectedRecipe); // Frissítjük az alapanyagok listáját
            } else
            {
                Debug.Log("Craftolás sikertelen: nincs elegendõ alapanyag.");
            }
        } else
        {
            Debug.LogWarning("Nincs kiválasztva recept!");
        }
    }

    


}