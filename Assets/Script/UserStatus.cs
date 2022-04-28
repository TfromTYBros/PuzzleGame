using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatus : MonoBehaviour
{
    /********HaveBallCount*********/
    [SerializeField] private int haveBallCount = 0;
    public Text haveBallCountText;
    private readonly Vector3 haveBallCountPosOnStartGame = new Vector3(1.0f, -3.5f, -5.0f);

    /**********GameLevel**********/
    [SerializeField] private int gameLevel = 1;
    [SerializeField] private int gameLevelUpCount = 0;
    public Slider gameLevelUpSlider;
    public Text gameLevelText;

    /**********BlockBreak*********/
    [SerializeField] private int blockBreakPoint = 0;
    public GameObject trashBox;
    public WaitForSeconds delayToDestroy = new WaitForSeconds(0.5f);

    /************Anime*************/
    AnimesScript animesScript;

    /************Parts**************/
    PuzzleGame puzzleGame;
    private readonly float ZERO = 0.0f;
    private readonly float ONE = 1.0f;

    void Start()
    {
        puzzleGame = FindObjectOfType<PuzzleGame>();
        animesScript = FindObjectOfType<AnimesScript>();
        ChangeHaveBallCount(1);
        ChangeTextHaveBallCount();
        SetBallCountTextPosOnStartGame();
        ChangeGameLevelText();
    }

    /*****
     *Ball
     *****/
    public void ChangeHaveBallCount(int value)
    {
        haveBallCount = value;
        if (haveBallCount > 1000) haveBallCount = 999;
    }

    public int GetHaveBallCount()
    {
        return haveBallCount;
    }

    public void ChangeTextHaveBallCount()
    {
        haveBallCountText.text = "x " + GetHaveBallCount().ToString();
    }

    public void BallCountReset()
    {
        ChangeHaveBallCount(1);
        ChangeTextHaveBallCount();
    }
    public void EnaHaveBallCountText()
    {
        haveBallCountText.transform.gameObject.SetActive(true);
    }

    public void DisHaveBallCountText()
    {
        haveBallCountText.transform.gameObject.SetActive(false);
    }
    public void SetBallCountTextPosOnStartGame()
    {
        haveBallCountText.transform.position = haveBallCountPosOnStartGame;
    }
    public void DicideBallCountTextPos(float x)
    {
        if (x < ZERO) x += ONE;
        else x -= ONE;
        haveBallCountText.transform.position = new Vector3(x, haveBallCountText.transform.position.y, haveBallCountText.transform.position.z);
    }

    public void ResetHaveBallCount()
    {
        SetBallCountTextPosOnStartGame();
        ChangeHaveBallCount(1);
        ChangeTextHaveBallCount();
    }

    /*******
     *Slider
     *******/

    public void ChangeSlider(int breakCount)
    {
        gameLevelUpSlider.value = breakCount % 10;
    }

    public void ResetSlider()
    {
        gameLevelUpSlider.value = 0;
    }

    /**********
     *GameLevel
     **********/

    public void ChangeGameLevel(int level)
    {
        gameLevel = level;
    }

    public int GetGameLevel()
    {
        return gameLevel;
    }
    public void IsGameLevelUp()
    {
        if ((GetBlockBreakPoint() % 10) == 0 && GetBlockBreakPoint() != 0 && puzzleGame.state != PuzzleGame.GameState.GAMESET)
        {
            GameLevelUp();
            ChangeGameLevelText();
            if (IsGAMESET())
            {
                puzzleGame.StartGameSet();
                return;
            }
            PlusGameLevelUpcOunt();
            puzzleGame.MainCameraMaterialChange(GetGameLevel() % 10);
            if (GetGameLevel() != 9) animesScript.GoAnimeOnLevelUp();
            else animesScript.GoAnimeOnLevel9();
        }
    }
    private void GameLevelUp()
    {
        if (gameLevel < 999) gameLevel++;
    }
    private bool IsGAMESET()
    {
        return GetGameLevel() >= 10;
    }

    public void ChangeGameLevelText()
    {
        if (GetGameLevel() != 10) gameLevelText.text = "LEVEL " + GetGameLevel().ToString();
        else gameLevelText.text = "CLEAR!!";
    }

    /*****************
     *GameLevelUpCount
     *****************/

    public void PlusGameLevelUpcOunt()
    {
        gameLevelUpCount++;
    }

    public void MinusGameLevelUpCount()
    {
        if (0 < gameLevelUpCount) gameLevelUpCount--;
    }
    public int GetGameLevelUpCount()
    {
        return gameLevelUpCount;
    }

    public void ResetGameLevelUpCount()
    {
        gameLevelUpCount = 0;
    }

    /***********
     *BlockBreak
     ***********/

    public void ChangeBlockBreakPoint(int point)
    {
        blockBreakPoint = point;
    }

    public int GetBlockBreakPoint()
    {
        return blockBreakPoint;
    }

    public void PlusBlockBreakPoint()
    {
        if (puzzleGame.state == PuzzleGame.GameState.MOVING_NOW) blockBreakPoint++;
    }

    /******
     *Items
     ******/

    public void ItemSelect()
    {
        if (puzzleGame.state != PuzzleGame.GameState.MOVING_NOW) return;
        if (GetGameLevel() == 5) Item_Ballx2();
        else Item_BallPlus();
    }

    private void Item_Ballx2()
    {
        ChangeHaveBallCount(GetHaveBallCount() * 2);
    }
    
    private void Item_BallPlus()
    {
        ChangeHaveBallCount(GetHaveBallCount() + 1);
    }
}
