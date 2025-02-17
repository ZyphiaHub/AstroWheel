using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons; 

    private void Start()
    {
        Debug.Log("Bel�pt�l a F�men�be!");
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
        SceneManager.LoadScene($"Island_{islandIndex}");
    }
}