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
        // Gomb esem�ny hozz�rendel�se
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
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; // Bejelentkez�si URL

        // Bejelentkez�si adatok �ssze�ll�t�sa
        var loginData = new
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
}