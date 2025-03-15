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
    public static RegisterManager Instance; // Singleton p�ld�ny

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
    private int selectedCharacterIndex = 0; // Kiv�lasztott karakter indexe

    private void Start()
    {
        // Gomb esem�nyek
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        saveCharNameButton.onClick.AddListener(OnSaveCharacterNameButtonClicked);

        cancelCharacterButton.onClick.AddListener(CloseCharacterCreationPanels);
        cancelNameButton.onClick.AddListener(CloseCharacterCreationPanels);

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

        // Jelsz� ellen�rz�se
        if (!IsPasswordValid(password, out string passwordErrorMessage))
        {
            errorMessageText.text = passwordErrorMessage;
            return;
        }

        // Adatok ment�se a PlayerPrefs-be
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        GameManager.Instance.SaveLastCompletedIsland(0);
        GameManager.Instance.SaveTotalScore(0);
        PlayerPrefs.Save();

        Debug.Log("Sikeres regisztr�ci�!");
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
        if (characterSelectionPanel.activeSelf)
        {
            PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
            PlayerPrefs.Save(); // Azonnali ment�s
            Debug.Log("Selected character index saved: " + selectedCharacterIndex);
        }

        characterSelectionPanel.SetActive(!characterSelectionPanel.activeSelf);
        registerNamePanel.SetActive(true);
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
    }

    public void OnSaveCharacterNameButtonClicked()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            errorMessageText.text = "Please enter a valid name!";
            return;
        }

        // Az �sszes regisztr�ci�s adatot �sszegy�jtj�k
        string email = PlayerPrefs.GetString("RegisteredEmail", "");
        string password = PlayerPrefs.GetString("RegisteredPassword", "");
        int characterIndex = selectedCharacterIndex;

        // Adatok elk�ld�se a szerverre
        StartCoroutine(RegisterPlayer(playerName, email, password, characterIndex));
    }

    private IEnumerator RegisterPlayer(string playerName, string email, string password, int characterIndex)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/register"; // Regisztr�ci�s URL

        // Regisztr�ci�s adatok �ssze�ll�t�sa
        var registrationData = new RegistrationData
        {
            UserName = email, // A UserName mez�t is kit�ltj�k az e-mail c�mmel
            Email = email,
            Password = password,
            PlayerName = playerName,
            CharacterId = characterIndex
        };
        string jsonData = JsonUtility.ToJson(registrationData);

        Debug.Log("Sending registration data: " + jsonData); // JSON adatok logol�sa

        // POST k�r�s elk�ld�se
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
                Debug.LogError("Server response: " + webRequest.downloadHandler.text); // Szerver v�lasz logol�sa
                errorMessageText.text = "Registration failed: " + webRequest.downloadHandler.text;
            } else
            {
                Debug.Log("Registration successful: " + webRequest.downloadHandler.text);
                errorMessageText.text = "Registration successful!";

                // N�v regisztr�ci�s panel bez�r�sa
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

        // Ideiglenesen be�rt adatok t�rl�se, hogy ne l�ptessen be a f�men�be!
        PlayerPrefs.DeleteKey("RegisteredEmail");
        PlayerPrefs.DeleteKey("RegisteredPassword");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("SelectedCharacterIndex");
        PlayerPrefs.Save();

        // Visszal�ptet�s a login k�perny�re (ha sz�ks�ges)
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