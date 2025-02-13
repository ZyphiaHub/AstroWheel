using UnityEngine;
using System.Collections.Generic;

public class Island1Manager : IslandManager {
    private void Start()
    {
        islandIndex = 1; // Az els� sziget indexe

        // Dial�gus sorok l�trehoz�sa
        List<string> dialogLines = new List<string>
        {
            "�dv�z�llek az els� szigeten!",
            "Itt meg kell oldanod egy kih�v�st, hogy tov�bbjuss.",
            "J� sz�rakoz�st a puzzle megold�s�hoz!"
        };

        // Dial�gus megjelen�t�se
        dialogManager.ShowDialog(dialogLines);
    }
}