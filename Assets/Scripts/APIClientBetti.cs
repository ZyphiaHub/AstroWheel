using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour {
    public APIConfig apiConfig; // Az APIConfig ScriptableObject referenci�ja
    //public InventoryManager inventoryManager;
    
    public static APIClient Instance { get;  private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("APIClient initialized.");
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(FetchAndDisplayPlayers());
    }


    // GET k�r�s a j�t�kosok lek�rdez�s�hez
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
        string url = "https://astrowheelapi.onrender.com/api/auth/register"; // Regisztr�ci�s URL

        // Regisztr�ci�s adatok �ssze�ll�t�sa
        var registrationData = new RegisterRequest
        {
            email = email,
            password = password,
            playerName = playerName
        };
        string jsonData = JsonUtility.ToJson(registrationData);

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
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; // Bejelentkez�si URL

        // Bejelentkez�si adatok �ssze�ll�t�sa
        var loginData = new LoginRequest
        {
            email = email,
            password = password
        };
        string jsonData = JsonUtility.ToJson(loginData);

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

    public IEnumerator SaveInventory(string playerId, List<InventoryItem> items, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/inventoryMaterials";

        // Az inventory adatainak �ssze�ll�t�sa
        InventoryData inventoryData = new InventoryData
        {
            playerId = playerId,
            items = items
        };

        // JSON adat l�trehoz�sa
        string jsonData = JsonUtility.ToJson(inventoryData);
        Debug.Log("Sending inventory data: " + jsonData);

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
                Debug.LogError("Inventory save failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Inventory saved successfully: " + webRequest.downloadHandler.text);
            }
        }
    }
    public IEnumerator SaveCraftedInventory(string playerId, List<InventoryItem> craftedItems, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/inventoryMaterials"; // Vagy egy m�sik URL, ha a crafted inventory k�l�n v�gponton ment�dik

        // Az inventory adatainak �ssze�ll�t�sa
        InventoryData inventoryData = new InventoryData
        {
            playerId = playerId, // A playerId stringk�nt ker�l elk�ld�sre
            items = craftedItems
        };

        // JSON adat l�trehoz�sa
        string jsonData = JsonUtility.ToJson(inventoryData);
        Debug.Log("Sending crafted inventory data: " + jsonData);

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
                Debug.LogError("Crafted inventory save failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Crafted inventory saved successfully: " + webRequest.downloadHandler.text);
            }
        }
    }
    // PUT k�r�s a j�t�kos adatainak friss�t�s�hez
    public IEnumerator UpdatePlayer(string playerId, PlayerData updatedData, System.Action<string> onSuccess, System.Action<string> onError)
    {
        // Az URL �ssze�ll�t�sa a playerId alapj�n
        string url = $"https://astrowheelapi.onrender.com/api/players/{playerId}";

        // A friss�tend� adatok JSON form�tumba alak�t�sa
        string jsonData = JsonUtility.ToJson(updatedData);
        Debug.Log("Sending update data: " + jsonData);

        // PUT k�r�s elk�ld�se
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
                Debug.LogError("Player update failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Player updated successfully: " + webRequest.downloadHandler.text);
            }
        }
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

[System.Serializable]
public class InventoryData {
    public string playerId; // A j�t�kos egyedi azonos�t�ja
    public List<InventoryItem> items; // Az inventory t�rgyai
}

[System.Serializable]
public class InventoryItem {
    public int itemId; // A t�rgy egyedi azonos�t�ja
    public int quantity; // A t�rgy mennyis�ge
}