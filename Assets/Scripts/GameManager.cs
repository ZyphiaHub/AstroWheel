using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private IGameState currentState;

    public static GameManager Instance { get; private set; }

    public const string LastCompletedIslandKey = "LastCompletedIsland";
    public const string TotalScoreKey = "TotalScore";
    //public const int PlayerIdKey = "PlayerId";
    public const string CharacterIdKey = "CharacterId";

    private int islandIndex; // Új változó az aktuális sziget indexének tárolására

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("GameManager inicializálva.");
        } else
        {
            Debug.LogWarning("Második GameManager példány törölve!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        

        //Debug.Log("Beléptél a Login Menübe");
        SceneManager.LoadScene("Login");

        
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
        

    }

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();
    }

    // Mentés: Utolsó teljesített sziget mentése
    public void SaveLastCompletedIsland(int islandIndex)
    {
        PlayerPrefs.SetInt(LastCompletedIslandKey, islandIndex);
        PlayerPrefs.Save(); // Azonnali mentés
    }

    // Betöltés: Utolsó teljesített sziget betöltése
    public int LoadLastCompletedIsland()
    {
        return PlayerPrefs.GetInt(LastCompletedIslandKey, 0); // Alapértelmezett érték: 0 (elsõ sziget)
    }

    // Ellenõrzés: Egy adott sziget teljesítve van-e
    public bool IsIslandCompleted(int islandIndex)
    {
        return islandIndex <= LoadLastCompletedIsland();
    }

    public void SaveTotalScore(int totalScore)
    {
        PlayerPrefs.SetInt(TotalScoreKey, totalScore);
        PlayerPrefs.Save(); // Azonnali mentés
    }

    // TotalScore betöltése
    public int LoadTotalScore()
    {
        return PlayerPrefs.GetInt(TotalScoreKey, 0); 
    }
    public bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    // PlayerId mentése
    /* public void SavePlayerId(int playerId)
     {
         PlayerPrefs.SetInt(PlayerIdKey, playerId);
         PlayerPrefs.Save(); 
     }*/
    public void SavePlayerId(int playerId)
    {
        PlayerPrefs.SetInt("PlayerId", playerId);
        PlayerPrefs.Save();
        Debug.Log("PlayerId saved: " + playerId);
    }

    // PlayerId betöltése
    public int LoadPlayerId()
    {
        // Példa: PlayerId betöltése PlayerPrefsbõl
        if (PlayerPrefs.HasKey("PlayerId"))
        {
            return PlayerPrefs.GetInt("PlayerId");
        } else
        {
            Debug.LogError("PlayerId not found in PlayerPrefs!");
            return 0; // Vagy dobj egy kivételt, ha szükséges
        }
    }
    

    // Puzzle megoldásának állapotának mentése
    public void SetPuzzleSolved(bool isSolved)
    {
        PlayerPrefs.SetInt($"Island_{islandIndex}_Solved", isSolved ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Puzzle megoldásának állapotának betöltése
    public bool IsPuzzleSolved(int islandIndex)
    {
        return PlayerPrefs.GetInt($"Island_{islandIndex}_Solved", 0) == 1;
    }

    // Aktuális sziget indexének lekérése
    public int GetCurrentIslandIndex()
    {
        return islandIndex; // vagy bármilyen más logika, ami visszaadja az aktuális sziget indexét
    }

    // Aktuális sziget indexének beállítása
    public void SetCurrentIslandIndex(int index)
    {
        islandIndex = index;
    }
   
}
