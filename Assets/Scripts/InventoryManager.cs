using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    public Inventory inventory { get; private set; }

    [Header("References")]
    public PlantDatabase plantDatabase; // Az Inspectorban állítsd be!

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

        InitializeInventory();
    }

    //  Inventory inicializálása és betöltése
    private void InitializeInventory()
    {
        inventory = new Inventory();

        bool hasSavedData = false;

        // Ellenõrizzük, van-e mentett adat
        foreach (var item in plantDatabase.items)
        {
            string key = "plant_" + item.englishName.Replace(" ", "");
            if (PlayerPrefs.HasKey(key))
            {
                hasSavedData = true;
                break;
            }
        }

        if (hasSavedData)
        {
            LoadInventory();
        } else
        {
            // Ha nincs mentett adat, inicializáljuk 0 értékekkel és mentjük
            foreach (var item in plantDatabase.items)
            {
                inventory.AddItem(item, 0);
            }
            SaveInventory();
            Debug.Log("New inventory initialized with 0 quantities.");
        }
    }

    //  Inventory mentése PlayerPrefs-be
    public void SaveInventory()
    {
        foreach (var entry in plantDatabase.items)
        {
            string key = "plant_" + entry.englishName.Replace(" ", "");
            int quantity = inventory.items.ContainsKey(entry) ? inventory.items[entry] : 0;
            PlayerPrefs.SetInt(key, quantity);
        }

        PlayerPrefs.Save();
        Debug.Log("Inventory saved.");
    }

    //  Inventory betöltése PlayerPrefs-bõl
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
        Debug.Log("Inventory loaded.");
    }

    //  PlayerPrefs törlése 
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
    }
}
