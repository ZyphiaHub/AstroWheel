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

    private int islandIndex; // �j v�ltoz� az aktu�lis sziget index�nek t�rol�s�ra

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("GameManager inicializ�lva.");
        } else
        {
            Debug.LogWarning("M�sodik GameManager p�ld�ny t�r�lve!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        

        //Debug.Log("Bel�pt�l a Login Men�be");
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

    // Ment�s: Utols� teljes�tett sziget ment�se
    public void SaveLastCompletedIsland(int islandIndex)
    {
        PlayerPrefs.SetInt(LastCompletedIslandKey, islandIndex);
        PlayerPrefs.Save(); // Azonnali ment�s
    }

    // Bet�lt�s: Utols� teljes�tett sziget bet�lt�se
    public int LoadLastCompletedIsland()
    {
        return PlayerPrefs.GetInt(LastCompletedIslandKey, 0); // Alap�rtelmezett �rt�k: 0 (els� sziget)
    }

    // Ellen�rz�s: Egy adott sziget teljes�tve van-e
    public bool IsIslandCompleted(int islandIndex)
    {
        return islandIndex <= LoadLastCompletedIsland();
    }

    public void SaveTotalScore(int totalScore)
    {
        PlayerPrefs.SetInt(TotalScoreKey, totalScore);
        PlayerPrefs.Save(); // Azonnali ment�s
    }

    // TotalScore bet�lt�se
    public int LoadTotalScore()
    {
        return PlayerPrefs.GetInt(TotalScoreKey, 0); 
    }
    public bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    // PlayerId ment�se
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

    // PlayerId bet�lt�se
    public int LoadPlayerId()
    {
        // P�lda: PlayerId bet�lt�se PlayerPrefsb�l
        if (PlayerPrefs.HasKey("PlayerId"))
        {
            return PlayerPrefs.GetInt("PlayerId");
        } else
        {
            Debug.LogError("PlayerId not found in PlayerPrefs!");
            return 0; // Vagy dobj egy kiv�telt, ha sz�ks�ges
        }
    }
    

    // Puzzle megold�s�nak �llapot�nak ment�se
    public void SetPuzzleSolved(bool isSolved)
    {
        PlayerPrefs.SetInt($"Island_{islandIndex}_Solved", isSolved ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Puzzle megold�s�nak �llapot�nak bet�lt�se
    public bool IsPuzzleSolved(int islandIndex)
    {
        return PlayerPrefs.GetInt($"Island_{islandIndex}_Solved", 0) == 1;
    }

    // Aktu�lis sziget index�nek lek�r�se
    public int GetCurrentIslandIndex()
    {
        return islandIndex; // vagy b�rmilyen m�s logika, ami visszaadja az aktu�lis sziget index�t
    }

    // Aktu�lis sziget index�nek be�ll�t�sa
    public void SetCurrentIslandIndex(int index)
    {
        islandIndex = index;
    }
   
}
