using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    // Az inventory p�ld�ny
    public Inventory inventory { get; private set; }

    private void Awake()
    {
        // Singleton minta implement�ci�ja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Maradjon �letben jelenetv�lt�skor
        } else
        {
            Destroy(gameObject); // T�bb p�ld�ny ne legyen
        }

        // Inventory l�trehoz�sa
        inventory = new Inventory();
    }
}