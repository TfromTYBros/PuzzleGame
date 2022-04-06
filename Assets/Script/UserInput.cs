using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    PuzzleGame puzzleGame;
    public Transform touchGroundPos;
    private readonly Vector3 ballPosOnStartGame = new Vector3(0.0f, -3.5f, 0.0f);
    public GameObject guideBall;

    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        
    }

    void Update()
    {
        if (puzzleGame.state == PuzzleGame.GameState.BALL_ANGLE)
        {
            MouseFollow();
        }
        else if (puzzleGame.state == PuzzleGame.GameState.BALL_ANGLE && Input.GetMouseButtonDown(0))
        {
            puzzleGame.Shot();
        }
    }

    /************
     *OnStartGame
     ************/

    public void EnaGuideBall()
    {
        guideBall.SetActive(true);
    }

    public void FirstDicidePos()
    {
        guideBall.transform.position = ballPosOnStartGame;
    }

    /************
    *OnBALL_ANGLE
    *************/

    private void MouseFollow()
    {
        Vector2 mouse = guideBall.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        guideBall.transform.rotation = Quaternion.FromToRotation(Vector2.up, mouse);
    }

    /**********
    *OnMOVETIME
    ***********/

    private void DicidePos()
    {
        
    }
}
