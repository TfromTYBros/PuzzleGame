using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    PuzzleGame puzzleGame;
    /*********Pos*********/
    public Transform touchGroundPos;
    private readonly Vector3 ballPosOnStartGame = new Vector3(0.0f, -3.5f, -5.0f);
    [SerializeField] private bool groundTouched = false;
    public GameObject ballShadow;

    /********Rotate********/
    public GameObject guideBall;
    private readonly float ROTATE_LIMIT = 1.0f;
    private readonly float MOUSE_LIMIT = 1.5f;
    [SerializeField] float way = 0.0f;

    /*********GameOver********/
    public GameObject gameOverPanel;

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
                BoolGroundTouchReset();
                puzzleGame.Shot();
                puzzleGame.StartMovingNow();
            }
            else if (puzzleGame.state == PuzzleGame.GameState.GAMEOVER)
            {
                DisGameOverPanel();
                puzzleGame.ReStart();
            }
        }

        //ずっと動くメソッド
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

    private void GroundTouch()
    {
        groundTouched = true;
    }

    public void BoolGroundTouchReset()
    {
        groundTouched = false;
    }

    private bool BoolGetGroundTouch()
    {
        return groundTouched;
    }

    public void EnaGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    private void DisGameOverPanel()
    {
        gameOverPanel.SetActive(false);
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

    public void DisBallShadow()
    {
        ballShadow.SetActive(false);
    }

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
        //Debug.Log(mouse);
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

    public void DicidePos(float x)
    {
        if (!BoolGetGroundTouch())
        {
            Vector3 touchPos = new Vector3(x, touchGroundPos.position.y, touchGroundPos.position.z);
            touchGroundPos.position = touchPos;

            //ballShadow
            ballShadow.transform.position = touchPos;
            EnaBallShadow();

            //締め切り
            GroundTouch();
        }
    }

    private void EnaBallShadow()
    {
        ballShadow.SetActive(true);
    }

    /**********
    *OnGAMEOVER
    ***********/

    public void ResetPos()
    {
        touchGroundPos.position = ballPosOnStartGame;
    }
}
