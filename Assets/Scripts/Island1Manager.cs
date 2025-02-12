using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Island1Manager : MonoBehaviour {

    public DialogManager dialogManager; // DialogManager referencia

    private void Start()
    {
        // Dial�gus megjelen�t�se
        dialogManager.ShowDialog("Welcome young one! J� sz�rakoz�st a puzzle megold�s�hoz!");
    }
    private void OnPuzzleSolved()
    {
        // Puzzle megoldva, mentj�k az els� sziget teljes�t�s�t
        GameManager.Instance.SaveLastCompletedIsland(1); // 1 = els� sziget
        SceneManager.LoadScene("Main_Menu"); // Vissza a f�men�be
    }
}
