using System.Collections.Generic;
using UnityEngine;

public static class DialogDatabase {
    public static Dictionary<int, string[]> IslandDialogs = new Dictionary<int, string[]>
    {
        { 1, new string[] { "Welcome, young one!", "You are about to embark on a year-long journey.", 
            "If you can solve every task, you will become a fully accredited Time Witch!",
            "However, you must first prove your understanding of the yearly cycle.",
            "Solve this puzzle and place the missing pieces where they belong!" } },
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
