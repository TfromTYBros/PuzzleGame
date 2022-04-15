using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    UserStatus userStatus;
    public Text CountText;
    private int HitCount = 0;
    [SerializeField] private int LineIndex = 0;

    void Start()
    {
        userStatus = FindObjectOfType<UserStatus>();
        TextChange();
    }

    public void OnBallHit()
    {
        DecrementHitCount();
        TextChange();
        IsDestroy();
    }

    public void SetHitCount(int gameLevel)
    {
        int value = RandomSeed(gameLevel);
        //Debug.Log("SetHitCount : " + value);
        HitCount = value;
    }

    public int RandomSeed(int gameLevel)
    {
        if (gameLevel == 1)
        {
            return Random.Range(1, 3);
        }
        else if (gameLevel == 2)
        {
            return Random.Range(2, 4);
        }
        else if (gameLevel == 3)
        {
            return Random.Range(4, 6);
        }
        else if (gameLevel == 4)
        {
            return Random.Range(6, 8);
        }
        else if (gameLevel == 5)
        {
            return Random.Range(8, 10);
        }
        else if (gameLevel == 6)
        {
            return Random.Range(10, 12);
        }
        else if (gameLevel == 7)
        {
            return Random.Range(12, 14);
        }
        else if (gameLevel == 8)
        {
            return Random.Range(14, 16);
        }
        else if (gameLevel == 9)
        {
            return Random.Range(16, 18);
        }
        else if (gameLevel == 10)
        {
            return Random.Range(18, 20);
        }
        else
        {
            return Random.Range(20, 30);
        }
    }

    private void DecrementHitCount()
    {
        //Debug.Log("DecrementHitCount");
        HitCount--;
    }

    private void TextChange()
    {
        CountText.text = HitCount.ToString();
    }

    private void IsDestroy()
    {
        //Debug.Log("IsDestroy");
        if (HitCount == 0)
        {
            //Debug.Log("Destroy : " + this.gameObject.name);
            userStatus.ScoreChangeOnBlockBreak();
            userStatus.PlusBlockBreakPoint();
            userStatus.IsGameLevelUp();
            Destroy(this.gameObject);
        }
        else if (HitCount < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetLineIndex(int lineIndex)
    {
        LineIndex = lineIndex;
    }

    public int GetLineIndex()
    {
        return LineIndex;
    }

    public void MoveLine()
    {
        LineIndex++;
    }
}
