using System.IO;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour {
    void Start()
    {
        // Az adatbázis fájl elérési útvonalának meghatározása
        string dbFilePath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");

        // Ha a fájl még nem létezik a persistentDataPath mappában, másoljuk onnan
        if (!File.Exists(dbFilePath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, "GameDatabase.db");

            // A fájl másolása a persistentDataPath mappába
            File.Copy(sourcePath, dbFilePath);
            Debug.Log("Database copied to persistent data path.");
        } else
        {
            Debug.Log("Database already exists in persistent data path.");
        }

        // Ellenõrizzük, hogy sikerült-e a fájl másolása
        Debug.Log("Database Path: " + dbFilePath);

        // Most itt már elérheted az adatbázist az EF Core vagy SQLite használatával.
    }
}
