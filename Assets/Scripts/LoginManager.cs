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
    public TMP_Text errorMessageText;

    private void Start()
    {
        // Gomb esemény hozzárendelése
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    public void OnLoginButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // Email formátum ellenõrzése
        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Érvénytelen email cím!";
            return;
        }

        // Egyszerû ellenõrzés (csak példa)
        if (email == "admin@ex.com" && password == "admin")
        {
            Debug.Log("Sikeres bejelentkezés!");

            // Utolsó teljesített sziget betöltése
            int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();

            // Jelenetváltás a mentett adatok alapján
            if (lastCompletedIsland >= 1) // Ha az elsõ sziget teljesítve van
            {
                SceneManager.LoadScene("Main_Menu"); // Fõmenü betöltése
            } else
            {
                SceneManager.LoadScene("Island_1_Sagittarius"); // Elsõ sziget betöltése
            }
        } else
        {
            errorMessageText.text = "Hibás email cím vagy jelszó!";
        }

        
    }
    private bool IsValidEmail(string email)
    {
        // Egyszerû regex az email cím ellenõrzésére
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}