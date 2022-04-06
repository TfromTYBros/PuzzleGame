using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    PuzzleGame puzzleGame;
    private void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            //ˆÊ’u‹L‰¯


            puzzleGame.IsStartCleanUp();
        }
    }
}
