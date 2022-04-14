using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatus : MonoBehaviour
{
    [SerializeField] private int score = 0;
    public Text scoreText;
    [SerializeField] private int haveBallCount = 0;
    
    void Start()
    {
        ChangeHaveBallCount(1);
    }

    void Update()
    {
        
    }

    /******************
     *ゲッターセッター
     *****************/
    private void ChangeScore(int newScore)
    {
        score = newScore;
    }

    public int GetScore()
    {
        return score;
    }

    private void ScoreTextChange()
    {
        scoreText.text = "SCORE : " + score.ToString("00000000");
    }

    public void ChangeHaveBallCount(int value)
    {
        haveBallCount = value;
    }

    public int GetHaveBallCount()
    {
        return haveBallCount;
    }

    /************
     *ItemMethod
     ************/

    public void Item_Ballx2()
    {
        ChangeHaveBallCount(GetHaveBallCount() * 2);
    }

    public void Item_BallPlus()
    {
        ChangeHaveBallCount(GetHaveBallCount() + 1);
    }
}
