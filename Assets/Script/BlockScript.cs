using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    UserStatus userStatus;
    Animator animator;
    public Text countText;
    private int hitCount = 0;
    [SerializeField] private int lineIndex = 0;
    //public GameObject[] edges;

    void Start()
    {
        animator = GetComponent<Animator>();
        userStatus = FindObjectOfType<UserStatus>();
        TextChange();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            OnBallHit();
        }
    }

    private int GetHitCount()
    {
        return hitCount;
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
        hitCount = value;
    }

    public int RandomSeed(int gameLevel)
    {
        return gameLevel;
    }

    private void DecrementHitCount()
    {
        //Debug.Log("DecrementHitCount");
        if (1 <= GetHitCount()) hitCount--;
    }

    private void TextChange()
    {
        countText.text = GetHitCount().ToString();
    }

    private void IsDestroy()
    {
        //Debug.Log("IsDestroy");
        if (hitCount == 0)
        {
            //Debug.Log("Destroy : " + this.gameObject.name);
            userStatus.PlusBlockBreakPoint();
            userStatus.ChangeSlider(userStatus.GetBlockBreakPoint() % 10);
            userStatus.IsGameLevelUp();
            ThisBlockDestroy();
        }
        else if (hitCount < 0)
        {
            ThisBlockDestroy();
        }
    }

    private void ThisBlockDestroy()
    {
        DisEdgeRigid();
        this.gameObject.transform.SetParent(userStatus.trashBox.transform);
        GoDestroyAnime();
        StartCoroutine(DelayDestroy());
    }

    private void DisEdgeRigid()
    {
        this.transform.GetComponent<Rigidbody2D>().simulated = false;
    }

    private void GoDestroyAnime()
    {
        animator.SetTrigger("Destroy");
    }

    private IEnumerator DelayDestroy()
    {
        yield return userStatus.delayToDestroy;
        Destroy(this.gameObject);
    }

    public void SetLineIndex(int Index)
    {
        lineIndex = Index;
    }

    public int GetLineIndex()
    {
        return lineIndex;
    }

    public void MoveLine()
    {
        lineIndex++;
    }
}
