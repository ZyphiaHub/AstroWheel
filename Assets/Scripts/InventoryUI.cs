using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro n�vt�r import�l�sa

public class InventoryUI : MonoBehaviour {
    public PlantDatabase plantDatabase; // ItemDatabase referenci�ja
    public GameObject slotPrefab;     // Slot prefab referenci�ja
    public Transform panelParent;     

       

    private void Start()
    {
        // Friss�tj�k az UI-t az inventory tartalma alapj�n
        RefreshInventoryUI();
    }

    private void RefreshInventoryUI()
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
    }

    

    
}