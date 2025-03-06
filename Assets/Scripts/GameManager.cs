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
        //manu�lis tesztel�shez
        PlayerPrefs.SetInt(LastCompletedIslandKey, 3); // P�ld�ul a m�sodik sziget teljes�tve
        PlayerPrefs.Save();

        Debug.Log("Bel�pt�l a Login Men�be");
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
        return PlayerPrefs.GetInt(LastCompletedIslandKey, 3); // Alap�rtelmezett �rt�k: 0 (els� sziget)
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
        return PlayerPrefs.GetInt(TotalScoreKey, 0); // Alap�rtelmezett �rt�k: 0
    }

    // PlayerId ment�se
    public void SavePlayerId(string playerId)
    {
        PlayerPrefs.SetString(PlayerIdKey, playerId);
        PlayerPrefs.Save(); // Azonnali ment�s
    }

    // PlayerId bet�lt�se
    public string LoadPlayerId()
    {
        return PlayerPrefs.GetString(PlayerIdKey, ""); // Alap�rtelmezett �rt�k: �res string
    }

}
