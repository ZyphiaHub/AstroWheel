using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    public Inventory inventory { get; private set; }
    public CraftedInventory craftedInventory { get; private set; } // Új CraftedInventory

    [Header("References")]
    public PlantDatabase plantDatabase; // Az Inspectorban állítsd be!
    public ItemDatabase itemDatabase; // Új ItemDatabase referenciája

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Maradjon életben jelenetváltáskor
        } else
        {
            Destroy(gameObject); // Ne legyen több példány
            return;
        }

        InitializeInventories();
    }

    // Inventoryk inicializálása és betöltése
    private void InitializeInventories()
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

        /*bool hasSavedCraftedData = false;
        
        foreach (var item in itemDatabase.items)
        {
            string key = "crafted_" + item.itemName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                hasSavedCraftedData = true;
                break;
            }
        }

        if (hasSavedCraftedData)
        {
            LoadCraftedInventory();
            
            // Mentjük a frissített CraftedInventory-t
            SaveCraftedInventory();
            Debug.Log("Crafted inventory initialized with new items.");

        } else
        {
            foreach (var item in itemDatabase.items)
            {
                craftedInventory.AddItem(item, 0);
            }
            SaveCraftedInventory();
            Debug.Log("New crafted inventory initialized with 0 quantities.");
        }*/
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
        Debug.Log("Plant inventory saved.");
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
        Debug.Log("Crafted inventory saved.");
        /*foreach (var entry in itemDatabase.items)
        {
            string key = "crafted_" + entry.itemName.Replace(" ", "");
            int quantity = craftedInventory.items.ContainsKey(entry) ? craftedInventory.items[entry] : 0;
            PlayerPrefs.SetInt(key, quantity);
        }

        PlayerPrefs.Save();
        Debug.Log("Crafted inventory saved.");*/
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
        /*foreach (var item in itemDatabase.items)
        {
            string key = "crafted_" + item.itemName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                int quantity = PlayerPrefs.GetInt(key);
                craftedInventory.AddItem(item, quantity);
            }
        }
        Debug.Log("Crafted inventory loaded.");*/
    }

    // PlayerPrefs törlése (mindkét inventoryhoz)
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
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