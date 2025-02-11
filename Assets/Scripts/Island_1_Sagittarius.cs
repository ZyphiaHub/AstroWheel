using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Island_1_Sagittarius : IGameState {
    public void EnterState()
    {
        Debug.Log("Beléptél az 1. jelenetbe!");
        // Itt inicializálhatod a jelenetet (pl. puzzle elemek betöltése)
        
        SceneManager.LoadScene("Island_1_Sagittarius"); // Az 1. jelenet betöltése
    }

    public void UpdateState()
    {
        // Itt ellenõrizheted, hogy a puzzle megoldódott-e
        if (IsPuzzleSolved())
        {
            //GameManager.Instance.ChangeState(new Scene2State());
        }
    }

    public void ExitState()
    {
        Debug.Log("Kiléptél az 1. jelenetbõl!");
        // Itt takaríthatod fel a jelenetet (pl. puzzle elemek eltávolítása)
    }

    private bool IsPuzzleSolved()
    {
        // Puzzle megoldva, mentjük az elsõ sziget teljesítését
        GameManager.Instance.SaveLastCompletedIsland(1); // 1 = elsõ sziget
        SceneManager.LoadScene("Island_2_Capricorn"); // Következõ sziget betöltése
        return true;
    }
}

public class Island1Manager : MonoBehaviour {
    private void OnPuzzleSolved()
    {
        // Puzzle megoldva, mentjük az elsõ sziget teljesítését
        GameManager.Instance.SaveLastCompletedIsland(1); // 1 = elsõ sziget
        SceneManager.LoadScene("Main_Menu"); // Vissza a fõmenübe
    }
}
