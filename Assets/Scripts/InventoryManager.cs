using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    public Inventory inventory { get; private set; }
    public CraftedInventory craftedInventory { get; private set; } 

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
            DontDestroyOnLoad(gameObject); // Maradjon �letben jelenetv�lt�skor
        } else
        {
            Destroy(gameObject); // Ne legyen t�bb p�ld�ny
            return;
        }

        InitializeInventories();
    }

    // Inventoryk inicializ�l�sa �s bet�lt�se
    private void InitializeInventories()
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
        // Ellen�rizz�k, hogy van-e elegend� alapanyag
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) 
            {
                if (!inventory.items.ContainsKey(ingredient.plantItem) || inventory.items[ingredient.plantItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elegend� {ingredient.plantItem.englishName} a craftol�shoz.");
                    return false; // Nincs elegend� alapanyag
                }
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                if (!craftedInventory.items.ContainsKey(ingredient.craftedItem) || craftedInventory.items[ingredient.craftedItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elegend� {ingredient.craftedItem.itemName} a craftol�shoz.");
                    return false; // Nincs elegend� alapanyag
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