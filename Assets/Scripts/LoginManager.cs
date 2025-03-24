using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using static RegisterManager;
using System;

public class LoginManager : MonoBehaviour {
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public Button registerButton;
    public Button quitButton;
    public TMP_Text errorMessageText;

    private bool isFetchPlayerDataCompleted = false;

    private void Start()
    {

        loginButton.onClick.AddListener(OnLoginButtonClicked);
        quitButton.onClick.AddListener(OnQuitToDesktopClicked);

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

        // Bejelentkezési kérés indítása
        // Bejelentkezési kérés indítása
        StartCoroutine(LoginAndFetchData(email, password));
        /*StartCoroutine(Login(email, password,
            response =>
            {
                Debug.Log("Login successful: " + response);


                int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
                Debug.Log("corutin last island: " + lastCompletedIsland);

                if (lastCompletedIsland >= 1)
                { // Ha az elsõ sziget teljesítve van
                    SceneManager.LoadScene("Main_Menu"); // Fõmenü betöltése
                } else
                {
                    SceneManager.LoadScene("Island_1"); // Elsõ sziget betöltése
                }
            },
            error =>
            {
                errorMessageText.text = "Invalid email or password!";
            }
        ));*/

    }

    private bool IsValidEmail(string email)
    {
        // Egyszerû regex az email cím ellenõrzésére
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    private void OnQuitToDesktopClicked()
    {
        Debug.Log("Kilépés a játékból...");
        Application.Quit();
    }

    // Bejelentkezési kérés
    
    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; 

        // Bejelentkezési adatok összeállítása
        var loginData = new LoginData
        {
            Email = email,
            Password = password
        };
        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Sending login data: " + jsonData);

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
                onError?.Invoke(webRequest.error);
                Debug.LogError("Login failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text); 
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Login successful: " + webRequest.downloadHandler.text);
                var response = JsonUtility.FromJson<LoginResponse>(webRequest.downloadHandler.text);
                PlayerPrefs.SetString("Password", password);
                PlayerPrefs.SetString("AuthToken", response.token);
                PlayerPrefs.Save();


                // Bejelentkezési kérés indítása
                yield return StartCoroutine(FetchPlayerData());


            }
        }
    }
    private IEnumerator LoginAndFetchData(string email, string password)
    {
        yield return StartCoroutine(Login(email, password,
            response =>
            {
                Debug.Log("Login successful: " + response);
            },
            error =>
            {
                errorMessageText.text = "Invalid email or password!";
            }
        ));

        // Várjuk meg, amíg a FetchPlayerData befejezõdik
        yield return new WaitUntil(() => isFetchPlayerDataCompleted);

        int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
        Debug.Log("corutin last island: " + lastCompletedIsland);

        if (lastCompletedIsland >= 1)
        { // Ha az elsõ sziget teljesítve van
            SceneManager.LoadScene("Main_Menu"); // Fõmenü betöltése
        } else
        {
            SceneManager.LoadScene("Island_1"); // Elsõ sziget betöltése
        }
    }
    // Játékos adatainak lekérése
    private IEnumerator FetchPlayerData()
    {
        string url = "https://astrowheelapi.onrender.com/api/players/me";
        string authToken = PlayerPrefs.GetString("AuthToken", ""); // Hitelesítési token lekérése

        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("No authentication token found.");
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken); // Token hozzáadása a fejléchez
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching player data: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                Debug.Log("Player data fetched successfully: " + webRequest.downloadHandler.text);

                try
                {
                    // Deszerializálás a PlayerData osztályba
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(webRequest.downloadHandler.text);

                    if (playerData != null)
                    {

                        // MySQL szerveren lévõ lastLogin
                        string serverLastLogin = playerData.lastLogin;
                        Debug.Log("serverlastloginkérés" + serverLastLogin);

                        // Helyi lastLogin lekérése
                        string localLastLogin = LocalDatabaseManager.Instance.GetLastLogin(playerData.playerId);
                        Debug.Log("locallastloginkérés" + localLastLogin);

                        Debug.Log($"Player data: ID = {playerData.playerId}, Username = {playerData.playerName}, " +
                            $"Email = {playerData.userId}, Password = {playerData.playerPassword}, CharacterId= {playerData.characterId}, Score = {playerData.totalScore}, " +
                            $"InventoryID = {playerData.inventoryId}, LastCompletedIsland = {playerData.islandId}");
                        // Adatok mentése PlayerPrefs-be
                        
                        if (string.Compare(serverLastLogin, localLastLogin, StringComparison.Ordinal) > 0) {
                            Debug.Log("compare ifben vagyok");
                        GameManager.Instance.SavePlayerId(playerData.playerId);
                        GameManager.Instance.SaveLastCompletedIsland(playerData.islandId);
                            
                        PlayerPrefs.SetString("PlayerUsername", playerData.playerName ?? string.Empty);
                        PlayerPrefs.SetString("PlayerEmail", playerData.userId ?? string.Empty);
                        PlayerPrefs.SetInt("PlayerScore", playerData.totalScore);
                        PlayerPrefs.SetInt("InventoryID", playerData.inventoryId);
                        //PlayerPrefs.SetInt("LastCompletedIsland", playerData.islandId); 
                        PlayerPrefs.Save();

                        Debug.Log("Player data saved to PlayerPrefs.");

                            playerData.playerPassword = PlayerPrefs.GetString("Password", "");

                        // Adatok mentése SQLite adatbázisba
                        LocalDatabaseManager.Instance.SavePlayerData(
                            playerData.playerId,
                            playerData.playerName ?? string.Empty,
                            playerData.userId ?? string.Empty,
                            playerData.playerPassword,
                            playerData.characterId,
                            playerData.totalScore,
                            playerData.inventoryId,
                            playerData.islandId,
                            playerData.lastLogin,
                            playerData.createdAt
                        );
                            }
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
        // Jelzés, hogy a FetchPlayerData befejezõdött
        isFetchPlayerDataCompleted = true;
    }


    [System.Serializable]
    public class LoginData
    {
        public string Email;
        public string Password;
    }

    [System.Serializable]
    public class PlayerData {
        public int playerId; // A szerver "playerId" mezõje
        public string playerName; 
        public string userId; // A szerver "userId" mezõje
        public string playerPassword;
        public int characterId;
        public int totalScore; // A szerver "totalScore" mezõje
        public int inventoryId; 
        public int islandId; // A szerver "islandId" mezõje (nullable)
        public string characterName; 
        public string lastLogin; // A szerver "lastLogin" mezõje
        public string createdAt; // A szerver "createdAt" mezõje
    }
}