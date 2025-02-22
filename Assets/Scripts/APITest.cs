using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ApiTest : MonoBehaviour {
    void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        string url = "https://localhost:7178/swagger/index.html";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: webrequest sikeres" + request.downloadHandler.text);
        } else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
