using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private IGameState currentState;

    public static GameManager Instance { get; private set; }

    public const string LastCompletedIslandKey = "LastCompletedIsland";
    public const string TotalScoreKey = "TotalScore";
    public const string PlayerIdKey = "PlayerId";
    public const string CharacterIdKey = "CharacterId";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //manuális teszteléshez
        PlayerPrefs.SetInt(LastCompletedIslandKey, 3); // Például a második sziget teljesítve
        PlayerPrefs.Save();

        Debug.Log("Beléptél a Login Menübe");
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
        return PlayerPrefs.GetInt(LastCompletedIslandKey, 3); // Alapértelmezett érték: 0 (elsõ sziget)
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
        return PlayerPrefs.GetInt(TotalScoreKey, 0); // Alapértelmezett érték: 0
    }

    // PlayerId mentése
    public void SavePlayerId(string playerId)
    {
        PlayerPrefs.SetString(PlayerIdKey, playerId);
        PlayerPrefs.Save(); // Azonnali mentés
    }

    // PlayerId betöltése
    public string LoadPlayerId()
    {
        return PlayerPrefs.GetString(PlayerIdKey, ""); // Alapértelmezett érték: üres string
    }

}
