using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro n�vt�r import�l�sa

public class InventoryUI : MonoBehaviour {
    [Header("Plant Inventory References")]
    public PlantDatabase plantDatabase; // ItemDatabase referenci�ja
    public GameObject plantSlotPrefab;     // Slot prefab referenci�ja
    public Transform plantPanelParent;

    [Header("Crafted Inventory References")]
    public ItemDatabase itemDatabase;   // ItemDatabase referenci�ja
    public GameObject craftedSlotPrefab; // Crafted slot prefab referenci�ja
    public Transform craftedPanelParent;

    private void Start()
    {
        // Friss�tj�k az UI-t az inventory tartalma alapj�n
        RefreshInventoryUI();
    }

    private void RefreshInventoryUI()
    {
        // T�r�lj�k a kor�bbi slotokat a plant inventoryb�l
        foreach (Transform child in plantPanelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat a plant inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.inventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.englishName, entry.Value.ToString(), plantSlotPrefab, plantPanelParent);
        }

        // T�r�lj�k a kor�bbi slotokat a crafted inventoryb�l
        foreach (Transform child in craftedPanelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat a crafted inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.craftedInventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.itemName, entry.Value.ToString(), craftedSlotPrefab, craftedPanelParent);
        }
    }

    // Egy �j slot l�trehoz�sa �s be�ll�t�sa
    private void CreateSlot(Sprite icon, string itemName, string quantity, GameObject slotPrefab, Transform parent)
    {
        // L�trehozunk egy �j slotot
        GameObject slot = Instantiate(slotPrefab, parent);

        // Be�ll�tjuk az ikont
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        } else
        {
            Debug.LogError("Icon object not found in slot prefab!");
        }

        // Be�ll�tjuk a t�rgy nev�t (TMP komponens haszn�lata)
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

        // Be�ll�tjuk a mennyis�get (TMP komponens haszn�lata)
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

    /*InventoryUI.Instance.RefreshInventoryUI(); // Friss�ti az UI-t*/

    /*private void RefreshInventoryUI()
    {
        // T�r�lj�k a kor�bbi slotokat
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat az inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.inventory.items)
        {
              
            // L�trehozunk egy �j slotot
            GameObject slot = Instantiate(slotPrefab, panelParent);

            // Be�ll�tjuk az ikont
            Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = entry.Key.icon;
            } else
            {
                Debug.LogError("Icon object not found in slot prefab!");
            }

            // Be�ll�tjuk a n�v�ny nev�t (TMP komponens haszn�lata)
            Transform nameTransform = slot.transform.Find("Label/Name");
            if (nameTransform != null)
            {
                TextMeshProUGUI nameLabel = nameTransform.GetComponent<TextMeshProUGUI>();
                if (nameLabel != null)
                {
                    nameLabel.text = entry.Key.englishName;
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

            // Be�ll�tjuk a mennyis�get (TMP komponens haszn�lata)
            Transform stackTransform = slot.transform.Find("Stack");
            if (stackTransform != null)
            {
                TextMeshProUGUI quantityText = stackTransform.GetComponent<TextMeshProUGUI>();
                if (quantityText != null)
                {
                    quantityText.text = entry.Value.ToString();
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
    }*/




}