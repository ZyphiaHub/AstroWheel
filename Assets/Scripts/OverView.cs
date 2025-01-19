using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverView : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("Main_Menu");
    }

    public void HOGame()
    {
        SceneManager.LoadSceneAsync("HOGame");
    }
    public void Island_1()
    {
        SceneManager.LoadSceneAsync("Island_1_Sagittarius");
    }

    public void Island_2()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Island_3()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void Island_4()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void Island_5()
    {
        SceneManager.LoadSceneAsync(6);
    }

    public void Island_6()
    {
        SceneManager.LoadSceneAsync(7);
    }

    public void Island_7()
    {
        SceneManager.LoadSceneAsync(8);
    }

    public void Island_8()
    {
        SceneManager.LoadSceneAsync(9);
    }

    public void Island_9()
    {
        SceneManager.LoadSceneAsync(10);
    }

    public void Island_10()
    {
        SceneManager.LoadSceneAsync(11);
    }

    public void Island11()
    {
        SceneManager.LoadSceneAsync(12);
    }

    public void Island12()
    {
        SceneManager.LoadSceneAsync(13);
    }

    public void Hearth()
    {
        SceneManager.LoadSceneAsync(14);
    }

    public void MemoryGame()
    {
        SceneManager.LoadSceneAsync("MemoGame");
    }
}
