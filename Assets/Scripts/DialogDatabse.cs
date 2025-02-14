using System.Collections.Generic;
using UnityEngine;

public static class DialogDatabase {
    public static Dictionary<int, string[]> IslandDialogs = new Dictionary<int, string[]>
    {
        { 1, new string[] { "Welcome, young one!", "You are about to embark on a year-long journey.", 
                            "If you can solve every task, you will become a fully accredited Time Witch!",
                            "However, you must first prove your understanding of the yearly cycle.",
                            "Solve this puzzle and place the missing pieces where they belong!" } },
        { 2, new string[] { "Welcome to the domain of Capricorn! As a Time Witch you must craft potions later.",
                            "But before that, you need to show me that you can distinguish between different plant materials.",
                            "Fail, and you will be trapped in limbo—just as I am, caught between two states of being.",
                            "Capricorn gestures with his hooves, demonstrating his predicament."} },
        { 3, new string[] { "Welcome to the realm of Aquarius!", "To prove your worth, you must catch what falls from the sky—be swift, " +
                            "be precise, and do not let distractions break your focus.",
                            "You must follow where the wind takes you, adapting with every moment.",
                            "Show me your mastery, and I shall grant you passage!"} },
        { 4, new string[] { "Drifting through the tides, you have reached my domain—welcome to the waters of Pisces.",
                            "You must prove your wisdom by matching names to forms,  just as a true alchemist deciphers the hidden language of ingredients.",
                            "A name without a form is an echo. A form without a name is a shadow.",
                            "As I am both fish and maiden, so too must you unite knowledge and perception.",
                            "Show me your insight, and I shall guide you further along your path."} },
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
