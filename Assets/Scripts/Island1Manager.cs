using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Island1Manager : MonoBehaviour {

    public DialogManager dialogManager; // DialogManager referencia

    private void Start()
    {
        // Dialógus megjelenítése
        dialogManager.ShowDialog("Welcome young one! Jó szórakozást a puzzle megoldásához!");
    }
    private void OnPuzzleSolved()
    {
        // Puzzle megoldva, mentjük az elsõ sziget teljesítését
        GameManager.Instance.SaveLastCompletedIsland(1); // 1 = elsõ sziget
        SceneManager.LoadScene("Main_Menu"); // Vissza a fõmenübe
    }
}
