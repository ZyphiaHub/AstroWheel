using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour {
    public static RegisterManager Instance; // Singleton p�ld�ny

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public Button selectCharacterButton;
    public TMP_Text errorMessageText;
    public GameObject characterSelectionPanel; 
    public Transform characterImageContainer; 

    [SerializeField] private Sprite[] characterSprites; 
    // Property a characterSprites t�mb el�r�s�hez
    public Sprite[] CharacterSprites => characterSprites;
    private int selectedCharacterIndex = 0; // Kiv�lasztott karakter indexe

    private void Start()
    {
        // Gomb esem�ny hozz�rendel�se
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        

        // Karakterk�pek megjelen�t�se a panelen
        LoadCharacterImages();
    }
    private void Awake()
    {
        // Singleton minta implement�ci�ja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ne t�rl�dj�n a scene v�lt�skor
        } else
        {
            Destroy(gameObject); // Ha m�r van p�ld�ny, t�r�ld ezt
        }
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
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex); // Ment�s a PlayerPrefs-be
        PlayerPrefs.Save(); // Azonnali ment�s
        Debug.Log("Selected character index: " + index);
       // characterSelectionPanel.SetActive(false); // Panel bez�r�sa
    }

    
}