using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    public Inventory inventory { get; private set; }

    [Header("References")]
    public PlantDatabase plantDatabase; // Az Inspectorban �ll�tsd be!

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

        InitializeInventory();
    }

    //  Inventory inicializ�l�sa �s bet�lt�se
    private void InitializeInventory()
    {
        inventory = new Inventory();

        bool hasSavedData = false;

        // Ellen�rizz�k, van-e mentett adat
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
            // Ha nincs mentett adat, inicializ�ljuk 0 �rt�kekkel �s mentj�k
            foreach (var item in plantDatabase.items)
            {
                inventory.AddItem(item, 0);
            }
            SaveInventory();
            Debug.Log("New inventory initialized with 0 quantities.");
        }
    }

    //  Inventory ment�se PlayerPrefs-be
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

    //  Inventory bet�lt�se PlayerPrefs-b�l
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

    //  PlayerPrefs t�rl�se 
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
    }
}
