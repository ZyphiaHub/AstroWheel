using UnityEngine;
using System;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour, IGameState {
    public int islandIndex; // A sziget indexe (pl. 1, 2, ..., 12)
    public DialogManager dialogManager; // DialogManager referencia

    private void Start()
    {
        // �llapot inicializ�l�sa
        EnterState();
    }

    // IGameState met�dusok
    public void EnterState()
    {
        Debug.Log($"Bel�pt�l a(z) {islandIndex}. szigetre!");

        // Dial�gus megjelen�t�se, ha a j�t�kos el�sz�r �rkezik
        if (GameManager.Instance.LoadLastCompletedIsland() < islandIndex)  ///0 t�l kezd�dik az els�, 1 t�l a m�sodik
        {
            List<string> dialogList = new List<string>(DialogDatabase.GetDialogForIsland(islandIndex));
            dialogManager.ShowDialog(dialogList);
        } else
        {
            dialogManager.dialogPanel.SetActive(false);
            
        }
    }

    public void UpdateState()
    {
        // Itt ellen�rizheted, hogy a puzzle megold�dott-e
        if (IsPuzzleSolved())
        {
            ExitState();
        }
    }

    public void ExitState()
    {
        Debug.Log($"Kil�pt�l a(z) {islandIndex}. szigetr�l!");

        // Puzzle megoldva, mentj�k a sziget teljes�t�s�t
        GameManager.Instance.SaveLastCompletedIsland(islandIndex);
        GameStateManager.Instance.ChangeState(new MainMenuState()); // Vissza a f�men�be
    }

    // Puzzle megold�s�nak ellen�rz�se
    private bool IsPuzzleSolved()
    {
        // Itt implement�ld a puzzle megold�s�nak logik�j�t
        // P�ld�ul: return puzzle.IsSolved;
        return false; // Csak p�lda
    }
}