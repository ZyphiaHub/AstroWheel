using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons; // Gombok tömbje

    private void Start()
    {
        // Gombok állapotának beállítása
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1; // A szigetek indexelése 1-tõl kezdõdik
            if (GameManager.Instance.IsIslandCompleted(islandIndex))
            {
                islandButtons[i].interactable = true; // Gomb engedélyezve
            } else
            {
                islandButtons[i].interactable = false; // Gomb letiltva
            }

            // Gomb esemény hozzárendelése
            int sceneIndex = islandIndex; // A szigetek scene-jei indexelése
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
        }
    }
        public void LoadIslandScene(int islandIndex)
    {
        // Sziget scene betöltése
        SceneManager.LoadScene($"Island_{islandIndex}");
    }
}