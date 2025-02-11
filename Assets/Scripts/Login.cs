using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : IGameState {
    public void EnterState()
    {
        Debug.Log("Beléptél a Login jelentbe");
        SceneManager.LoadScene("Login"); 
    }

    public void UpdateState()
    {
        // Itt ellenõrizheted, hogy a login megoldódott-e
        if (IsPuzzleSolved())
        {
            //GameManager.Instance.ChangeState(new Main_Menu()); // Következõ jelenet
        }
    }

    public void ExitState()
    {
        Debug.Log("Kiléptél az 1. jelenetbõl (Island_1_Sagittarius)!");
    }

    private bool IsPuzzleSolved()
    {
        // Itt ellenõrizd, hogy a puzzle megoldódott-e
        return true; // Csak példa, helyettesítsd a valós logikával
    }
}
