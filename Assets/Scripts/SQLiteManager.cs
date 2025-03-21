using UnityEngine;
using SQLite4Unity3d; 
using System.IO;
using System.Linq;
using System.Collections.Generic;


public class LocalDatabaseManager : MonoBehaviour {
    public static LocalDatabaseManager Instance { get; private set; }
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

        // Ellenõrizzük, hogy a forrásfájl létezik-e
        if (!File.Exists(sourcePath))
        {
            Debug.LogError("Source database file not found in StreamingAssets.");
            return;
        }

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
            connection.CreateTable<CraftedInventoryTbl>();

            Debug.Log("Database initialized successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing database: " + ex.Message);
        }
    }

    public void SavePlayerData(int playerId, string playerName, string userId, int characterId,
        int totalScore, int inventoryId, int lastCompletedIsland)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return;
        }

        var playerData = new PlayerTbl
        {
            playerId = playerId,
            playerName = playerName ?? string.Empty,
            userId = userId ?? string.Empty,
            characterId = characterId,
            totalScore = totalScore,
            inventoryId = inventoryId,
            islandId = lastCompletedIsland,
            isActive = 1,
            playerPassword = string.Empty,
            lastLogin = 0,
            createdAt = 0
        };

        try
        {
            connection.InsertOrReplace(playerData);
            Debug.Log("Player data saved to SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving player data: " + ex.Message);
        }
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
    // Inventory mentése az adatbázisba
    public void SaveInventoryData(int inventoryId, Dictionary<PlantDatabase.Item, int> inventoryItems)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return;
        }

        try
        {
            foreach (var entry in inventoryItems)
            {
                var existingRecord = connection.Table<InventoryTbl>()
                                            .FirstOrDefault(x => x.InventoryId == inventoryId && x.MaterialId == entry.Key.plantId);

                if (existingRecord != null)
                {
                    // Ha már létezik a rekord, frissítsd a mennyiséget
                    existingRecord.MatQuantity = entry.Value;
                    connection.Update(existingRecord);
                } else
                {
                    // Ha nem létezik, hozz létre új rekordot
                    var inventoryData = new InventoryTbl
                    {
                        InventoryId = inventoryId,
                        MaterialId = entry.Key.plantId,
                        MatQuantity = entry.Value
                    };

                    connection.Insert(inventoryData);
                }
            }

            Debug.Log("Inventory data saved to SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving inventory data: " + ex.Message);
        }
    }

    // Inventory betöltése az adatbázisból
    public Dictionary<int, int> LoadInventoryData(int inventoryId)
    {
        var inventoryData = new Dictionary<int, int>();

        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return inventoryData;
        }

        try
        {
            var items = connection.Table<InventoryTbl>()
                                  .Where(x => x.InventoryId == inventoryId)
                                  .ToList();

            foreach (var item in items)
            {
                inventoryData[item.MaterialId] = item.MatQuantity;
            }

            Debug.Log("Inventory data loaded from SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading inventory data: " + ex.Message);
        }

        return inventoryData;
    }
    public void SaveCraftedInventoryData(int inventoryId, Dictionary<ItemDatabase.Item, int> craftedItems)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return;
        }

        try
        {
            foreach (var entry in craftedItems)
            {
                var existingRecord = connection.Table<CraftedInventoryTbl>()
                                              .FirstOrDefault(x => x.InventoryId == inventoryId && x.ItemId == entry.Key.itemId);

                if (existingRecord != null)
                {
                    // Ha már létezik a rekord, frissítsd a mennyiséget
                    existingRecord.Quantity = entry.Value;
                    connection.Update(existingRecord);
                } else
                {
                    // Ha nem létezik, hozz létre új rekordot
                    var craftedInventoryData = new CraftedInventoryTbl
                    {
                        InventoryId = inventoryId,
                        ItemId = entry.Key.itemId,
                        Quantity = entry.Value
                    };

                    connection.Insert(craftedInventoryData);
                }
            }

            Debug.Log("Crafted inventory data saved to SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving crafted inventory data: " + ex.Message);
        }
    }
    public Dictionary<int, int> LoadCraftedInventoryData(int inventoryId)
    {
        var craftedInventoryData = new Dictionary<int, int>();

        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return craftedInventoryData;
        }

        try
        {
            var items = connection.Table<CraftedInventoryTbl>()
                                  .Where(x => x.InventoryId == inventoryId)
                                  .ToList();

            foreach (var item in items)
            {
                craftedInventoryData[item.ItemId] = item.Quantity;
            }

            Debug.Log("Crafted inventory data loaded from SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading crafted inventory data: " + ex.Message);
        }

        return craftedInventoryData;
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
        [MaxLength(255)] public string playerName { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
        public string playerPassword { get; set; } = string.Empty;
        public int characterId { get; set; } = 0;
        public int islandId { get; set; } = 0;
        public int inventoryId { get; set; } = 0;
        public int totalScore { get; set; } = 0;
        public int lastLogin { get; set; } = 0;
        public int createdAt { get; set; } = 0;
        public int isActive { get; set; } = 0;
    }

    public class CharacterTbl {
        [PrimaryKey] public int CharacterId { get; set; } 
        [MaxLength(50)] public string AstroSign { get; set; } 
        [MaxLength(10)] public string Gender { get; set; }
        public int CharacterIndex { get; set; } // Karakter indexe
    }

    public class InventoryTbl {
        [PrimaryKey] [AutoIncrement] public int RecordId { get; set; }
        public int InventoryId { get; set; }
        public int MaterialId { get; set; }
        public int MatQuantity  { get; set; }
        }
    public class CraftedInventoryTbl {
        [PrimaryKey, AutoIncrement] public int RecordId { get; set; }
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
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