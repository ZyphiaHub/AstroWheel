using UnityEngine;
using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class DatabaseManagerregi : MonoBehaviour {
    private SQLiteConnection dbConnection;

    void Start()
    {
        // Adatb�zis �tvonal
        string dbPath = $"{Application.streamingAssetsPath}/game_data.db";


        if (System.IO.File.Exists(dbPath))
        {
            Debug.Log("Adatb�zis f�jl el�rhet�!");
        } else
        {
            Debug.LogError("Adatb�zis f�jl nem tal�lhat�!");
        }
        // Adatb�zis tesztel�se
        try
        {
            dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("Adatb�zis csatlakoztatva!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Hiba az adatb�zis csatlakoztat�sa sor�n: {ex.Message}");
        }



        dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        Debug.Log("Adatb�zis csatlakoztatva!");

        // Tesztel�s
        TestDatabase();
    }

    void TestDatabase()
    {
        // T�bl�k l�trehoz�sa, ha nem l�teznek
        dbConnection.CreateTable<Potion>();

        // Adatok besz�r�sa
        dbConnection.InsertAll(new List<Potion>
        {
            new Potion { Name = "Healing Potion", Effect = "Restores 50 HP", Rarity = 2, Price = 10.5f },
            new Potion { Name = "Mana Elixir", Effect = "Restores 30 Mana", Rarity = 3, Price = 15.0f }
        });

        Debug.Log("Adatok hozz�adva!");

        // Adatok lek�rdez�se
        var potions = dbConnection.Table<Potion>().ToList();
        foreach (var potion in potions)
        {
            Debug.Log(potion.ToString());
        }
    }

    private void OnDestroy()
    {
        // Adatb�zis kapcsolat bez�r�sa
        dbConnection.Close();
        Debug.Log("Adatb�zis kapcsolat lez�rva!");
    }
}
      
    

