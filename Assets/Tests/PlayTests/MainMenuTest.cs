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
        // Betöltjük a Main_Menu scene-t
        SceneManager.LoadScene("Main_Menu");

        // Várunk egy képkockát, hogy a scene teljesen betöltõdjön
        //yield return null;

        // Megkeressük a MainMenuManager-t
        mainMenuManager = GameObject.FindObjectOfType<MainMenuManager>();

        // Debug üzenet
        if (mainMenuManager == null)
        {
            Debug.LogError("A MainMenuManager nem található a scene-ben.");
        } else
        {
            Debug.Log("A MainMenuManager sikeresen megtalálva.");
        }

        // Ellenõrizzük, hogy a MainMenuManager létezik
        Assert.IsNotNull(mainMenuManager, "A MainMenuManager nem található a scene-ben.");
    }




    [UnityTest]
    public IEnumerator Test_Buttons_Activate_Based_On_LastCompletedIsland()
    {
        // Gombok tömbjének elérése
        islandButtons = mainMenuManager.islandButtons;

        // Ellenõrizzük, hogy minden gomb letiltva van kezdetben
        for (int i = 1; i < islandButtons.Length; i++)
        {
            Assert.IsFalse(islandButtons[i].interactable, $"A(z) {i + 1}. gomb nem lett letiltva kezdetben.");
        }

        // Mentjük, hogy az elsõ sziget teljesítve van
        GameManager.Instance.SaveLastCompletedIsland(1);
        yield return null; // Várunk egy képkockát, hogy a mentés frissüljön

        // Ellenõrizzük, hogy csak az elsõ gomb engedélyezve van
        Assert.IsTrue(islandButtons[0].interactable, "Az elsõ gomb nem lett engedélyezve.");
        for (int i = 1; i < islandButtons.Length; i++)
        {
            Assert.IsFalse(islandButtons[i].interactable, $"A(z) {i + 1}. gomb nem lett letiltva.");
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Töröljük a mentett adatokat a teszt után
        PlayerPrefs.DeleteKey(GameManager.LastCompletedIslandKey);
        PlayerPrefs.Save();
    }
}
