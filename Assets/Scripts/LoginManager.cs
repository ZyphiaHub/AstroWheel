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
        // Gomb esem�ny hozz�rendel�se
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    public void OnLoginButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // Email form�tum ellen�rz�se
        if (!IsValidEmail(email))
        {
            errorMessageText.text = "�rv�nytelen email c�m!";
            return;
        }

        // Egyszer� ellen�rz�s (csak p�lda)
        if (email == "admin@ex.com" && password == "admin")
        {
            Debug.Log("Sikeres bejelentkez�s!");

            // Utols� teljes�tett sziget bet�lt�se
            int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();

            // Jelenetv�lt�s a mentett adatok alapj�n
            if (lastCompletedIsland >= 1) // Ha az els� sziget teljes�tve van
            {
                SceneManager.LoadScene("Main_Menu"); // F�men� bet�lt�se
            } else
            {
                SceneManager.LoadScene("Island_1_Sagittarius"); // Els� sziget bet�lt�se
            }
        } else
        {
            errorMessageText.text = "Hib�s email c�m vagy jelsz�!";
        }

        
    }
    private bool IsValidEmail(string email)
    {
        // Egyszer� regex az email c�m ellen�rz�s�re
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}