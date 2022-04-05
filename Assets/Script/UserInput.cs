using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    PuzzleGame puzzleGame;
    public GameObject GuideBox;
    public Transform BallPos;
    public GameObject BallGuide;

    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (puzzleGame.state == PuzzleGame.GameState.BALL_POS)
            {
                DicidePos();
                puzzleGame.StartBallAngle();
            }
            else if (puzzleGame.state == PuzzleGame.GameState.BALL_ANGLE)
            {

            }
        }
    }

    private void DicidePos()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log(mouse);
        BallPos.position = mouse;
    }
}
