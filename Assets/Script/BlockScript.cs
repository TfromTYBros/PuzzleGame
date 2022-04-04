using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    public Text CountText;
    private int HitCount = 0;
    [SerializeField] private int LineIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        //SetHitCount(1);
        TextChange();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            DecrementHitCount();
            TextChange();
            IsDestroy();
        }
    }

    public void SetHitCount(int gameLevel)
    {
        int value = RandomSeed(gameLevel);
        Debug.Log("SetHitCount : " + value);
        HitCount = value;
    }

    public int RandomSeed(int gameLevel)
    {
        if (gameLevel == 1)
        {
            return Random.Range(1, 4);
        }
        else if (gameLevel == 2)
        {
            return Random.Range(4, 8);
        }
        else if (gameLevel == 3)
        {
            return Random.Range(8, 13);
        }
        else
        {
            return Random.Range(13, 17);
        }
    }

    private void DecrementHitCount()
    {
        Debug.Log("DecrementHitCount");
        HitCount--;
    }

    private void TextChange()
    {
        CountText.text = HitCount.ToString();
    }

    private void IsDestroy()
    {
        Debug.Log("IsDestroy");
        if (HitCount <= 0)
        {
            Debug.Log("Destroy : " + this.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("NotDestroy");
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

    public void MoveLineIndex()
    {
        LineIndex++;
    }
}
