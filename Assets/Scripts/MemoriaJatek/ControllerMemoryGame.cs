using System.Collections.Generic;
using UnityEngine;
using static MemoryCard;

public class ControllerMemoryGame : MonoBehaviour {
    public GameObject cardPrefab; // A kártya prefab
    public Sprite[] cardFronts;   // A kártyák elõlapjai (képek)
    public Transform cardParent; // A Grid Layout Group helye

    private List<GameObject> cards = new List<GameObject>();

    private void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        // Képek duplázása a párosításokhoz
        List<Sprite> cardDeck = new List<Sprite>();
        foreach (var front in cardFronts)
        {
            cardDeck.Add(front);
            cardDeck.Add(front);
        }

        // Keverjük meg a kártyákat
        Shuffle(cardDeck);

        // Kártyák létrehozása
        foreach (var frontImage in cardDeck)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);
            var memoryCard = newCard.GetComponent<MemoryCard>();
            memoryCard.frontImage = frontImage; 
            memoryCard.ShowBack();             // Kezdõ állapotban a hátlap látszódjon
            cards.Add(newCard);
        }
    }

    // Egyszerû keverési algoritmus (Fisher-Yates Shuffle)
    private void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
