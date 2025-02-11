using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : IGameState {
    public void EnterState()
    {
        Debug.Log("Bel�pt�l a Login jelentbe");
        SceneManager.LoadScene("Login"); 
    }

    public void UpdateState()
    {
        // Itt ellen�rizheted, hogy a login megold�dott-e
        if (IsPuzzleSolved())
        {
            //GameManager.Instance.ChangeState(new Main_Menu()); // K�vetkez� jelenet
        }
    }

    public void ExitState()
    {
        Debug.Log("Kil�pt�l az 1. jelenetb�l (Island_1_Sagittarius)!");
    }

    private bool IsPuzzleSolved()
    {
        // Itt ellen�rizd, hogy a puzzle megold�dott-e
        return true; // Csak p�lda, helyettes�tsd a val�s logik�val
    }
}
