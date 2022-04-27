using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    UserStatus userStatus;
    public Text description;

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
            Destroy(this.gameObject);
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
