using System.Collections.Generic;
using UnityEngine;
using static MemoryCard;

public class ControllerMemoryGame : MonoBehaviour {
    public GameObject cardPrefab; // A k�rtya prefab
    public Sprite[] cardFronts;   // A k�rty�k el�lapjai (k�pek)
    public Transform cardParent; // A Grid Layout Group helye

    private List<GameObject> cards = new List<GameObject>();

    private void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        // K�pek dupl�z�sa a p�ros�t�sokhoz
        List<Sprite> cardDeck = new List<Sprite>();
        foreach (var front in cardFronts)
        {
            cardDeck.Add(front);
            cardDeck.Add(front);
        }

        // Keverj�k meg a k�rty�kat
        Shuffle(cardDeck);

        // K�rty�k l�trehoz�sa
        foreach (var frontImage in cardDeck)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);
            var memoryCard = newCard.GetComponent<MemoryCard>();
            memoryCard.frontImage = frontImage; 
            memoryCard.ShowBack();             // Kezd� �llapotban a h�tlap l�tsz�djon
            cards.Add(newCard);
        }
    }

    // Egyszer� kever�si algoritmus (Fisher-Yates Shuffle)
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
