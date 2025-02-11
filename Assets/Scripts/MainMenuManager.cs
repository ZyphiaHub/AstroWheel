using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons; // Gombok t�mbje

    private void Start()
    {
        // Gombok �llapot�nak be�ll�t�sa
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1; // A szigetek indexel�se 1-t�l kezd�dik
            if (GameManager.Instance.IsIslandCompleted(islandIndex))
            {
                islandButtons[i].interactable = true; // Gomb enged�lyezve
            } else
            {
                islandButtons[i].interactable = false; // Gomb letiltva
            }

            // Gomb esem�ny hozz�rendel�se
            int sceneIndex = islandIndex; // A szigetek scene-jei indexel�se
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
        }
    }
        public void LoadIslandScene(int islandIndex)
    {
        // Sziget scene bet�lt�se
        SceneManager.LoadScene($"Island_{islandIndex}");
    }
}