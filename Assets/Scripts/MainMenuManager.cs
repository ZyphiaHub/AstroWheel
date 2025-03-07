using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons;

    public TMP_Text playerNameText; // J�t�kos neve
    public TMP_Text playerScoreText; // Pontok
    public TMP_Text lastCompletedIslandText; // Utols� teljes�tett sziget sz�ma

    private void Start()
    {
        Debug.Log("Bel�pt�l a F�men�be!");
        // Adatok bet�lt�se �s megjelen�t�se
        LoadAndDisplayPlayerData();
        // Gombok �llapot�nak be�ll�t�sa
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1; // A szigetek indexel�se 1-t�l kezd�dik
            if (GameManager.Instance.IsIslandCompleted(islandIndex-1))
            {
                islandButtons[i].interactable = true; 
                Debug.Log($"gomb {i+1} �l");
            } else
            {
                islandButtons[i].interactable = false; 
                Debug.Log($"gomb {i+1} letiltva");
            }

            //gomb lenyom�s vizsg�lat
            int sceneIndex = islandIndex; 
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
            
        }
    }
    public void LoadIslandScene(int islandIndex)
    {
        // Sziget scene bet�lt�se
        SceneManager.LoadScene($"Island_{islandIndex}"); }

        private void LoadAndDisplayPlayerData()
    {
        // J�t�kos neve
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        playerNameText.text = "Player: " + playerName;

        // Pontok
        int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        playerScoreText.text = "TimeScore: " + playerScore;

        // Utols� teljes�tett sziget sz�ma
        int lastCompletedIsland = PlayerPrefs.GetInt("LastCompletedIsland", 0);
        lastCompletedIslandText.text = "Last Completed Island: " + lastCompletedIsland;
    }

}