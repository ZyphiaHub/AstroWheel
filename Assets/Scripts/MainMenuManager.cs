using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons;
    [Header("Volume Controls")]
    public Button volumeUpButton;
    public Button volumeDownButton;
    public TMP_Text volumeText;

    public TMP_Text playerNameText; 
    public TMP_Text playerScoreText; 
    public TMP_Text lastCompletedIslandText; 

    private void Start()
    {
        Debug.Log("Bel�pt�l a F�men�be!");
        
        volumeUpButton.onClick.AddListener(IncreaseVolume);
        volumeDownButton.onClick.AddListener(DecreaseVolume);
        UpdateVolumeUI();
        LoadAndDisplayPlayerData();
        
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1; // A szigetek indexel�se 1-t�l kezd�dik
            if (GameManager.Instance.IsIslandCompleted(islandIndex-1))
            {
                islandButtons[i].interactable = true; 
                //Debug.Log($"gomb {i+1} �l");
            } else
            {
                islandButtons[i].interactable = false; 
                //Debug.Log($"gomb {i+1} letiltva");
            }

            //gomb lenyom�s vizsg�lat
            int sceneIndex = islandIndex; 
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
            
        }
    }
    private void IncreaseVolume()
    {
        AudioManager.Instance.IncreaseVolume();
        UpdateVolumeUI();
    }

    private void DecreaseVolume()
    {
        AudioManager.Instance.DecreaseVolume();
        UpdateVolumeUI();
    }

    private void UpdateVolumeUI()
    {
        if (volumeText != null)
        {
            float volume = AudioManager.Instance.GetCurrentVolume();
            volumeText.text = $"Volume: {Mathf.RoundToInt(volume * 100)}%";
        }
    }

    public void LoadIslandScene(int islandIndex)
    {
        SceneManager.LoadScene($"Island_{islandIndex}"); }

        private void LoadAndDisplayPlayerData()
    {
        // J�t�kos neve
        string playerName = PlayerPrefs.GetString("PlayerUsername", "Guest");
        playerNameText.text = "Witch: \n" + playerName;

        // Pontok bet�lt�se a GameManager seg�ts�g�vel
        int playerScore = GameManager.Instance.LoadTotalScore();
        playerScoreText.text = "ChronoPoints: \n" + playerScore.ToString();

        // Utols� teljes�tett sziget sz�ma
        int lastCompletedIsland = PlayerPrefs.GetInt("LastCompletedIsland", 0);
        lastCompletedIslandText.text = "Last Completed \nIsland: " + lastCompletedIsland;
    }

}