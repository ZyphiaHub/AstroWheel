using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour {
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public Button selectCharacterButton;
    public TMP_Text errorMessageText;
    public GameObject characterSelectionPanel; // Karakterk�p v�laszt� panel
    public Transform characterImageContainer; // Karakterk�pek t�rol�ja

    [SerializeField] private Sprite[] characterSprites; // Karakterk�pek t�mbje
    private int selectedCharacterIndex = 0; // Kiv�lasztott karakter indexe

    private void Start()
    {
        // Gomb esem�ny hozz�rendel�se
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        

        // Karakterk�pek megjelen�t�se a panelen
        LoadCharacterImages();
    }

    public void OnRegisterButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Invalid email address!";
            return;
        }

        if (password.Length < 6)
        {
            errorMessageText.text = "The password should be at least 6 character long!";
            return;
        }

        // Adatok ment�se a PlayerPrefs-be
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex); // Karakterk�p index�nek ment�se
        PlayerPrefs.Save();

        Debug.Log("Sikeres regisztr�ci�!");
        errorMessageText.text = "Player registered!";
    }

    private bool IsValidEmail(string email)
    {
        // Egyszer� regex az email c�m ellen�rz�s�re
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
    public void OnSelectCharacterButtonClicked()
    {
        // Panel aktiv�l�sa vagy deaktiv�l�sa
        characterSelectionPanel.SetActive(!characterSelectionPanel.activeSelf);
    }
    private void LoadCharacterImages()
    {
        // T�r�lj�k a kor�bbi k�peket (ha vannak)
        foreach (Transform child in characterImageContainer)
        {
            Destroy(child.gameObject);
        }

        // Karakterk�pek megjelen�t�se a panelen
        for (int i = 0; i < characterSprites.Length; i++)
        {
            GameObject imageObject = new GameObject("CharacterImage");
            imageObject.transform.SetParent(characterImageContainer, false);

            Image image = imageObject.AddComponent<Image>();
            image.sprite = characterSprites[i];

            // Gomb hozz�ad�sa a k�phez
            Button button = imageObject.AddComponent<Button>();
            int index = i; // Lok�lis v�ltoz� a ciklusban
            button.onClick.AddListener(() => OnCharacterSelected(index));
        }
        Debug.Log("Loaded " + characterSprites.Length + " character images.");
    }
    private void OnCharacterSelected(int index)
    {
        selectedCharacterIndex = index;
        Debug.Log("Selected character index: " + index);
       // characterSelectionPanel.SetActive(false); // Panel bez�r�sa
    }

    
}