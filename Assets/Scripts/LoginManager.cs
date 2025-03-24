using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using static RegisterManager;
using System;
using static LocalDatabaseManager;
using System.Collections.Generic;
using static LoginManager;

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

        
        // Bejelentkez�si k�r�s ind�t�sa
        StartCoroutine(LoginAndFetchData(email, password));
        

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

    
    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; 

        // Bejelentkez�si adatok �ssze�ll�t�sa
        var loginData = new LoginData
        {
            Email = email,
            Password = password
        };
        PlayerPrefs.SetString("LoginEmail", email);
        PlayerPrefs.Save();
        //var proba = PlayerPrefs.GetString("LoginEmail", "");
        //Debug.Log("login email erteke: " + proba);
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
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);

                
                StartCoroutine(FetchPlayerSQLite(email, password));
                int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
                //Debug.Log("lite  last island: " + lastCompletedIsland);



                if (lastCompletedIsland >= 1)
                { // Ha az els� sziget teljes�tve van
                    SceneManager.LoadScene("Main_Menu"); // F�men� bet�lt�se
                } else
                {
                    SceneManager.LoadScene("Island_1"); // Els� sziget bet�lt�se
                }

            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Login successful: " + webRequest.downloadHandler.text);
                var response = JsonUtility.FromJson<LoginResponse>(webRequest.downloadHandler.text);
                PlayerPrefs.SetString("Password", password);
                PlayerPrefs.SetString("AuthToken", response.token);
                PlayerPrefs.Save();


                // Bejelentkez�si k�r�s ind�t�sa
                yield return StartCoroutine(FetchPlayerData());


            }
        }
    }
    private IEnumerator FetchPlayerSQLite(string email, string password)
    {
        Debug.Log("Fetching player data from SQLite...");
        Debug.Log("email �s pass liteban" + email + password);
        PlayerTbl playerData = LocalDatabaseManager.Instance.LoadPlayerDataByEmailAndPassword(email, password);

        if (playerData != null)
        {
            Debug.Log($"Local Player Data: ID = {playerData.playerId}, Username = {playerData.playerName}, " +
                $"Email = {playerData.userId}, CharacterId = {playerData.characterId}, Score = {playerData.totalScore}, " +
                $"InventoryID = {playerData.inventoryId}, LastCompletedIsland = {playerData.islandId}");

            // Adatok bet�lt�se a GameManager-be
            GameManager.Instance.SavePlayerId(playerData.playerId);
            GameManager.Instance.SaveLastCompletedIsland(playerData.islandId);

            PlayerPrefs.SetString("PlayerUsername", playerData.playerName ?? string.Empty);
            PlayerPrefs.Save();
            PlayerPrefs.SetString("PlayerEmail", playerData.userId ?? string.Empty);
            PlayerPrefs.Save();
            PlayerPrefs.SetInt("PlayerScore", playerData.totalScore);
            PlayerPrefs.Save();
            PlayerPrefs.SetInt("InventoryID", playerData.inventoryId);
            PlayerPrefs.Save();

            Debug.Log("Local player data loaded successfully.");
        } else
        {
            Debug.LogError("No player data found in SQLite for the given email and password.");
        }

        //isFetchPlayerDataCompleted = true;
        yield break;
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

        // V�rjuk meg, am�g a FetchPlayerData befejez�dik
        yield return new WaitUntil(() => isFetchPlayerDataCompleted);

        int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
        Debug.Log("corutin last island: " + lastCompletedIsland);

        if (lastCompletedIsland >= 1)
        { // Ha az els� sziget teljes�tve van
            SceneManager.LoadScene("Main_Menu"); // F�men� bet�lt�se
        } else
        {
            SceneManager.LoadScene("Island_1"); // Els� sziget bet�lt�se
        }
    }
    // J�t�kos adatainak lek�r�se
    private IEnumerator FetchPlayerData()
    {
        string url = "https://astrowheelapi.onrender.com/api/players/me";
        string authToken = PlayerPrefs.GetString("AuthToken", ""); 

        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("No authentication token found.");
            isFetchPlayerDataCompleted = true;
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken); 
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching player data: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
                isFetchPlayerDataCompleted = true;
                yield break;
            } else
            {
                Debug.Log("Player data fetched successfully: " + webRequest.downloadHandler.text);
                PlayerDataFetch playerData = null;
                try
                {
                    playerData = JsonUtility.FromJson<PlayerDataFetch>(webRequest.downloadHandler.text);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error deserializing player data: " + ex.Message);
                    isFetchPlayerDataCompleted = true;
                    yield break;
                }
                if (playerData == null)
                {
                    Debug.LogError("Failed to deserialize player data.");
                    isFetchPlayerDataCompleted = true;
                    yield break;
                }


                // MySQL szerveren l�v� lastLogin
                string serverLastLogin = playerData.lastLogin;
                Debug.Log("serverlastlogink�r�s" + serverLastLogin);

                // Helyi lastLogin lek�r�se
                string localLastLogin = LocalDatabaseManager.Instance.GetLastLogin(playerData.playerId);
                Debug.Log("locallastlogink�r�s" + localLastLogin);

                // Adatok ment�se PlayerPrefs-be
                        
                if (string.Compare(serverLastLogin, localLastLogin, StringComparison.Ordinal) > 0) {
                   GameManager.Instance.SavePlayerId(playerData.playerId);
                   GameManager.Instance.SaveLastCompletedIsland(playerData.islandId);
                            
                   PlayerPrefs.SetString("PlayerUsername", playerData.playerName ?? string.Empty);
                   PlayerPrefs.Save();
                        
                   PlayerPrefs.SetInt("PlayerScore", playerData.totalScore);
                   PlayerPrefs.Save();
                   PlayerPrefs.SetInt("InventoryID", playerData.inventoryId);
                   PlayerPrefs.Save();

                   Debug.Log("Player data saved to PlayerPrefs.");

                   playerData.playerPassword = PlayerPrefs.GetString("Password", "");
                   playerData.playerEmail = PlayerPrefs.GetString("LoginEmail", "");

                   // Adatok ment�se SQLite adatb�zisba
                   LocalDatabaseManager.Instance.SavePlayerData(
                      playerData.playerId,
                      playerData.playerName ?? string.Empty,
                    playerData.userId ?? string.Empty,
                    playerData.playerEmail,
                    playerData.playerPassword,
                    playerData.characterId,
                    playerData.totalScore,
                     playerData.inventoryId,
                     playerData.islandId,
                    playerData.lastLogin,
                     playerData.createdAt       );

                    if (playerData.materials != null && playerData.materials.Count > 0)
                    {
                        InventoryManager.Instance.LoadFetchedMatToPlantDatabase(playerData.materials.ToArray());
                        InventoryManager.Instance.SaveInventory();
                        InventoryManager.Instance.SaveCraftedInventory();

                    }
                    } else
                    {
                        Debug.LogError("Failed to deserialize player data.");
                    }
                

            }
        }
        // Jelz�s, hogy a FetchPlayerData befejez�d�tt
        isFetchPlayerDataCompleted = true;
        
    }


    [System.Serializable]
    public class LoginData
    {
        public string Email;
        public string Password;
    }

    [System.Serializable]
    public class PlayerDataFetch {
        public int playerId; // A szerver "playerId" mez�je
        public string playerName; 
        public string userId; // A szerver "userId" mez�je
        public string playerPassword;
        public string playerEmail;
        public int characterId;
        public int totalScore; // A szerver "totalScore" mez�je
        public int inventoryId; 
        public int islandId; // A szerver "islandId" mez�je (nullable)
        public string characterName; 
        public string lastLogin; // A szerver "lastLogin" mez�je
        public string createdAt; // A szerver "createdAt" mez�je
        public List<MaterialDataFetch> materials;
    }

    [System.Serializable]
    public class MaterialDataFetch {
        public int materialId;
        public string witchName;
        public string englishName;
        public string latinName;
        public int quantity;
    }
}