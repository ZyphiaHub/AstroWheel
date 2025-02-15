using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IslandManager : MonoBehaviour, IGameState {
    public int islandIndex; // A sziget indexe (pl. 1, 2, ..., 12)
    public DialogManager dialogManager;
    public Button bactToMainMenuBtn;
    public Button previousIslandButton; // El�z� sziget gombja
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
        Debug.Log($"Kil�pt�l a(z) {islandIndex}. szigetr�l!");

        // Puzzle megoldva, mentj�k a sziget teljes�t�s�t
        GameManager.Instance.SaveLastCompletedIsland(islandIndex);
        GameStateManager.Instance.ChangeState(new MainMenuState()); 
    }
    private void OnBackToMainMenuClicked()
    {
        Debug.Log($"GameStateManager.Instance: {GameStateManager.Instance}");
        // Visszal�p�s a f�men�be
        GameStateManager.Instance.ChangeState(new MainMenuState());
    }

    private void OnPreviousIslandClicked()
    {
        NavigateToIsland(islandIndex - 1);
    }

    // Gomb esem�nykezel�je: K�vetkez� sziget
    private void OnNextIslandClicked()
    {
        NavigateToIsland(islandIndex + 1);
    }

    private void NavigateToIsland(int targetIslandIndex)
    {
        if (targetIslandIndex >= 1 && targetIslandIndex <= 12) 
        {
            Debug.Log($"Navig�l�s a(z) {targetIslandIndex}. szigetre!");
            SceneManager.LoadScene($"Island_{targetIslandIndex}"); 
        } else
        {
            Debug.LogWarning($"�rv�nytelen sziget index: {targetIslandIndex}");
        }
    }

    private void UpdateNavigationButtons()
    {
        // El�z� sziget gomb letilt�sa, ha az els� szigeten vagyunk
        if (previousIslandButton != null)
        {
            previousIslandButton.interactable = (islandIndex > 1);
        }

        // K�vetkez� sziget gomb letilt�sa, ha az utols� szigeten vagyunk
        if (nextIslandButton != null)
        {
            nextIslandButton.interactable = (islandIndex < 12);
        }
    }

    // Puzzle megold�s�nak ellen�rz�se
    private bool IsPuzzleSolved()
    {
        // Itt implement�ld a puzzle megold�s�nak logik�j�t
        // P�ld�ul: return puzzle.IsSolved;
        return false; // Csak p�lda
    }
}