using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using static RegisterManager;

public class LoginManager : MonoBehaviour {
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public Button registerButton;
    public Button quitButton;
    public TMP_Text errorMessageText;

    private void Start()
    {
        // Gomb esem�ny hozz�rendel�se
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        quitButton.onClick.AddListener(OnQuitToDesktopClicked);
        //StartCoroutine(FetchPlayerData()); //itt m�g az el�z� van benne
        PrintAllPlayerPrefs();
    }

    public void OnLoginButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Invalid email address!";
            return;
        }

        // Bejelentkez�si k�r�s ind�t�sa
        StartCoroutine(Login(email, password,
            response =>
            {
                Debug.Log("Login successful: " + response);

                // Sikeres bejelentkez�s eset�n bet�ltj�k a megfelel� jelenetet
                int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
                
                if (lastCompletedIsland >= 1)
                { // Ha az els� sziget teljes�tve van
                    SceneManager.LoadScene("Main_Menu"); // F�men� bet�lt�se
                } else
                {
                    SceneManager.LoadScene("Island_1"); // Els� sziget bet�lt�se
                }
            },
            error =>
            {
                errorMessageText.text = "Invalid email or password!";
            }
        ));
        
    }

    private bool IsValidEmail(string email)
    {
        // Egyszer� regex az email c�m ellen�rz�s�re
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    private void OnQuitToDesktopClicked()
    {
        Debug.Log("Kil�p�s a j�t�kb�l...");
        Application.Quit();
    }

    // Bejelentkez�si k�r�s
    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; 

        // Bejelentkez�si adatok �ssze�ll�t�sa
        var loginData = new LoginData
        {
            Email = email,
            Password = password
        };
        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Sending login data: " + jsonData);

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
                onError?.Invoke(webRequest.error);
                Debug.LogError("Login failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text); // Szerver v�lasz logol�sa
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Login successful: " + webRequest.downloadHandler.text);
                var response = JsonUtility.FromJson<LoginResponse>(webRequest.downloadHandler.text);
                PlayerPrefs.SetString("AuthToken", response.token);
                PlayerPrefs.Save();
                
                StartCoroutine(FetchPlayerData());
                //ide ment�s sqliteba

            }
        }
    }

    private void PrintAllPlayerPrefs()
    {
        Debug.Log("PlayerPrefs v�ltoz�k:");

        // Az �sszes PlayerPrefs kulcs lek�r�se
        string[] keys = PlayerPrefs.GetString("PlayerPrefsKeys", "").Split(',');

        foreach (string key in keys)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (PlayerPrefs.HasKey(key))
                {
                    string value = PlayerPrefs.GetString(key, "");
                    Debug.Log($"{key}: {value}");
                }
            }
        }
    }
    // J�t�kos adatainak lek�r�se
    private IEnumerator FetchPlayerData()
    {
        string url = "https://astrowheelapi.onrender.com/api/players/me";
        string authToken = PlayerPrefs.GetString("AuthToken", ""); // Hiteles�t�si token lek�r�se

        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("No authentication token found.");
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken); // Token hozz�ad�sa a fejl�chez
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching player data: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                Debug.Log("Player data fetched successfully: " + webRequest.downloadHandler.text);

                try
                {
                    // Deszerializ�l�s a PlayerData oszt�lyba
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(webRequest.downloadHandler.text);

                    if (playerData != null)
                    {
                        // Adatok ment�se PlayerPrefs-be
                        PlayerPrefs.SetInt("PlayerID", playerData.id);
                        PlayerPrefs.SetString("PlayerUsername", playerData.username);
                        PlayerPrefs.SetString("PlayerEmail", playerData.email);
                        PlayerPrefs.SetInt("PlayerScore", playerData.score);
                        PlayerPrefs.SetInt("LastCompletedIsland", playerData.lastCompletedIsland);
                        PlayerPrefs.Save();

                        Debug.Log("Player data saved to PlayerPrefs.");
                    } else
                    {
                        Debug.LogError("Failed to deserialize player data.");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error deserializing player data: " + ex.Message);
                }
            
        }
        }
    }


    [System.Serializable]
    public class LoginData
    {
        public string Email;
        public string Password;
    }

    [System.Serializable]
    public class PlayerData {
        public int id;
        public string username;
        public string email;
        public int score;
        public int lastCompletedIsland;
    }
}