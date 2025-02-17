using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System.Collections;

public class MainMenuManagerTests {
    private GameManager gameManager;
    private MainMenuManager mainMenuManager;
    private GameObject gameManagerObject;
    private GameObject mainMenuManagerObject;

    public const string LastCompletedIslandKey = "LastCompletedIsland";

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy új GameManager objektumot
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Létrehozunk egy új MainMenuManager objektumot
        mainMenuManagerObject = new GameObject();
        mainMenuManager = mainMenuManagerObject.AddComponent<MainMenuManager>();

        // Létrehozunk egy új gomb tömböt és hozzárendeljük a MainMenuManager-hez
        mainMenuManager.islandButtons = new Button[12];
        for (int i = 0; i < mainMenuManager.islandButtons.Length; i++)
        {
            GameObject buttonObject = new GameObject();
            mainMenuManager.islandButtons[i] = buttonObject.AddComponent<Button>();
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Töröljük a létrehozott objektumokat a teszt után
        Object.Destroy(gameManagerObject);
        Object.Destroy(mainMenuManagerObject);
    }

    [UnityTest]
    public IEnumerator TestIslandButtonsInteractability()
    {
        // Beállítjuk, hogy a 3. sziget legyen az utolsó teljesített sziget
        
        
        GameManager.Instance.SaveLastCompletedIsland(3);
        yield return null; 

        // Ellenõrizzük, hogy a gombok interaktívvá váltak-e a megfelelõ szigeteknél
        for (int i = 0; i < mainMenuManager.islandButtons.Length; i++)
        {
            if (i < 4)
                Assert.IsTrue(mainMenuManager.islandButtons[i].interactable, $"Gomb {i} nem interaktív, pedig kellene lennie.");
            
            else
                Assert.IsFalse(mainMenuManager.islandButtons[i].interactable, $"Gomb {i} interaktív, pedig nem kellene lennie.");
        }
    }
}