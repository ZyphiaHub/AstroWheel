using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIClient : MonoBehaviour {
    public string apiUrl = "https://unityrest.onrender.com/api/Players"; // Cseréld ki a tényleges végpontra

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
                // Feldolgozhatod a választ itt
            }
        }
    }
}