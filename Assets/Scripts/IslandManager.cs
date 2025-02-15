using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IslandManager : MonoBehaviour, IGameState {
    public int islandIndex; // A sziget indexe (pl. 1, 2, ..., 12)
    public DialogManager dialogManager;
    public Button bactToMainMenuBtn;
    public Button previousIslandButton; // Elõzõ sziget gombja
    public Button nextIslandButton;

    private void Start()
    {
        if (bactToMainMenuBtn != null)
        {
            bactToMainMenuBtn.onClick.AddListener(OnBackToMainMenuClicked);
        }
        if (previousIslandButton != null)
        {
            previousIslandButton.onClick.AddListener(OnPreviousIslandClicked);
        }
        if (nextIslandButton != null)
        {
            nextIslandButton.onClick.AddListener(OnNextIslandClicked);
        }

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
        UpdateNavigationButtons();
    }

    public void UpdateState()
    {
        
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
        GameStateManager.Instance.ChangeState(new MainMenuState()); 
    }
    private void OnBackToMainMenuClicked()
    {
        Debug.Log($"GameStateManager.Instance: {GameStateManager.Instance}");
        // Visszalépés a fõmenübe
        GameStateManager.Instance.ChangeState(new MainMenuState());
    }

    private void OnPreviousIslandClicked()
    {
        NavigateToIsland(islandIndex - 1);
    }

    // Gomb eseménykezelõje: Következõ sziget
    private void OnNextIslandClicked()
    {
        NavigateToIsland(islandIndex + 1);
    }

    private void NavigateToIsland(int targetIslandIndex)
    {
        if (targetIslandIndex >= 1 && targetIslandIndex <= 12) 
        {
            Debug.Log($"Navigálás a(z) {targetIslandIndex}. szigetre!");
            SceneManager.LoadScene($"Island_{targetIslandIndex}"); 
        } else
        {
            Debug.LogWarning($"Érvénytelen sziget index: {targetIslandIndex}");
        }
    }

    private void UpdateNavigationButtons()
    {
        // Elõzõ sziget gomb letiltása, ha az elsõ szigeten vagyunk
        if (previousIslandButton != null)
        {
            previousIslandButton.interactable = (islandIndex > 1);
        }

        // Következõ sziget gomb letiltása, ha az utolsó szigeten vagyunk
        if (nextIslandButton != null)
        {
            nextIslandButton.interactable = (islandIndex < 12);
        }
    }

    // Puzzle megoldásának ellenõrzése
    private bool IsPuzzleSolved()
    {
        // Itt implementáld a puzzle megoldásának logikáját
        // Például: return puzzle.IsSolved;
        return false; // Csak példa
    }
}