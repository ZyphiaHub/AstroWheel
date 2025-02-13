using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {
    public GameObject dialogPanel; // A dialógus panel (UI Panel)
    public TMP_Text dialogText;    // A dialógus szövege (TextMeshPro - Text)
    public Button nextButton;      // A "Tovább" gomb (Button)
    public Button closeButton;     // A "Bezárás" gomb (Button)

    private List<string> dialogLines; // A dialógus sorok listája
    private int currentLineIndex;     // Az aktuális sor indexe

    private void Start()
    {
        // Gomb események hozzárendelése
        nextButton.onClick.AddListener(ShowNextLine);
        closeButton.onClick.AddListener(CloseDialog);

        // Dialógus elrejtése indításkor
        if (GameManager.Instance.LoadLastCompletedIsland() == 0)
        {
            dialogPanel.SetActive(true);
        }
    }

    // Dialógus megjelenítése több sorral
    public void ShowDialog(List<string> lines)
    {
        dialogLines = lines; // Sorok beállítása
        currentLineIndex = 0; // Az elsõ sor kezdõdik

        // Az elsõ sor megjelenítése
        dialogText.text = dialogLines[currentLineIndex];
        dialogPanel.SetActive(true);

        // "Tovább" gomb engedélyezése, ha több sor van
        nextButton.gameObject.SetActive(dialogLines.Count > 1);
        closeButton.gameObject.SetActive(false); // "Bezárás" gomb elrejtése
    }

    // Következõ sor megjelenítése
    private void ShowNextLine()
    {
        currentLineIndex++;

        // Ha van még sor, akkor megjelenítjük
        if (currentLineIndex < dialogLines.Count)
        {
            dialogText.text = dialogLines[currentLineIndex];
        } else
        {
            // Ha nincs több sor, akkor bezárjuk a dialógust
            CloseDialog();
        }

        // "Tovább" gomb elrejtése, ha ez az utolsó sor
        if (currentLineIndex == dialogLines.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true); // "Bezárás" gomb megjelenítése
        }
    }

    // Dialógus bezárása
    private void CloseDialog()
    {
        dialogPanel.SetActive(false); // Panel elrejtése
    }
}