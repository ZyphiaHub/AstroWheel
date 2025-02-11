using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Island_1_Sagittarius : IGameState {
    public void EnterState()
    {
        Debug.Log("Bel�pt�l az 1. jelenetbe!");
        // Itt inicializ�lhatod a jelenetet (pl. puzzle elemek bet�lt�se)
        
        SceneManager.LoadScene("Island_1_Sagittarius"); // Az 1. jelenet bet�lt�se
    }

    public void UpdateState()
    {
        // Itt ellen�rizheted, hogy a puzzle megold�dott-e
        if (IsPuzzleSolved())
        {
            //GameManager.Instance.ChangeState(new Scene2State());
        }
    }

    public void ExitState()
    {
        Debug.Log("Kil�pt�l az 1. jelenetb�l!");
        // Itt takar�thatod fel a jelenetet (pl. puzzle elemek elt�vol�t�sa)
    }

    private bool IsPuzzleSolved()
    {
        // Puzzle megoldva, mentj�k az els� sziget teljes�t�s�t
        GameManager.Instance.SaveLastCompletedIsland(1); // 1 = els� sziget
        SceneManager.LoadScene("Island_2_Capricorn"); // K�vetkez� sziget bet�lt�se
        return true;
    }
}

public class Island1Manager : MonoBehaviour {
    private void OnPuzzleSolved()
    {
        // Puzzle megoldva, mentj�k az els� sziget teljes�t�s�t
        GameManager.Instance.SaveLastCompletedIsland(1); // 1 = els� sziget
        SceneManager.LoadScene("Main_Menu"); // Vissza a f�men�be
    }
}
