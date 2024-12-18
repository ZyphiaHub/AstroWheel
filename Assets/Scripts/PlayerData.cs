using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    // Start is called before the first frame update

    public int level;
    public int timepoints;
    public int pipacs;

    public PlayerData (Player player)
    {
        level = player.level;
        timepoints = player.timepoints;
        pipacs = player.pipacs;

    }
}
