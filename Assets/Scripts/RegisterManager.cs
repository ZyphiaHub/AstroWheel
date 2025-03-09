using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour {
    public static RegisterManager Instance; // Singleton példány

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public Button selectCharacterButton;
    public TMP_Text errorMessageText;
    public GameObject characterSelectionPanel; 
    public Transform characterImageContainer; 

    [SerializeField] private Sprite[] characterSprites; 
    // Property a characterSprites tömb eléréséhez
    public Sprite[] CharacterSprites => characterSprites;
    private int selectedCharacterIndex = 0; // Kiválasztott karakter indexe

    private void Start()
    {
        // Gomb esemény hozzárendelése
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        

        // Karakterképek megjelenítése a panelen
        LoadCharacterImages();
    }
    private void Awake()
    {
        // Singleton minta implementációja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ne törlõdjön a scene váltáskor
        } else
        {
            Destroy(gameObject); // Ha már van példány, töröld ezt
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

        // Adatok mentése a PlayerPrefs-be
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex); // Karakterkép indexének mentése
        PlayerPrefs.Save();

        Debug.Log("Sikeres regisztráció!");
        errorMessageText.text = "Player registered!";
    }

    private bool IsValidEmail(string email)
    {
        // Egyszerû regex az email cím ellenõrzésére
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
    public void OnSelectCharacterButtonClicked()
    {
        // Panel aktiválása vagy deaktiválása
        characterSelectionPanel.SetActive(!characterSelectionPanel.activeSelf);
    }
    private void LoadCharacterImages()
    {
        // Töröljük a korábbi képeket (ha vannak)
        foreach (Transform child in characterImageContainer)
        {
            Destroy(child.gameObject);
        }

        // Karakterképek megjelenítése a panelen
        for (int i = 0; i < characterSprites.Length; i++)
        {
            GameObject imageObject = new GameObject("CharacterImage");
            imageObject.transform.SetParent(characterImageContainer, false);

            Image image = imageObject.AddComponent<Image>();
            image.sprite = characterSprites[i];

            // Gomb hozzáadása a képhez
            Button button = imageObject.AddComponent<Button>();
            int index = i; // Lokális változó a ciklusban
            button.onClick.AddListener(() => OnCharacterSelected(index));
        }
        Debug.Log("Loaded " + characterSprites.Length + " character images.");
    }
    private void OnCharacterSelected(int index)
    {
        selectedCharacterIndex = index;
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex); // Mentés a PlayerPrefs-be
        PlayerPrefs.Save(); // Azonnali mentés
        Debug.Log("Selected character index: " + index);
       // characterSelectionPanel.SetActive(false); // Panel bezárása
    }

    
}