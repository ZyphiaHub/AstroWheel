using System.IO;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour {
    void Start()
    {
        // Az adatb�zis f�jl el�r�si �tvonal�nak meghat�roz�sa
        string dbFilePath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");

        // Ha a f�jl m�g nem l�tezik a persistentDataPath mapp�ban, m�soljuk onnan
        if (!File.Exists(dbFilePath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, "GameDatabase.db");

            // A f�jl m�sol�sa a persistentDataPath mapp�ba
            File.Copy(sourcePath, dbFilePath);
            Debug.Log("Database copied to persistent data path.");
        } else
        {
            Debug.Log("Database already exists in persistent data path.");
        }

        // Ellen�rizz�k, hogy siker�lt-e a f�jl m�sol�sa
        Debug.Log("Database Path: " + dbFilePath);

        // Most itt m�r el�rheted az adatb�zist az EF Core vagy SQLite haszn�lat�val.
    }
}
