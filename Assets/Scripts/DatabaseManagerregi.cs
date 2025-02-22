using UnityEngine;
using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class DatabaseManagerregi : MonoBehaviour {
    private SQLiteConnection dbConnection;

    void Start()
    {
        // Adatbázis útvonal
        string dbPath = $"{Application.streamingAssetsPath}/game_data.db";


        if (System.IO.File.Exists(dbPath))
        {
            Debug.Log("Adatbázis fájl elérhetõ!");
        } else
        {
            Debug.LogError("Adatbázis fájl nem található!");
        }
        // Adatbázis tesztelése
        try
        {
            dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("Adatbázis csatlakoztatva!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Hiba az adatbázis csatlakoztatása során: {ex.Message}");
        }



        dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        Debug.Log("Adatbázis csatlakoztatva!");

        // Tesztelés
        TestDatabase();
    }

    void TestDatabase()
    {
        // Táblák létrehozása, ha nem léteznek
        dbConnection.CreateTable<Potion>();

        // Adatok beszúrása
        dbConnection.InsertAll(new List<Potion>
        {
            new Potion { Name = "Healing Potion", Effect = "Restores 50 HP", Rarity = 2, Price = 10.5f },
            new Potion { Name = "Mana Elixir", Effect = "Restores 30 Mana", Rarity = 3, Price = 15.0f }
        });

        Debug.Log("Adatok hozzáadva!");

        // Adatok lekérdezése
        var potions = dbConnection.Table<Potion>().ToList();
        foreach (var potion in potions)
        {
            Debug.Log(potion.ToString());
        }
    }

    private void OnDestroy()
    {
        // Adatbázis kapcsolat bezárása
        dbConnection.Close();
        Debug.Log("Adatbázis kapcsolat lezárva!");
    }
}
      
    

