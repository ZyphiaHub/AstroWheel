using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuTest
{
    private MainMenuManager mainMenuManager;
    private Button[] islandButtons;
  
    [SetUp]
    public void SetUp()
    {
        // Bet�ltj�k a Main_Menu scene-t
        SceneManager.LoadScene("Main_Menu");

        // V�runk egy k�pkock�t, hogy a scene teljesen bet�lt�dj�n
        //yield return null;

        // Megkeress�k a MainMenuManager-t
        mainMenuManager = GameObject.FindObjectOfType<MainMenuManager>();

        // Debug �zenet
        if (mainMenuManager == null)
        {
            Debug.LogError("A MainMenuManager nem tal�lhat� a scene-ben.");
        } else
        {
            Debug.Log("A MainMenuManager sikeresen megtal�lva.");
        }

        // Ellen�rizz�k, hogy a MainMenuManager l�tezik
        Assert.IsNotNull(mainMenuManager, "A MainMenuManager nem tal�lhat� a scene-ben.");
    }




    [UnityTest]
    public IEnumerator Test_Buttons_Activate_Based_On_LastCompletedIsland()
    {
        // Gombok t�mbj�nek el�r�se
        islandButtons = mainMenuManager.islandButtons;

        // Ellen�rizz�k, hogy minden gomb letiltva van kezdetben
        for (int i = 1; i < islandButtons.Length; i++)
        {
            Assert.IsFalse(islandButtons[i].interactable, $"A(z) {i + 1}. gomb nem lett letiltva kezdetben.");
        }

        // Mentj�k, hogy az els� sziget teljes�tve van
        GameManager.Instance.SaveLastCompletedIsland(1);
        yield return null; // V�runk egy k�pkock�t, hogy a ment�s friss�lj�n

        // Ellen�rizz�k, hogy csak az els� gomb enged�lyezve van
        Assert.IsTrue(islandButtons[0].interactable, "Az els� gomb nem lett enged�lyezve.");
        for (int i = 1; i < islandButtons.Length; i++)
        {
            Assert.IsFalse(islandButtons[i].interactable, $"A(z) {i + 1}. gomb nem lett letiltva.");
        }
    }

    [TearDown]
    public void TearDown()
    {
        // T�r�lj�k a mentett adatokat a teszt ut�n
        PlayerPrefs.DeleteKey(GameManager.LastCompletedIslandKey);
        PlayerPrefs.Save();
    }
}
