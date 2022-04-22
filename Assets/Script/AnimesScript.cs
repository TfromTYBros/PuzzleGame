using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimesScript : MonoBehaviour
{
    public Animator levelUpAnime;
    public Animator gameSetAnime;
    public void GoAnimeOnLevelUp()
    {
        levelUpAnime.SetTrigger("Start");
    }
    public void CancelAnimeToLevelUp()
    {
        levelUpAnime.Play("LevelUpFromDown", 0, 0.93f);
    }

    public void GoAnimeOnGAMESET()
    {
        gameSetAnime.SetTrigger("Start");
    }

    public void CancelAnimeToGameSet()
    {
        gameSetAnime.Play("GameSet",0,1.0f);
    }
}
