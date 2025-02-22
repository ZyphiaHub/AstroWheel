using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {
    private SQLiteConnection db;
    private string dbFileName = "game_data.db";

    void Start()
    {
        InitializeDatabase();
        CreateTable();
        InsertSampleData(); // Opcionális, csak teszteléshez
        LoadCharacters();
    }

    private void InitializeDatabase()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, dbFileName);
        string streamingPath = Path.Combine(Application.streamingAssetsPath, dbFileName);

        if (!File.Exists(dbPath))
        {
            File.Copy(streamingPath, dbPath);
            Debug.Log("Database copied to: " + dbPath);
        } else
        {
            Debug.Log("Database already exists at: " + dbPath);
        }

        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Database connected: " + dbPath);
    }

    private void CreateTable()
    {
        db.CreateTable<Character>();
        Debug.Log("Character table ensured.");
    }

    private void InsertSampleData()
    {
        if (db.Table<Character>().Count() == 0)
        {
            db.Insert(new Character { AstroSign = "Aries", Gender = "Male" });
            db.Insert(new Character { AstroSign = "Taurus", Gender = "Female" });
            Debug.Log("Sample data inserted.");
        }
    }

    public List<Character> GetAllCharacters()
    {
        return db.Table<Character>().ToList();
    }

    private void LoadCharacters()
    {
        List<Character> characters = GetAllCharacters();

        foreach (Character character in characters)
        {
            Debug.Log($"ID: {character.CharacterId}, Sign: {character.AstroSign}, Gender: {character.Gender}");
        }
    }
}

public class Character {
    [PrimaryKey, AutoIncrement]
    public int CharacterId { get; set; }
    public string AstroSign { get; set; }
    public string Gender { get; set; }
}