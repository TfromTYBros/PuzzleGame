using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    PuzzleGame puzzleGame;
    /*********Pos*********/
    public Transform touchGroundPos;
    private readonly Vector3 ballPosOnStartGame = new Vector3(0.0f, -3.5f, -5.0f);

    /********Rotate********/
    public GameObject guideBall;
    private readonly float ROTATE_LIMIT = 1.0f;
    private readonly float MOUSE_LIMIT = 1.5f;
    [SerializeField] float way = 0.0f;

    /*********Parts********/
    private readonly float NOTHING = 0.0f;
    private readonly float REVERSE = -1.0f;

    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (puzzleGame.state == PuzzleGame.GameState.BALL_ANGLE)
            {
                //Debug.Log("Click from BALL_ANGLE");
                DisGuideBall();
                puzzleGame.Shot();
                puzzleGame.StartMovingNow();
            }
        }
        if (puzzleGame.state == PuzzleGame.GameState.BALL_ANGLE)
        {
            MouseFollow();
        }
    }

    /******************
     *ゲッターセッター
     *****************/

    public float GetWay()
    {
        return way;
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
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.y = MOUSE_LIMIT;
        if (mouse.x >= ROTATE_LIMIT)
        {
            mouse.x = ROTATE_LIMIT;
        }
        if (mouse.x <= ROTATE_LIMIT * REVERSE)
        {
            mouse.x = ROTATE_LIMIT * REVERSE;
        }
        Debug.Log(mouse);
        guideBall.transform.rotation = Quaternion.FromToRotation(Vector2.up, mouse);
        way = mouse.x;
    }

    private void DisGuideBall()
    {
        guideBall.SetActive(false);
    }

    /**********
    *OnMOVETIME
    ***********/

    private void DicidePos()
    {
        
    }
}
