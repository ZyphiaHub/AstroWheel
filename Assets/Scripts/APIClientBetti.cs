using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour {
    public APIConfig apiConfig; // Az APIConfig ScriptableObject referenciája

    // Singleton minta
    public static APIClient Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(FetchAndDisplayPlayers());
    }


    // GET kérés a játékosok lekérdezéséhez
    public IEnumerator GetPlayers(System.Action<PlayerData[]> onSuccess, System.Action<string> onError)
    {
        string url = apiConfig.playersGetUrl;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
            } else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>("{\"players\":" + jsonResponse + "}");
                onSuccess?.Invoke(wrapper.players);
            }
        }
    }

    public IEnumerator Register(string email, string password, string playerName, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/register"; // Regisztrációs URL

        // Regisztrációs adatok összeállítása
        var registrationData = new RegisterRequest
        {
            email = email,
            password = password,
            playerName = playerName
        };
        string jsonData = JsonUtility.ToJson(registrationData);

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
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; // Bejelentkezési URL

        // Bejelentkezési adatok összeállítása
        var loginData = new LoginRequest
        {
            email = email,
            password = password
        };
        string jsonData = JsonUtility.ToJson(loginData);

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
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
    private IEnumerator FetchAndDisplayPlayers()
    {
        yield return APIClient.Instance.GetPlayers(
            onSuccess: players =>
            {
                foreach (var player in players)
                {
                    Debug.Log($"Player ID: {player.playerId}, Name: {player.playerName}, Character: {player.characterName}");
                }
            },
            onError: error =>
            {
                Debug.LogError("Error fetching players: " + error);
            }
        );
    }


}


[System.Serializable]
public class PlayerData {
    public int playerId;
    public string playerName;
    public string userId;
    public int characterId;
    public int? islandId; // Nullable, mert az islandId lehet null
    public int inventoryId;
    public int? recipeBookId; // Nullable, mert a recipeBookId lehet null
    public int totalScore;
    public DateTime? lastLogin; // Nullable, mert a lastLogin lehet null
    public DateTime createdAt;
    public string characterName;
    public string islandName; // Nullable, mert az islandName lehet null
}

[System.Serializable]
public class PlayerDataWrapper {
    public PlayerData[] players;
}

[System.Serializable]
public class RegisterRequest {
    public string email;
    public string password;
    public string playerName;
}

[System.Serializable]
public class LoginRequest {
    public string email;
    public string password;
}