using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

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

        // Regisztrált adatok betöltése a PlayerPrefs-bõl
        string registeredEmail = PlayerPrefs.GetString("RegisteredEmail", "");
        string registeredPassword = PlayerPrefs.GetString("RegisteredPassword", "");

        // Bejelentkezési adatok ellenõrzése
        if (email == registeredEmail && password == registeredPassword)
        {
            Debug.Log("Sikeres bejelentkezés!");

            // Utolsó teljesített sziget betöltése
            int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();

            // Jelenetváltás a mentett adatok alapján
            if (lastCompletedIsland >= 1)
            { // Ha az elsõ sziget teljesítve van
                SceneManager.LoadScene("Main_Menu"); // Fõmenü betöltése
            } else
            {
                SceneManager.LoadScene("Island_1"); // Elsõ sziget betöltése
            }
        } else
        {
            errorMessageText.text = "Invald email or password!";
        }


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
}