using System.Collections.Generic;
using UnityEngine;

public static class DialogDatabase {
    public static Dictionary<int, string[]> IslandDialogs = new Dictionary<int, string[]>
    {
        { 1, new string[] { "Üdv a Nyilas szigeten!", "Itt a tudás és bölcsesség próbája vár rád.", "Oldd meg a rejtvényt a továbbhaladáshoz!" } },
        { 2, new string[] { "Üdv a Bak szigeten!", "Ez a hely a fegyelem és a kitartás próbatétele.", "Meg tudod mutatni az erõdet?" } },
        { 3, new string[] { "Ez a Vízöntõ sziget!", "Itt az intuíció és kreativitás próbája vár rád.", "Találd meg a megoldást!" } },
        { 4, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 5, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 6, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 7, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 8, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 9, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 10, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 11, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } },
        { 12, new string[] { "Üdv a Halak szigeten!", "A víz elem ereje segít a bölcsesség elérésében.", "Készen állsz a kihívásra?" } }
        
    };

    public static string[] GetDialogForIsland(int islandIndex)
    {
        if (IslandDialogs.ContainsKey(islandIndex))
        {
            return IslandDialogs[islandIndex];
        }
        return new string[] { "Ismeretlen sziget.", "Nincs elérhetõ párbeszéd." };
    }
}
