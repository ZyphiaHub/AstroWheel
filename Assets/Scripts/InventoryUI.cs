using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro névtér importálása

public class InventoryUI : MonoBehaviour {
    public PlantDatabase itemDatabase; // ItemDatabase referenciája
    public GameObject slotPrefab;     // Slot prefab referenciája
    public Transform panelParent;     // A Panel, amelybe a slotok kerülnek

    private Inventory inventory;     // A játékos inventoryja

    void Start()
    {
        // Példa: Betöltjük az inventoryt (ez lehet más forrásból is)
        inventory = new Inventory();
        inventory.Initialize(itemDatabase);

        // Inventory tartalmának megjelenítése
        DisplayInventory();
    }

    private void DisplayInventory()
    {
        // Töröljük a korábbi slotokat
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }

        // Létrehozzuk a slotokat az inventory tartalma alapján
        foreach (var itemEntry in inventory.items)
        {
            // Létrehozunk egy új slotot
            GameObject slot = Instantiate(slotPrefab, panelParent);

            // Beállítjuk az ikont
            Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = itemEntry.Key.icon;
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
                    nameLabel.text = itemEntry.Key.englishName;
                } else
                {
                    Debug.LogError("TextMeshProUGUI component not found on Name object!");
                    Debug.Log("Name object components:");
                    foreach (var component in nameTransform.GetComponents<Component>())
                    {
                        Debug.Log(" - " + component.GetType().Name);
                    }
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
                    quantityText.text = itemEntry.Value.ToString();
                } else
                {
                    Debug.LogError("TextMeshProUGUI component not found on Stack object!");
                    Debug.Log("Stack object components:");
                    foreach (var component in stackTransform.GetComponents<Component>())
                    {
                        Debug.Log(" - " + component.GetType().Name);
                    }
                }
            } else
            {
                Debug.LogError("Stack object not found in slot prefab!");
            }

            
        }
    }
}