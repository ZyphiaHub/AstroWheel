using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{
    wait,
    move
}


public class Board : MonoBehaviour {


    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public GameObject[] dots;
    public GameObject tilePrefab;
    
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    



	// Use this for initialization
	void Start () {
        
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
	}
	
    private void SetUp(){
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition,Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "tile:( " + i + ", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "dot:( " + i + ", " + j + " )";
                allDots[i, j] = dot;







            }

        }
    }

    



}
