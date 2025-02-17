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
        // L�trehozunk egy �j GameManager objektumot
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // L�trehozunk egy �j MainMenuManager objektumot
        mainMenuManagerObject = new GameObject();
        mainMenuManager = mainMenuManagerObject.AddComponent<MainMenuManager>();

        // L�trehozunk egy �j gomb t�mb�t �s hozz�rendelj�k a MainMenuManager-hez
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
        // T�r�lj�k a l�trehozott objektumokat a teszt ut�n
        Object.Destroy(gameManagerObject);
        Object.Destroy(mainMenuManagerObject);
    }

    [UnityTest]
    public IEnumerator TestIslandButtonsInteractability()
    {
        // Be�ll�tjuk, hogy a 3. sziget legyen az utols� teljes�tett sziget
        
        
        GameManager.Instance.SaveLastCompletedIsland(3);
        yield return null; 

        // Ellen�rizz�k, hogy a gombok interakt�vv� v�ltak-e a megfelel� szigetekn�l
        for (int i = 0; i < mainMenuManager.islandButtons.Length; i++)
        {
            if (i < 4)
                Assert.IsTrue(mainMenuManager.islandButtons[i].interactable, $"Gomb {i} nem interakt�v, pedig kellene lennie.");
            
            else
                Assert.IsFalse(mainMenuManager.islandButtons[i].interactable, $"Gomb {i} interakt�v, pedig nem kellene lennie.");
        }
    }
}