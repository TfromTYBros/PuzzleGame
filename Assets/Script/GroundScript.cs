using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    PuzzleGame puzzleGame;
    UserInput userInput;

    private void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        userInput = FindObjectOfType<UserInput>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            userInput.DicidePos(collision.transform.position.x);
            puzzleGame.IsStartCleanUp();
        }
    }
}
