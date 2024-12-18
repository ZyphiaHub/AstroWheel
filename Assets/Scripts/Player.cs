using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int level = 3;
    public int timepoints = 40;
    public int pipacs = 12;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer ()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        timepoints = data.timepoints;
        pipacs = data.pipacs;


    }
}
