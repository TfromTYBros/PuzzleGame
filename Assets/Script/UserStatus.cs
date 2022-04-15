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

    /**********GameLevel**********/
    [SerializeField] private int gameLevel = 1;

    /**********BlockBreak*********/
    [SerializeField] private int blockBreakPoint = 0;

    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        ChangeHaveBallCount(1);
    }

    /*****
     *Ball
     *****/
    public void ChangeHaveBallCount(int value)
    {
        haveBallCount = value;
        if (haveBallCount > 1000) haveBallCount = 999;
    }

    public int GetHaveBallCount()
    {
        return haveBallCount;
    }

    public void ChangeTextBallCount()
    {
        haveBallCountText.text = "BALL : " + GetHaveBallCount().ToString("000");
    }

    public void BallCountReset()
    {
        ChangeHaveBallCount(1);
        ChangeTextBallCount();
    }

    /******
     *Score
    *******/
    private void ChangeScore(int newScore)
    {
        score = newScore;
        if (score > 99999999) score = 9999999;
    }

    public int GetScore()
    {
        return score;
    }

    public void ScoreChangeOnBlockBreak()
    {
        ScoreCal();
        ScoreTextChange();
    }

    private void ScoreCal()
    {
        ChangeScore(GetGameLevel() * 1 + GetScore());
    }

    private void ScoreTextChange()
    {
        scoreText.text = "SCORE : " + score.ToString("00000000");
    }

    public void ScoreReset()
    {
        ChangeScore(0);
        ScoreTextChange();
    }

    /**********
     *GameLevel
     **********/

    public void ChangeGameLevel(int level)
    {
        gameLevel = level;
    }

    public int GetGameLevel()
    {
        return gameLevel;
    }
    public void IsGameLevelUp()
    {
        if ((GetBlockBreakPoint() % 10) == 0 && GetBlockBreakPoint() != 0)
        {
            GameLevelUp();
        }
    }
    private void GameLevelUp()
    {
        gameLevel++;
    }

    /***********
     *BlockBreak
     ***********/

    public void ChangeBlockBreakPoint(int point)
    {
        blockBreakPoint = point;
    }

    public int GetBlockBreakPoint()
    {
        return blockBreakPoint;
    }

    public void PlusBlockBreakPoint()
    {
        blockBreakPoint++;
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
