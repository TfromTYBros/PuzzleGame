using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpTextAnime : MonoBehaviour
{
    public Animator levelUpAnime;
    public void GoAnime()
    {
        levelUpAnime.SetTrigger("LevelUp");
        Debug.Log("GoAnime");
    }
}
