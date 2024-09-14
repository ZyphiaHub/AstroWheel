using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Island_3 : MonoBehaviour
{
    public void BackToOverView()
    {
        SceneManager.LoadSceneAsync(1); 
    }
}
