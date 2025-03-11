using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    // Az inventory példány
    public Inventory inventory { get; private set; }

    private void Awake()
    {
        // Singleton minta implementációja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Maradjon életben jelenetváltáskor
        } else
        {
            Destroy(gameObject); // Több példány ne legyen
        }

        // Inventory létrehozása
        inventory = new Inventory();
    }
}