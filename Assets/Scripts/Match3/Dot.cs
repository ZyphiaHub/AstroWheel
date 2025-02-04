using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
   
    public bool isMatched = false;

    private Board board;
    public GameObject otherDot;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;


    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if (isMatched)
        {

            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            Color currentColor = mySprite.color;
            mySprite.color = new Color(0f, 1f, 0f, .2f);
        }

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move Towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .5f);
        } else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move Towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .5f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            

        } else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;

        }
    }

    private void OnMouseDown()
    {
        
            firstTouchPosition = Input.mousePosition;
            Debug.Log("first: " + firstTouchPosition);
        
    }

    private void OnMouseUp()
    {
        
        
            finalTouchPosition = Input.mousePosition;
            Debug.Log("final" + finalTouchPosition);
            CalculateAngle();
        
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) 
            * 180 / Mathf.PI ;   // <- makes it degree instead of rad
        
        Debug.Log("swipeangle:" + swipeAngle);
        MovePieces();   
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right Swipe
            otherDot = board.allDots[column + 1, row];
            
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;

        } else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up Swipe
            otherDot = board.allDots[column, row + 1];
            
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;

        } else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left Swipe
            otherDot = board.allDots[column - 1, row];
            
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        } else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down Swipe
            otherDot = board.allDots[column, row - 1];
            
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }

        
    }

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    
}
