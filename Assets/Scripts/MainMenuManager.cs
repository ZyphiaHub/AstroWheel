using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons;

    public TMP_Text playerNameText; // Játékos neve
    public TMP_Text playerScoreText; // Pontok
    public TMP_Text lastCompletedIslandText; // Utolsó teljesített sziget száma

    private void Start()
    {
        Debug.Log("Beléptél a Fõmenübe!");
        // Adatok betöltése és megjelenítése
        LoadAndDisplayPlayerData();
        // Gombok állapotának beállítása
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1; // A szigetek indexelése 1-tõl kezdõdik
            if (GameManager.Instance.IsIslandCompleted(islandIndex-1))
            {
                islandButtons[i].interactable = true; 
                //Debug.Log($"gomb {i+1} él");
            } else
            {
                islandButtons[i].interactable = false; 
                //Debug.Log($"gomb {i+1} letiltva");
            }

            //gomb lenyomás vizsgálat
            int sceneIndex = islandIndex; 
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
            
        }
    }
    public void LoadIslandScene(int islandIndex)
    {
        // Sziget scene betöltése
        SceneManager.LoadScene($"Island_{islandIndex}"); }

        private void LoadAndDisplayPlayerData()
    {
        // Játékos neve
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        playerNameText.text = "Witch: \n" + playerName;

        // Pontok betöltése a GameManager segítségével
        int playerScore = GameManager.Instance.LoadTotalScore();
        playerScoreText.text = "ChronoPoints: \n" + playerScore.ToString();

        // Utolsó teljesített sziget száma
        int lastCompletedIsland = PlayerPrefs.GetInt("LastCompletedIsland", 0);
        lastCompletedIslandText.text = "Last Completed \nIsland: " + lastCompletedIsland;
    }

}