using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatus : MonoBehaviour
{
    PuzzleGame puzzleGame;
    [SerializeField] private int score = 0;
    public Text scoreText;
    [SerializeField] private int haveBallCount = 0;
    public Text haveBallCountText;
    
    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        ChangeHaveBallCount(1);
    }

    /******************
     *ゲッターセッター
     *****************/
    private void ChangeScore(int newScore)
    {
        score = newScore;
        if (score > 99999999) score = 9999999;
    }

    public int GetScore()
    {
        return score;
    }

    public void ChangeHaveBallCount(int value)
    {
        haveBallCount = value;
        if (haveBallCount > 1000) haveBallCount = 999;
    }

    public int GetHaveBallCount()
    {
        return haveBallCount;
    }

    /***************
     *BallTextChange
     ***************/

    public void ChangeTextBallCount()
    {
        haveBallCountText.text = "BALL : " + GetHaveBallCount().ToString("000");
    }

    /************
     *ScoreChange
     ************/

    public void ScoreChangeOnBlockBreak()
    {
        ScoreCal();
        ScoreTextChange();
    }

    private void ScoreCal()
    {
        ChangeScore(puzzleGame.GetGameLevel() * 1 + GetScore());
    }

    private void ScoreTextChange()
    {
        scoreText.text = "SCORE : " + score.ToString("00000000");
    }

    /******
     *Items
     ******/

    public void ItemSelect(int index)
    {
        if (index == 4) Item_Ballx2();
        else Item_BallPlus();
    }

    private void Item_Ballx2()
    {
        ChangeHaveBallCount(GetHaveBallCount() * 2);
    }
    
    private void Item_BallPlus()
    {
        ChangeHaveBallCount(GetHaveBallCount() + 1);
    }
}
