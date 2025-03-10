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

        // Regisztr�lt adatok bet�lt�se a PlayerPrefs-b�l
        string registeredEmail = PlayerPrefs.GetString("RegisteredEmail", "");
        string registeredPassword = PlayerPrefs.GetString("RegisteredPassword", "");

        // Bejelentkez�si adatok ellen�rz�se
        if (email == registeredEmail && password == registeredPassword)
        {
            Debug.Log("Sikeres bejelentkez�s!");

            // Utols� teljes�tett sziget bet�lt�se
            int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();

            // Jelenetv�lt�s a mentett adatok alapj�n
            if (lastCompletedIsland >= 1)
            { // Ha az els� sziget teljes�tve van
                SceneManager.LoadScene("Main_Menu"); // F�men� bet�lt�se
            } else
            {
                SceneManager.LoadScene("Island_1"); // Els� sziget bet�lt�se
            }
        } else
        {
            errorMessageText.text = "Invald email or password!";
        }


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
}