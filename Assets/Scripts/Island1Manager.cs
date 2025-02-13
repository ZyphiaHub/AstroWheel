using UnityEngine;
using System.Collections.Generic;

public class Island1Manager : IslandManager {
    private void Start()
    {
        islandIndex = 1; // Az elsõ sziget indexe

        // Dialógus sorok létrehozása
        List<string> dialogLines = new List<string>
        {
            "Üdvözöllek az elsõ szigeten!",
            "Itt meg kell oldanod egy kihívást, hogy továbbjuss.",
            "Jó szórakozást a puzzle megoldásához!"
        };

        // Dialógus megjelenítése
        dialogManager.ShowDialog(dialogLines);
    }
}