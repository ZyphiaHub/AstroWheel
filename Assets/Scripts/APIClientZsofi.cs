using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*public class APIClientZsofi : MonoBehaviour {
    public string apiUrl = "https://unityrest.onrender.com/api/Players";
    public static APIClient Instance;
    private void Awake()
    {
        // Singleton minta implement�ci�ja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else
        {
            Destroy(gameObject); // Ha m�r van p�ld�ny, t�r�ld ezt
        }
    }
    void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            } else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                // Feldolgozhatod a v�laszt itt
            }
        }
    }

    public IEnumerator PostData(string playerName, string email, string password, int selectedCharacterIndex)
    {
        // J�t�kos adatainak �ssze�ll�t�sa JSON form�tumban
        PlayerData playerData = new PlayerData
        {
            playerName = playerName,
            email = email,
            password = password,
            selectedCharacterIndex = selectedCharacterIndex
            
        };

        string jsonData = JsonUtility.ToJson(playerData);

        // POST k�r�s l�trehoz�sa
        using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // K�r�s elk�ld�se
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            } else
            {
                Debug.Log("Data sent successfully!");
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class PlayerData {
        public string playerName;
        public string email;
        public string password;
        public int selectedCharacterIndex;

        public int totalScore;
    }
}*/