using UnityEngine;
using SQLite4Unity3d; 
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;

public class LocalDatabaseManager : MonoBehaviour {
    private SQLiteConnection connection;

    void Start()
    {
        InitializeDatabase();
    }

    void InitializeDatabase()
    {
        string databasePath = Path.Combine(Application.persistentDataPath, "gamedatabase.db");

        // Adatb�zis kapcsolat l�trehoz�sa
        connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // T�bla l�trehoz�sa, ha m�g nem l�tezik
        //connection.CreateTable<PlayerData>();
    }

    /*public void SavePlayerData(int playerId, int totalScore)
    {
        // Adatok besz�r�sa vagy friss�t�se
        var playerData = new PlayerTbl
        {
            playerId = 1, // Egyedi azonos�t�
            totalScore = totalScore
            
        };

        connection.InsertOrReplace(playerData);
    }*/

    public void LoadPlayerData()
    {
        // Adatok lek�r�se
        var playerData = connection.Table<PlayerTbl>().FirstOrDefault(p => p.playerId == 1);

        if (playerData != null)
        {
            Debug.Log($"Loaded Data - {playerData.playerName},  {playerData.totalScore}");
        } else
        {
            Debug.Log("No player data found.");
        }
    }

    void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
        }
    }

    // Adatmodell a PlayerTbl t�bl�hoz
    public class PlayerTbl {
        [PrimaryKey] public int playerId { get; set; }
        [Required] [MaxLength(255)] public string playerName { get; set; } 
        [Required] public string userId { get; set; } //email akart lenni sztem
        public string playerPassword { get; set; }
        public int? characterId { get; set; } //kar k�p id
        public int islandId { get; set; } //utols� teljes�tett sziget
        public int totalScore { get; set; }
        public int lastLogin { get; set; }
        [Required] public int createdAt { get; set; }
        public int isActive { get; set; }

    }

    public class CharacterTbl {
        [PrimaryKey] public int CharacterId { get; set; } 
        [MaxLength(50)] public string AstroSign { get; set; } 
        [MaxLength(10)] public string Gender { get; set; }
        public int CharacterIndex { get; set; } // Karakter indexe
    }

    public class InventoryTbl {
        }

    public class IslandTbl { }
    public class MaterialTbl { }


}