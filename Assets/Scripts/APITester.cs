using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APITester : MonoBehaviour {
    private void Start()
    {
        StartCoroutine(FetchAndDisplayPlayers());
    }

    private IEnumerator FetchAndDisplayPlayers()
    {
        yield return APIClient.Instance.GetPlayers(
            onSuccess: players =>
            {
                foreach (var player in players)
                {
                    Debug.Log($"Player ID: {player.playerId}, Name: {player.playerName}, Character: {player.characterName}");
                }
            },
            onError: error =>
            {
                Debug.LogError("Error fetching players: " + error);
            }
        );
    }
}