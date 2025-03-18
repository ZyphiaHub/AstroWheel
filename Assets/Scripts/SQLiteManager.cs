using UnityEngine;
using SQLite4Unity3d; 
using System.IO;
using System.Linq;


public class LocalDatabaseManager : MonoBehaviour {
    public static LocalDatabaseManager Instance;
    private void Awake()
    {
        // Singleton minta implementációja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ne törlõdjön a scene váltáskor
        } else
        {
            Destroy(gameObject); // Ha már van példány, töröld ezt
        }
    }

    private SQLiteConnection connection;

    void Start()
    {
        InitializeDatabase();
    }

    void InitializeDatabase()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, "game_data.db");
        string destinationPath = Path.Combine(Application.persistentDataPath, "game_data.db");

        // Ha a célfájl még nem létezik, másold át a fájlt
        if (!File.Exists(destinationPath))
        {
            try
            {
                File.Copy(sourcePath, destinationPath);
                Debug.Log("Database copied from StreamingAssets to persistentDataPath.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to copy database: " + ex.Message);
                return;
            }
        }

        // Adatbázis kapcsolat létrehozása
        try
        {
            connection = new SQLiteConnection(destinationPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            // Táblák létrehozása, ha még nem léteznek
            connection.CreateTable<PlayerTbl>();
            connection.CreateTable<CharacterTbl>();
            connection.CreateTable<InventoryTbl>();
            connection.CreateTable<IslandTbl>();
            connection.CreateTable<MaterialTbl>();

            Debug.Log("Database initialized successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing database: " + ex.Message);
        }
    }

    public void SavePlayerData(int playerId, int totalScore)
    {
        // Adatok beszúrása vagy frissítése
        var playerData = new PlayerTbl
        {
            playerId = 1, // Egyedi azonosító
            totalScore = totalScore
            
        };

        connection.InsertOrReplace(playerData);
    }

    public void LoadPlayerData()
    {
        // Adatok lekérése
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

    // Adatmodell a PlayerTbl táblához
    public class PlayerTbl {
        [PrimaryKey] public int playerId { get; set; }
        [MaxLength(255)] public string playerName { get; set; } 
        public string userId { get; set; } //email akart lenni sztem
        public string playerPassword { get; set; }
        public int? characterId { get; set; } //kar kép id
        public int islandId { get; set; } //utolsó teljesített sziget
        public int totalScore { get; set; }
        public int lastLogin { get; set; }
        public int createdAt { get; set; }
        public int isActive { get; set; }

    }

    public class CharacterTbl {
        [PrimaryKey] public int CharacterId { get; set; } 
        [MaxLength(50)] public string AstroSign { get; set; } 
        [MaxLength(10)] public string Gender { get; set; }
        public int CharacterIndex { get; set; } // Karakter indexe
    }

    public class InventoryTbl {
        [PrimaryKey] public int InventoryId { get; set; }
        public int MaterialId { get; set; }
        public int MatQuantity  { get; set; }
        }

    public class IslandTbl {
        [PrimaryKey] public int IslandId    { get; set; }
        public string AstroSign { get; set; }
        public int MaterialId { get; set; }
    }
    public class MaterialTbl {
        [PrimaryKey] public int MaterialId { get; set; }
        public string EnglishName { get; set; }
        public string WitchName { get; set; }
        public string LatinName { get; set; }
        public string Description { get; set; }
        public string Icon {  get; set; }
    }


}