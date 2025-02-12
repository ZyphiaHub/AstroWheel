using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public GameObject dialogPanel; // A dial�gus panel (UI Panel)
    public TMP_Text dialogText;    // A dial�gus sz�vege (TextMeshPro - Text)
    public Button closeButton;     // A bez�r�s gomb (Button)

    private void Start()
    {
        // Gomb esem�ny hozz�rendel�se
        closeButton.onClick.AddListener(CloseDialog);

        // Dial�gus elrejt�se ind�t�skor
        dialogPanel.SetActive(false);
    }
    //teszt
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowDialog("Teszt �zenet: Ez egy dial�gus panel!");
        }
    }
    // Dial�gus megjelen�t�se
    public void ShowDialog(string message)
    {
        dialogText.text = message; // Sz�veg be�ll�t�sa
        dialogPanel.SetActive(true); // Panel megjelen�t�se
    }

    // Dial�gus bez�r�sa
    private void CloseDialog()
    {
        dialogPanel.SetActive(false); // Panel elrejt�se
    }
}