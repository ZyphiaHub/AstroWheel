using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }
    public APIClient apiClient;
    public Inventory inventory { get;  set; }
    public CraftedInventory craftedInventory { get;  set; } 

    [Header("References")]
    public PlantDatabase plantDatabase; 
    public ItemDatabase itemDatabase; 

    [Header("Crafting Recipes")]
    public List<CraftingRecipe> craftingRecipe;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else
        {
            Destroy(gameObject); 
            return;
        }

        InitializeInventories();
    }

    // Inventoryk inicializálása és betöltése
    public void InitializeInventories()
    {
        // Plant inventory inicializálása
        inventory = new Inventory();
        bool hasSavedPlantData = false;

        foreach (var item in plantDatabase.items)
        {
            string key = "plant_" + item.englishName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                hasSavedPlantData = true;
                break;
            }
        }

        if (hasSavedPlantData)
        {
            LoadInventory();
        } else
        {
            foreach (var item in plantDatabase.items)
            {
                inventory.AddItem(item, 0);
            }
            SaveInventory();
            Debug.Log("New plant inventory initialized with 0 quantities.");
        }

        // Crafted inventory inicializálása
        craftedInventory = new CraftedInventory();
        LoadCraftedInventory();

        // Ellenõrizzük, hogy minden tárgy szerepel-e a CraftedInventory-ben
        foreach (var item in itemDatabase.items)
        {
            if (!craftedInventory.items.ContainsKey(item))
            {
                // Ha nincs benne, hozzáadjuk 0 mennyiséggel
                craftedInventory.AddItem(item, 0);
                Debug.Log($"New item added to CraftedInventory: {item.itemName}");
            }
        }

        // Mentjük a frissített CraftedInventory-t
        SaveCraftedInventory();
        Debug.Log("Crafted inventory initialized with new items.");

        
    }

    // Plant inventory mentése PlayerPrefs-be
    public void SaveInventory()
    {
        foreach (var entry in plantDatabase.items)
        {
            string key = "plant_" + entry.englishName.Replace(" ", "");
            int quantity = inventory.items.ContainsKey(entry) ? inventory.items[entry] : 0;
            PlayerPrefs.SetInt(key, quantity);
        }

        PlayerPrefs.Save();
        //Debug.Log("Plant inventory saved to prefs.");

        // Mentés SQLite adatbázisba
        int inventoryId = PlayerPrefs.GetInt("InventoryID", -1);
        if (inventoryId != -1)
        {
            LocalDatabaseManager.Instance.SaveInventoryData(inventoryId, inventory.items);
            Debug.Log("inventory saved to sqlite");
        } else
        {
            Debug.LogError("InventoryId not found in PlayerPrefs. Make sure the player is logged in.");
        }

    }
    // Metódus az inventory adatainak küldésére
    public void SaveInventoryToServer()
    {
        // A PlayerID lekérése a PlayerPrefs-bõl
        int inventoryId = PlayerPrefs.GetInt("InventoryID", -1); 

        if (inventoryId == -1)
        {
            Debug.LogError("InventoryId not found in PlayerPrefs. Make sure the player is logged in.");
            return;
        }

        // Az inventory adatainak összeállítása
        List<InventoryData> inventoryDataList = new List<InventoryData>();
        foreach (var entry in inventory.items)
        {
            inventoryDataList.Add(new InventoryData
            {
                InventoryId = inventoryId,
                MaterialId = entry.Key.plantId,
                Quantity = entry.Value
            });
        }
        if (APIClient.Instance == null)
        {
            Debug.LogError("APIClient.Instance is null! Make sure it is initialized properly.");
            return;
        }
        // Az adatok küldése a szerverre
        StartCoroutine(APIClient.Instance.SaveInventory(inventoryId, inventoryDataList.ToArray(),
            onSuccess: response =>
            {
                //Debug.Log("Inventory saved to server: " + response);
            },
            onError: error =>
            {
                Debug.LogError("Failed to save inventory: " + error);
            }

        ));
    }


    // Crafted inventory mentése PlayerPrefs-be
    public void SaveCraftedInventory()
    {

        foreach (var entry in itemDatabase.items)
        {
            string key = "crafted_" + entry.itemName.Replace(" ", "");
            int quantity = craftedInventory.items.ContainsKey(entry) ? craftedInventory.items[entry] : 0;
            PlayerPrefs.SetInt(key, quantity);
        }

        PlayerPrefs.Save();
        //Debug.Log("Crafted inventory saved.");

        // Mentés SQLite adatbázisba
        int inventoryId = PlayerPrefs.GetInt("InventoryID", -1);
        if (inventoryId != -1)
        {

            // Mentés az SQLite adatbázisba
            LocalDatabaseManager.Instance.SaveCraftedInventoryData(inventoryId, craftedInventory.items);
            Debug.Log("Crafted inventory saved to SQLite database.");
        } else
        {
            Debug.LogError("InventoryId not found in PlayerPrefs. Make sure the player is logged in.");
        }

    }
    // Metódus a crafted inventory adatainak küldésére
    public void SaveCraftedInventoryToServer()
    {
        //Debug.Log($"SaveCraftedInventoryToServer called. Inventory is null? {craftedInventory == null}");

        if (craftedInventory == null)
        {
            Debug.LogError("Inventory is null! It was not initialized properly.");
            return;
        }

        if (craftedInventory.items == null)
        {
            Debug.LogError("Inventory items dictionary is null!");
            return;
        }
        // A PlayerID lekérése a PlayerPrefs-bõl
        int inventoryId = PlayerPrefs.GetInt("InventoryID", -1);


        // A crafted inventory adatainak összeállítása
        List<InventoryData> craftedDataList = new List<InventoryData>();
        foreach (var entry in craftedInventory.items)
        {
            craftedDataList.Add(new InventoryData
            {
                InventoryId = inventoryId,
                MaterialId = entry.Key.itemId,
                Quantity = entry.Value
            });
        }

        // Az adatok küldése a szerverre
        StartCoroutine(APIClient.Instance.SaveCraftedInventory(inventoryId, craftedDataList.ToArray(),
            onSuccess: response =>
            {
                //Debug.Log("Crafted inventory saved to server: " + response);
            },
            onError: error =>
            {
                Debug.LogError("Failed to save crafted inventory: " + error);
            }
        ));
    }

    // Plant inventory betöltése PlayerPrefs-bõl
    public void LoadInventory()
    {
        foreach (var item in plantDatabase.items)
        {
            string key = "plant_" + item.englishName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                int quantity = PlayerPrefs.GetInt(key);
                inventory.AddItem(item, quantity);
            }
        }
        Debug.Log("Plant inventory loaded.");
        /*// Betöltés SQLite adatbázisból
        int inventoryId = PlayerPrefs.GetInt("InventoryID", -1);
        if (inventoryId != -1)
        {
            var inventoryData = LocalDatabaseManager.Instance.LoadInventoryData(inventoryId);
            foreach (var entry in inventoryData)
            {
                var plantItem = plantDatabase.items.FirstOrDefault(x => x.plantId == entry.Key);
                if (plantItem != null)
                {
                    inventory.AddItem(plantItem, entry.Value);
                }
            }
            Debug.Log("Plant inventory loaded from SQLite database.");
        } else
        {
            Debug.LogError("InventoryId not found in PlayerPrefs. Make sure the player is logged in.");
        }*/
    }

    // Crafted inventory betöltése PlayerPrefs-bõl
    public void LoadCraftedInventory()
    {

        foreach (var item in itemDatabase.items)
        {
            string key = "crafted_" + item.itemName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                int quantity = PlayerPrefs.GetInt(key);
                craftedInventory.items[item] = quantity; // Frissítjük a mennyiséget
            }
        }
        Debug.Log("Crafted inventory loaded.");
        
    }

    // PlayerPrefs törlése (mindkét inventoryhoz)
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
    }


    /*CRAFTING*/
    public bool CraftItem(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) 
            {
                if (!inventory.items.ContainsKey(ingredient.plantItem) || inventory.items[ingredient.plantItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elegendõ {ingredient.plantItem.englishName} a craftoláshoz.");
                    return false; 
                }
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                if (!craftedInventory.items.ContainsKey(ingredient.craftedItem) || craftedInventory.items[ingredient.craftedItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elegendõ {ingredient.craftedItem.itemName} a craftoláshoz.");
                    return false; 
                }
            }
        }

        // Levonjuk az alapanyagokat
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) // Növény alapanyag
            {
                inventory.RemoveItem(ingredient.plantItem, ingredient.quantity);
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                craftedInventory.RemoveItem(ingredient.craftedItem, ingredient.quantity);
            }
        }

        // Hozzáadjuk a kimeneti crafted itemet
        craftedInventory.AddItem(recipe.outputItem, recipe.outputQuantity);
        Debug.Log($"Craftolás sikeres: {recipe.outputQuantity} db {recipe.outputItem.itemName} létrehozva.");
        SaveInventory();
        SaveCraftedInventory();

        SaveInventoryToServer(); 
        SaveCraftedInventoryToServer();
        return true; // Craftolás sikeres
    }
}



/*// Tárgyak hozzáadása a leltárakhoz
InventoryManager.Instance.inventory.AddItem(plantDatabase.items[0], 5); // 5 növény hozzáadása
InventoryManager.Instance.craftedInventory.AddItem(itemDatabase.items[0], 2); // 2 elkészített tárgy hozzáadása

// Leltárak mentése
InventoryManager.Instance.SaveInventory();
InventoryManager.Instance.SaveCraftedInventory();

// Leltárak betöltése
InventoryManager.Instance.LoadInventory();
InventoryManager.Instance.LoadCraftedInventory();

// Leltárak tartalmának kiírása
InventoryManager.Instance.inventory.PrintInventory();
InventoryManager.Instance.craftedInventory.PrintInventory();*/