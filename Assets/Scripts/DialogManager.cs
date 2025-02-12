using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public GameObject dialogPanel; // A dialógus panel (UI Panel)
    public TMP_Text dialogText;    // A dialógus szövege (TextMeshPro - Text)
    public Button closeButton;     // A bezárás gomb (Button)

    private void Start()
    {
        // Gomb esemény hozzárendelése
        closeButton.onClick.AddListener(CloseDialog);

        // Dialógus elrejtése indításkor
        dialogPanel.SetActive(false);
    }
    //teszt
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowDialog("Teszt üzenet: Ez egy dialógus panel!");
        }
    }
    // Dialógus megjelenítése
    public void ShowDialog(string message)
    {
        dialogText.text = message; // Szöveg beállítása
        dialogPanel.SetActive(true); // Panel megjelenítése
    }

    // Dialógus bezárása
    private void CloseDialog()
    {
        dialogPanel.SetActive(false); // Panel elrejtése
    }
}