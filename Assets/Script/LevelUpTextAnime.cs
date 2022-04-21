using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpTextAnime : MonoBehaviour
{
    public Animator levelUpAnime;
    public void GoAnime()
    {
        levelUpAnime.SetTrigger("Start");
    }

    public void CancelAnime()
    {
        levelUpAnime.Play("LevelUpFromDown",0,0.93f);
    }
}
