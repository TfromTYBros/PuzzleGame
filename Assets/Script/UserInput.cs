using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    PuzzleGame puzzleGame;
    UserStatus userStatus;
    /*********Pos*********/
    public Transform touchGroundPos;
    public Transform touchGroundPosTemp;
    private readonly Vector3 ballPosOnStartGame = new Vector3(0.0f, -3.5f, -5.0f);
    [SerializeField] private bool groundTouched = false;
    public GameObject ballShadow;

    /********Rotate********/
    public GameObject guideBall;
    private readonly float ROTATE_LIMIT = 1.0f;
    private readonly float MOUSE_LIMIT = 1.5f;
    [SerializeField] float way = 0.0f;

    /*********GAMEOVER********/
    public GameObject gameOverPanel;

    /********GAMESET**********/
    public GameObject gameSetPanel;

    /*********Parts********/
    private readonly float REVERSE = -1.0f;

    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        userStatus = FindObjectOfType<UserStatus>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (puzzleGame.state == PuzzleGame.GameState.BALL_ANGLE)
            {
                //Debug.Log("Click from BALL_ANGLE");
                DisGuideBall();
                userStatus.DisHaveBallCountText();
                BoolGroundTouchReset();
                puzzleGame.Shot();
                puzzleGame.StartMovingNow();
            }
            else if (puzzleGame.state == PuzzleGame.GameState.GAMEOVER)
            {
                DisGameOverPanel();
                puzzleGame.ReStart(0);
            }
            else if (puzzleGame.state == PuzzleGame.GameState.GAMESET && IsEnaGameSetPanel())
            {
                DisGameSetPanel();
                puzzleGame.ReStart(1);
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

    public void EnaGameSetPanel()
    {
        //Debug.Log("EnaGameSetPanel");
        gameSetPanel.SetActive(true);
    }

    private void DisGameSetPanel()
    {
        gameSetPanel.SetActive(false);
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
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - guideBall.transform.position;
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
            touchGroundPosTemp.position = touchPos;

            //ballShadow
            ballShadow.transform.position = touchGroundPosTemp.position;
            EnaBallShadow();

            //ballCountText
            userStatus.DicideBallCountTextPos(touchPos.x);

            //締め切り
            GroundTouch();
        }
    }

    private void EnaBallShadow()
    {
        ballShadow.SetActive(true);
    }
    public void ApplyDicidePos()
    {
        guideBall.transform.position = touchGroundPosTemp.position;
    }

    /**********
    *OnGAMEOVER
    ***********/

    public void ResetTouchRroundPos()
    {
        touchGroundPos.position = ballPosOnStartGame;
    }

    /**********
     *OnGAMESET
     **********/

    private bool IsEnaGameSetPanel()
    {
        return gameSetPanel.activeSelf;
    }
}
