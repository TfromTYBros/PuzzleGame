using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    UserStatus userStatus;
    public Text description;
    public Animator animator;

    /********Line********/
    [SerializeField] private int LineIndex = 0;

    void Start()
    {
        userStatus = FindObjectOfType<UserStatus>();
        TextChange();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Buffer();
            userStatus.ChangeTextHaveBallCount();
            ThisItemDestroy();
        }
    }

    private void TextChange()
    {
        description.text = GetItemStatus(userStatus.GetGameLevel());
    }

    /**********
     *BuffŒˆ‚ß
     **********/

    private void Buffer()
    {
        userStatus.ItemSelect();
    }

    public string GetItemStatus(int gameLevel)
    {
        return gameLevel == 5 ? "x2" : "+1";
    }

    /**********
     *Animator
     **********/

    private void ThisItemDestroy()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        userStatus.moveTrashBoxs[GetLineIndex()].transform.position = this.transform.position;
        this.gameObject.transform.SetParent(userStatus.moveTrashBoxs[GetLineIndex()].transform);
        GoDestroyAnime();
        StartCoroutine(DelayDestroy());
    }

    private void GoDestroyAnime()
    {
        animator.SetTrigger("Start");
    }

    private IEnumerator DelayDestroy()
    {
        yield return userStatus.delayToItemDestroy;
        Destroy(this.gameObject);
    }

    /**********
     *LineŠÖ˜A
     **********/

    public void MoveLine()
    {
        LineIndex++;
    }

    public int GetLineIndex()
    {
        return LineIndex;
    }

    public void SetLineIndex(int index)
    {
        LineIndex = index;
    }
}
