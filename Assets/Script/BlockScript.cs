using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    UserStatus userStatus;
    Animator animator;
    public Text CountText;
    private int HitCount = 0;
    [SerializeField] private int LineIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        userStatus = FindObjectOfType<UserStatus>();
        TextChange();
    }

    public void OnBallHit()
    {
        animator.SetTrigger("Hit");
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
        //return Random.Range(gameLevel, gameLevel + 2);
        return gameLevel;
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
            userStatus.PlusBlockBreakPoint();
            userStatus.ChangeSlider(userStatus.GetBlockBreakPoint() % 10);
            userStatus.IsGameLevelUp();
            ThisBlockDestroy();
        }
        else if (HitCount < 0)
        {
            ThisBlockDestroy();
        }
    }

    private void ThisBlockDestroy()
    {
        GetComponent<Rigidbody2D>().
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return userStatus.delayToDestroy;
        Destroy(this.gameObject);
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
