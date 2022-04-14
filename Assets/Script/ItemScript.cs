using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    UserStatus userStatus;
    [SerializeField] int[] randomSeed = { 0, 1, 2, 3, 4 };
    public Text description;

    void Start()
    {
        userStatus = FindObjectOfType<UserStatus>();
        RandomSeedChange();
        TextChange(GetRandomSeedTop());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Buffer();
            Destroy(this.gameObject);
        }
    }
    /***************************
     *ゲッターセッターRandomSeed 
     **************************/

    private void RandomSeedChange()
    {
        for (int i = 0; i < 5; i++)
        {
            int value = Random.Range(0, 5);
            int temp = randomSeed[i];
            randomSeed[i] = randomSeed[value];
            randomSeed[value] = temp;
        }
    }

    private int GetRandomSeedTop()
    {
        return randomSeed[0];
    }

    private void TextChange(int value)
    {
        if (value == 4)
        {
            description.text = "x2";
        }
        else
        {
            description.text = "+1";
        }
    }

    /**********
     *Buff決め
     **********/

    private void Buffer()
    {
        if (GetRandomSeedTop() == 4)
        {
            userStatus.Item_Ballx2();
        }
        else
        {
            userStatus.Item_BallPlus();
        }
    }
}
