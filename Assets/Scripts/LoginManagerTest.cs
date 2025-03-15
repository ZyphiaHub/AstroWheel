using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour {
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public Button registerButton;
    public Button quitButton;
    public TMP_Text errorMessageText;

    private void Start()
    {
        // Gomb esemény hozzárendelése
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
        StartCoroutine(Login(email, password,
            response =>
            {
                Debug.Log("Login successful: " + response);

                // Sikeres bejelentkezés esetén betöltjük a megfelelõ jelenetet
                int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();

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
        ));
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
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; // Bejelentkezési URL

        // Bejelentkezési adatok összeállítása
        var loginData = new
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
}