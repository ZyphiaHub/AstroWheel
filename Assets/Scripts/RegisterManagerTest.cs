using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

[System.Serializable]
public class RegistrationData {
    public string UserName;
    public string Email;
    public string Password;
    public string PlayerName;
    public int CharacterId;
}

public class RegisterManager : MonoBehaviour {
    public static RegisterManager Instance; // Singleton példány

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField nameInputField;
    public Button registerButton;
    public Button selectCharacterButton;
    public Button saveCharNameButton;
    public Button cancelCharacterButton;
    public Button cancelNameButton;
    public TMP_Text errorMessageText;
    public GameObject characterSelectionPanel;
    public GameObject registerNamePanel;
    public Transform characterImageContainer;

    [SerializeField] private Sprite[] characterSprites;
    public Sprite[] CharacterSprites => characterSprites;
    private int selectedCharacterIndex = 0; // Kiválasztott karakter indexe

    private void Start()
    {
        // Gomb események
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        saveCharNameButton.onClick.AddListener(OnSaveCharacterNameButtonClicked);

        cancelCharacterButton.onClick.AddListener(CloseCharacterCreationPanels);
        cancelNameButton.onClick.AddListener(CloseCharacterCreationPanels);

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

        // Jelszó ellenõrzése
        if (!IsPasswordValid(password, out string passwordErrorMessage))
        {
            errorMessageText.text = passwordErrorMessage;
            return;
        }

        // Adatok mentése a PlayerPrefs-be
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        GameManager.Instance.SaveLastCompletedIsland(0);
        GameManager.Instance.SaveTotalScore(0);
        PlayerPrefs.Save();

        Debug.Log("Sikeres regisztráció!");
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
        if (characterSelectionPanel.activeSelf)
        {
            PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
            PlayerPrefs.Save(); // Azonnali mentés
            Debug.Log("Selected character index saved: " + selectedCharacterIndex);
        }

        characterSelectionPanel.SetActive(!characterSelectionPanel.activeSelf);
        registerNamePanel.SetActive(true);
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
        Debug.Log("Selected character index: " + index);
    }

    public void OnSaveCharacterNameButtonClicked()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            errorMessageText.text = "Please enter a valid name!";
            return;
        }

        // Az összes regisztrációs adatot összegyûjtjük
        string email = PlayerPrefs.GetString("RegisteredEmail", "");
        string password = PlayerPrefs.GetString("RegisteredPassword", "");
        int characterIndex = selectedCharacterIndex;

        // Adatok elküldése a szerverre
        StartCoroutine(RegisterPlayer(playerName, email, password, characterIndex));
    }

    private IEnumerator RegisterPlayer(string playerName, string email, string password, int characterIndex)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/register"; // Regisztrációs URL

        // Regisztrációs adatok összeállítása
        var registrationData = new RegistrationData
        {
            UserName = email, // A UserName mezõt is kitöltjük az e-mail címmel
            Email = email,
            Password = password,
            PlayerName = playerName,
            CharacterId = characterIndex
        };
        string jsonData = JsonUtility.ToJson(registrationData);

        Debug.Log("Sending registration data: " + jsonData); // JSON adatok logolása

        // POST kérés elküldése
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Registration failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text); // Szerver válasz logolása
                errorMessageText.text = "Registration failed: " + webRequest.downloadHandler.text;
            } else
            {
                Debug.Log("Registration successful: " + webRequest.downloadHandler.text);
                errorMessageText.text = "Registration successful!";

                // Név regisztrációs panel bezárása
                registerNamePanel.SetActive(false);
                SceneManager.LoadScene("Island_1");
            }
        }
    }

    public void CloseCharacterCreationPanels()
    {
        Debug.Log("Character creation cancelled.");
        characterSelectionPanel.SetActive(false);
        registerNamePanel.SetActive(false);

        // Ideiglenesen beírt adatok törlése, hogy ne léptessen be a fõmenübe!
        PlayerPrefs.DeleteKey("RegisteredEmail");
        PlayerPrefs.DeleteKey("RegisteredPassword");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("SelectedCharacterIndex");
        PlayerPrefs.Save();

        // Visszaléptetés a login képernyõre (ha szükséges)
        SceneManager.LoadScene("Login");
    }

    private bool IsPasswordValid(string password, out string errorMessage)
    {
        errorMessage = "";

        if (password.Length < 6)
        {
            errorMessage = "The password should be at least 6 characters long!";
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            errorMessage = "The password must contain at least one digit!";
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            errorMessage = "The password must contain at least one lowercase letter!";
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            errorMessage = "The password must contain at least one uppercase letter!";
            return false;
        }

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            errorMessage = "The password must contain at least one special character!";
            return false;
        }

        if (password.Distinct().Count() < 1)
        {
            errorMessage = "The password must use at least 1 different characters!";
            return false;
        }

        return true;
    }
}