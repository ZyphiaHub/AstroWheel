using UnityEngine;
using System;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour, IGameState {
    public int islandIndex; // A sziget indexe (pl. 1, 2, ..., 12)
    public DialogManager dialogManager; // DialogManager referencia

    private void Start()
    {
        // Állapot inicializálása
        EnterState();
    }

    // IGameState metódusok
    public void EnterState()
    {
        Debug.Log($"Beléptél a(z) {islandIndex}. szigetre!");

        // Dialógus megjelenítése, ha a játékos elõször érkezik
        if (GameManager.Instance.LoadLastCompletedIsland() < islandIndex)  ///0 tól kezdõdik az elsõ, 1 tõl a második
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
        // Itt ellenõrizheted, hogy a puzzle megoldódott-e
        if (IsPuzzleSolved())
        {
            ExitState();
        }
    }

    public void ExitState()
    {
        Debug.Log($"Kiléptél a(z) {islandIndex}. szigetrõl!");

        // Puzzle megoldva, mentjük a sziget teljesítését
        GameManager.Instance.SaveLastCompletedIsland(islandIndex);
        GameStateManager.Instance.ChangeState(new MainMenuState()); // Vissza a fõmenübe
    }

    // Puzzle megoldásának ellenõrzése
    private bool IsPuzzleSolved()
    {
        // Itt implementáld a puzzle megoldásának logikáját
        // Például: return puzzle.IsSolved;
        return false; // Csak példa
    }
}