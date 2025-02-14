using System.Collections.Generic;
using UnityEngine;

public static class DialogDatabase {
    public static Dictionary<int, string[]> IslandDialogs = new Dictionary<int, string[]>
    {
        { 1, new string[] { "Welcome, young one!", "You are about to embark on a year-long journey.", 
            "If you can solve every task, you will become a fully accredited Time Witch!",
            "However, you must first prove your understanding of the yearly cycle.",
            "Solve this puzzle and place the missing pieces where they belong!" } },
        { 2, new string[] { "�dv a Bak szigeten!", "Ez a hely a fegyelem �s a kitart�s pr�bat�tele.", "Meg tudod mutatni az er�det?" } },
        { 3, new string[] { "Ez a V�z�nt� sziget!", "Itt az intu�ci� �s kreativit�s pr�b�ja v�r r�d.", "Tal�ld meg a megold�st!" } },
        { 4, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 5, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 6, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 7, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 8, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 9, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 10, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 11, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } },
        { 12, new string[] { "�dv a Halak szigeten!", "A v�z elem ereje seg�t a b�lcsess�g el�r�s�ben.", "K�szen �llsz a kih�v�sra?" } }
        
    };

    public static string[] GetDialogForIsland(int islandIndex)
    {
        if (IslandDialogs.ContainsKey(islandIndex))
        {
            return IslandDialogs[islandIndex];
        }
        return new string[] { "Ismeretlen sziget.", "Nincs el�rhet� p�rbesz�d." };
    }
}
