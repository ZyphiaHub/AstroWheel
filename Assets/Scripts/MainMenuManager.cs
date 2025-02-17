using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons; 

    private void Start()
    {
        Debug.Log("Beléptél a Fõmenübe!");
        // Gombok állapotának beállítása
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1; // A szigetek indexelése 1-tõl kezdõdik
            if (GameManager.Instance.IsIslandCompleted(islandIndex-1))
            {
                islandButtons[i].interactable = true; 
                Debug.Log($"gomb {i+1} él");
            } else
            {
                islandButtons[i].interactable = false; 
                Debug.Log($"gomb {i+1} letiltva");
            }

            //gomb lenyomás vizsgálat
            int sceneIndex = islandIndex; 
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
            
        }
    }
        public void LoadIslandScene(int islandIndex)
    {
        // Sziget scene betöltése
        SceneManager.LoadScene($"Island_{islandIndex}");
    }
}