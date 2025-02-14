using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoController : MonoBehaviour {
    [SerializeField]
    private Sprite backImage;

    public Sprite[] cardFaces;
    private int score = 0;
    private int remGuesses = 24;
    [SerializeField] private TMP_Text scoreText;  // Pontszám UI
    [SerializeField] private TMP_Text guessesText;

    public List<Sprite> cardPairs = new List<Sprite>();

    public List<Button> cardList = new List<Button>();

    private bool firstPick, secondPick;
    private int countPicks;
    
    private int countCorrectPicks;
    private int gamePicks;
    private int firstPickIndex, secondPickIndex;
    private string firstPickPuzzle, secondPickPuzzle;


    void Start()
    {
        GetButtons();
        AddListeners();
        AddCardPairs();
        Shuffle(cardPairs);
        gamePicks = cardPairs.Count / 2;

        UpdateUI();
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("CardButton");

        for (int i = 0; i < objects.Length; i++)
        {
            cardList.Add(objects[i].GetComponent<Button>());
            cardList[i].image.sprite = backImage;
        }

    }

    void AddCardPairs()
    {
        int looper = cardList.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2) { index = 0; }
            cardPairs.Add(cardFaces[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button card in cardList)
        {
            card.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstPick)
        {
            firstPick = true;
            firstPickIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstPickPuzzle = cardPairs[firstPickIndex].name;

            cardList[firstPickIndex].image.sprite = cardPairs[firstPickIndex];

        } else if (!secondPick)
        {
            secondPick = true;
            secondPickIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondPickPuzzle = cardPairs[secondPickIndex].name;

            cardList[secondPickIndex].image.sprite = cardPairs[secondPickIndex];

            StartCoroutine(CheckCardMatch());


        }
    }

    IEnumerator CheckCardMatch()
    {
        yield return new WaitForSeconds(.7f);
        remGuesses--;

        if (firstPickPuzzle == secondPickPuzzle)
        {
            yield return new WaitForSeconds(.5f);
            cardList[firstPickIndex].interactable = false;
            cardList[secondPickIndex].interactable = false;

            cardList[firstPickIndex].image.color = new Color(0, 0, 0, 0);
            cardList[secondPickIndex].image.color = new Color(0, 0, 0, 0);

            //pontozás
            score += Mathf.Max(2, 14 - countPicks);
            Debug.Log("pontszám:" + score);

            
            CheckIfTheGameIsFinished();

        } else
        {
            cardList[firstPickIndex].image.sprite = backImage;
            cardList[secondPickIndex].image.sprite = backImage;
        }

        yield return new WaitForSeconds(.5f);
        firstPick = secondPick = false;

        Debug.Log("remaining guesses" + remGuesses);
        UpdateUI();
        if (remGuesses > 0)
        {
            EndGame(false);
        } else {
            Debug.Log("Vesztettél");
            }
    }

    void CheckIfTheGameIsFinished()
    {
        countCorrectPicks++;

        if (countCorrectPicks == gamePicks)
        {
            Debug.Log("game finished");
            Debug.Log(countPicks);
            Debug.Log("összpontszám:" + score);

            EndGame(true);
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void EndGame(bool won)
    {
        if (won)
        {
            Debug.Log("Nyertél! A játéknak vége");
        } 
        
    }
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        guessesText.text = "Remaining Guesses: " + remGuesses;
        Debug.Log("update ui");
        }
}
