using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {
    public GameObject dialogPanel; // A dial�gus panel (UI Panel)
    public TMP_Text dialogText;    // A dial�gus sz�vege (TextMeshPro - Text)
    public Button nextButton;      // A "Tov�bb" gomb (Button)
    public Button closeButton;     // A "Bez�r�s" gomb (Button)

    private List<string> dialogLines; // A dial�gus sorok list�ja
    private int currentLineIndex;     // Az aktu�lis sor indexe

    private void Start()
    {
        // Gomb esem�nyek hozz�rendel�se
        nextButton.onClick.AddListener(ShowNextLine);
        closeButton.onClick.AddListener(CloseDialog);

        // Dial�gus elrejt�se ind�t�skor
        if (GameManager.Instance.LoadLastCompletedIsland() == 0)
        {
            dialogPanel.SetActive(true);
        }
    }

    // Dial�gus megjelen�t�se t�bb sorral
    public void ShowDialog(List<string> lines)
    {
        dialogLines = lines; // Sorok be�ll�t�sa
        currentLineIndex = 0; // Az els� sor kezd�dik

        // Az els� sor megjelen�t�se
        dialogText.text = dialogLines[currentLineIndex];
        dialogPanel.SetActive(true);

        // "Tov�bb" gomb enged�lyez�se, ha t�bb sor van
        nextButton.gameObject.SetActive(dialogLines.Count > 1);
        closeButton.gameObject.SetActive(false); // "Bez�r�s" gomb elrejt�se
    }

    // K�vetkez� sor megjelen�t�se
    private void ShowNextLine()
    {
        currentLineIndex++;

        // Ha van m�g sor, akkor megjelen�tj�k
        if (currentLineIndex < dialogLines.Count)
        {
            dialogText.text = dialogLines[currentLineIndex];
        } else
        {
            // Ha nincs t�bb sor, akkor bez�rjuk a dial�gust
            CloseDialog();
        }

        // "Tov�bb" gomb elrejt�se, ha ez az utols� sor
        if (currentLineIndex == dialogLines.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true); // "Bez�r�s" gomb megjelen�t�se
        }
    }

    // Dial�gus bez�r�sa
    private void CloseDialog()
    {
        dialogPanel.SetActive(false); // Panel elrejt�se
    }
}