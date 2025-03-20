using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }
    public APIClient apiClient;
    public Inventory inventory { get;  set; }
    public CraftedInventory craftedInventory { get;  set; } 

    [Header("References")]
    public PlantDatabase plantDatabase; // Az Inspectorban �ll�tsd be!
    public ItemDatabase itemDatabase; // �j ItemDatabase referenci�ja

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

    // Inventoryk inicializ�l�sa �s bet�lt�se
    public void InitializeInventories()
    {
        // Plant inventory inicializ�l�sa
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

        // Crafted inventory inicializ�l�sa
        craftedInventory = new CraftedInventory();
        LoadCraftedInventory();

        // Ellen�rizz�k, hogy minden t�rgy szerepel-e a CraftedInventory-ben
        foreach (var item in itemDatabase.items)
        {
            if (!craftedInventory.items.ContainsKey(item))
            {
                // Ha nincs benne, hozz�adjuk 0 mennyis�ggel
                craftedInventory.AddItem(item, 0);
                Debug.Log($"New item added to CraftedInventory: {item.itemName}");
            }
        }

        // Mentj�k a friss�tett CraftedInventory-t
        SaveCraftedInventory();
        Debug.Log("Crafted inventory initialized with new items.");

        
    }

    // Plant inventory ment�se PlayerPrefs-be
    public void SaveInventory()
    {
        foreach (var entry in plantDatabase.items)
        {
            string key = "plant_" + entry.englishName.Replace(" ", "");
            int quantity = inventory.items.ContainsKey(entry) ? inventory.items[entry] : 0;
            PlayerPrefs.SetInt(key, quantity);
        }

        PlayerPrefs.Save();
        Debug.Log("Plant inventory saved.");
    }
    // Met�dus az inventory adatainak k�ld�s�re
    public void SaveInventoryToServer()
    {
        // A PlayerID lek�r�se a PlayerPrefs-b�l
        int playerId = PlayerPrefs.GetInt("PlayerID", -1); // -1 az alap�rtelmezett �rt�k, ha a PlayerID nem l�tezik

        if (playerId == -1)
        {
            Debug.LogError("PlayerID not found in PlayerPrefs. Make sure the player is logged in.");
            return;
        }

        // Az inventory adatainak �ssze�ll�t�sa
        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        foreach (var entry in inventory.items)
        {
            inventoryItems.Add(new InventoryItem
            {
                itemId = entry.Key.plantId, 
                quantity = entry.Value
            });
        }
        if (APIClient.Instance == null)
        {
            Debug.LogError("APIClient.Instance is null! Make sure it is initialized properly.");
            return;
        }
        // Az adatok k�ld�se a szerverre
        StartCoroutine(APIClient.Instance.SaveInventory(playerId.ToString(), inventoryItems,
            onSuccess: response =>
            {
                Debug.Log("Inventory saved to server: " + response);
            },
            onError: error =>
            {
                Debug.LogError("Failed to save inventory: " + error);
            }

        ));
    }


    // Crafted inventory ment�se PlayerPrefs-be
    public void SaveCraftedInventory()
    {

        foreach (var entry in itemDatabase.items)
        {
            string key = "crafted_" + entry.itemName.Replace(" ", "");
            int quantity = craftedInventory.items.ContainsKey(entry) ? craftedInventory.items[entry] : 0;
            PlayerPrefs.SetInt(key, quantity);
        }

        PlayerPrefs.Save();
        Debug.Log("Crafted inventory saved.");
        
    }
    // Met�dus a crafted inventory adatainak k�ld�s�re
    public void SaveCraftedInventoryToServer()
    {
        Debug.Log($"SaveInventoryToServer called. Inventory is null? {inventory == null}");

        if (inventory == null)
        {
            Debug.LogError("Inventory is null! It was not initialized properly.");
            return;
        }

        if (inventory.items == null)
        {
            Debug.LogError("Inventory items dictionary is null!");
            return;
        }
        // A PlayerID lek�r�se a PlayerPrefs-b�l
        int playerId = PlayerPrefs.GetInt("PlayerID", -1); // -1 az alap�rtelmezett �rt�k, ha a PlayerID nem l�tezik

        if (playerId == -1)
        {
            Debug.LogError("PlayerID not found in PlayerPrefs. Make sure the player is logged in.");
            return;
        }

        // A crafted inventory adatainak �ssze�ll�t�sa
        List<InventoryItem> craftedItems = new List<InventoryItem>();
        foreach (var entry in craftedInventory.items)
        {
            craftedItems.Add(new InventoryItem
            {
                itemId = entry.Key.itemId, // Felt�telezve, hogy az Item oszt�lynak van itemId tulajdons�ga
                quantity = entry.Value
            });
        }

        // Az adatok k�ld�se a szerverre
        StartCoroutine(APIClient.Instance.SaveCraftedInventory(playerId.ToString(), craftedItems,
            onSuccess: response =>
            {
                Debug.Log("Crafted inventory saved to server: " + response);
            },
            onError: error =>
            {
                Debug.LogError("Failed to save crafted inventory: " + error);
            }
        ));
    }

    // Plant inventory bet�lt�se PlayerPrefs-b�l
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
    }

    // Crafted inventory bet�lt�se PlayerPrefs-b�l
    public void LoadCraftedInventory()
    {

        foreach (var item in itemDatabase.items)
        {
            string key = "crafted_" + item.itemName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                int quantity = PlayerPrefs.GetInt(key);
                craftedInventory.items[item] = quantity; // Friss�tj�k a mennyis�get
            }
        }
        Debug.Log("Crafted inventory loaded.");
        
    }

    // PlayerPrefs t�rl�se (mindk�t inventoryhoz)
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
                    Debug.LogWarning($"Nincs elegend� {ingredient.plantItem.englishName} a craftol�shoz.");
                    return false; 
                }
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                if (!craftedInventory.items.ContainsKey(ingredient.craftedItem) || craftedInventory.items[ingredient.craftedItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elegend� {ingredient.craftedItem.itemName} a craftol�shoz.");
                    return false; 
                }
            }
        }

        // Levonjuk az alapanyagokat
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) // N�v�ny alapanyag
            {
                inventory.RemoveItem(ingredient.plantItem, ingredient.quantity);
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                craftedInventory.RemoveItem(ingredient.craftedItem, ingredient.quantity);
            }
        }

        // Hozz�adjuk a kimeneti crafted itemet
        craftedInventory.AddItem(recipe.outputItem, recipe.outputQuantity);
        Debug.Log($"Craftol�s sikeres: {recipe.outputQuantity} db {recipe.outputItem.itemName} l�trehozva.");
        SaveInventory();
        SaveCraftedInventory();
        SaveInventoryToServer(); 
        SaveCraftedInventoryToServer();
        return true; // Craftol�s sikeres
    }
}



/*// T�rgyak hozz�ad�sa a lelt�rakhoz
InventoryManager.Instance.inventory.AddItem(plantDatabase.items[0], 5); // 5 n�v�ny hozz�ad�sa
InventoryManager.Instance.craftedInventory.AddItem(itemDatabase.items[0], 2); // 2 elk�sz�tett t�rgy hozz�ad�sa

// Lelt�rak ment�se
InventoryManager.Instance.SaveInventory();
InventoryManager.Instance.SaveCraftedInventory();

// Lelt�rak bet�lt�se
InventoryManager.Instance.LoadInventory();
InventoryManager.Instance.LoadCraftedInventory();

// Lelt�rak tartalm�nak ki�r�sa
InventoryManager.Instance.inventory.PrintInventory();
InventoryManager.Instance.craftedInventory.PrintInventory();*/