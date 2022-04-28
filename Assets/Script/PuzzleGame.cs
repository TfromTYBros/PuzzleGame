using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGame : MonoBehaviour
{
    public GameObject[] Lines;
    [SerializeField] private List<int> randomSeedBlockPos = new List<int>{ 0, 1, 2, 3, 4 };
    [SerializeField] private List<int> randomSeedBlockMake = new List<int>{ 2, 2, 2, 2, 2, 3, 3, 3, 3, 4};

    //*************Block***************//
    private readonly List<float> posXs = new List<float> { -2.2f, -1.1f, 0.0f, 1.1f, 2.2f };
    private readonly float blockPosZ = 2.0f;
    public GameObject blockPrefab;
    [SerializeField] private int blockMakeCount = 1;

    //*************Items************//
    public GameObject itemPrefab;

    //*************Balls***************//
    [SerializeField] private int copy = 0;
    public GameObject ballPrefab;
    public GameObject ballBox;
    WaitForSeconds ballMakeTimeDistance = new WaitForSeconds(0.1f);

    //***********CleanUP**************//
    [SerializeField] private int destroyCount = 0;

    /*********GameLevelUp*********/
    public Material[] materialBox = new Material[10];

    //***********GAMEOVER**************//
    public Fade fadeGameOver;
    public Text blocksOnGameOverText;
    public Text gameLevelOnGameOverText;
    public Text turnTextOnGAMEOVER;

    //***********GAMESET**************//
    public Fade fadeGameSet;
    //WaitForSeconds gameSetSeconds = new WaitForSeconds(4.6f);
    WaitForSeconds gameSetTime = new WaitForSeconds(3.0f);
    WaitForSeconds cancelAnimeTime = new WaitForSeconds(6.0f);
    public Text blocksOnGAMESET;
    public Text turnTextOnGAMESET;

    //***********GameState**************//
    public enum GameState { START_GAME,BALL_ANGLE,MOVING_NOW,CLEAN_UP,GAMESET,GAMEOVER };
    public GameState state;

    //*************Anime*************//
    AnimesScript animesScript;
    WaitForSeconds startGameAnimeTime = new WaitForSeconds(3.0f);

    //***********UserInput*************//
    UserInput userInput;

    //***********UserStatus************//
    UserStatus userStatus;

    void Start()
    {
        userInput = FindObjectOfType<UserInput>();
        userStatus = FindObjectOfType<UserStatus>();
        animesScript = FindObjectOfType<AnimesScript>();
        state = GameState.START_GAME;
        StartCoroutine(StartGameAnime());
    }

    /**********************
     *ゲッターセッターSeed
     **********************/

    private void ChangeBlockMakeCount(int count)
    {
        blockMakeCount = count;
    }

    private int GetBlockMakeCount()
    {
        return blockMakeCount;
    }

    private void ShuffleRandomSeed()
    {
        for (int i = 0; i < 5; i++)
        {
            int value = Random.Range(0, 5);
            int temp = randomSeedBlockPos[i];
            randomSeedBlockPos[i] = randomSeedBlockPos[value];
            randomSeedBlockPos[value] = temp;
        }
    }

    private void ShuffleRandomSeedBlockMake()
    {
        for (int i = 0; i < randomSeedBlockMake.Count; i++)
        {
            int value = Random.Range(0, randomSeedBlockMake.Count);
            int temp = randomSeedBlockMake[i];
            randomSeedBlockMake[i] = randomSeedBlockMake[value];
            randomSeedBlockMake[value] = temp;
        }
    }

    private void ChangeCopy(int value)
    {
        copy = value;
    }

    private int GetCopy()
    {
        return copy;
    }

    private void ResetDestroyCount()
    {
        destroyCount = 0;
    }

    private void DestroyCountPlus()
    {
        destroyCount++;
    }

    public int GetDestroyCount()
    {
        return destroyCount;
    }

    public Material GetMaterialInMaterialBox(int index)
    {
        return materialBox[index];
    }

    /************
     *START_GAME
     ***********/

    private IEnumerator StartGameAnime()
    {
        animesScript.GoAnimeOnSTART_GAME();
        yield return startGameAnimeTime;
        StartGame();
    }

    private void StartGame()
    {
        ShuffleRandomSeedBlockMake();
        FirstMakeBlockAndItems();
        userInput.EnaGuideBall();
        userInput.FirstDicidePos();
        userStatus.EnaHaveBallCountText();
        StartBallAngle();
    }

    private void FirstMakeBlockAndItems()
    {
        for (int i = 2; i >= 0; i--)
        {
            ShuffleRandomSeed();
            for (int j = 0; j < 3; j++)
            {
                if (i == 0 && j == 0)
                {
                    GameObject newItem = Instantiate(itemPrefab, new Vector3(posXs[randomSeedBlockPos[j]], Lines[i].transform.position.y, blockPosZ), Quaternion.identity, Lines[i].transform);
                    newItem.name = "Item" + newItem.GetComponent<ItemScript>().GetItemStatus(userStatus.GetGameLevel());
                    continue;
                }
                GameObject newBlock = Instantiate(blockPrefab, new Vector3(posXs[randomSeedBlockPos[j]], Lines[i].transform.position.y, blockPosZ), Quaternion.identity, Lines[i].transform);
                newBlock.name = "Block" + GetBlockMakeCount();
                BlockScript blockScript = newBlock.GetComponent<BlockScript>();
                blockScript.SetHitCount(userStatus.GetGameLevel());
                blockScript.SetLineIndex(i);
            }
            ChangeBlockMakeCount(GetBlockMakeCount() + 1);
        }
    }

    /***********
    *BALL_ANGLE
    ***********/

    public void StartBallAngle()
    {
        state = GameState.BALL_ANGLE;
        userInput.DisBallShadow();
        userInput.EnaGuideBall();
        userStatus.EnaHaveBallCountText();
    }

    /***********
    *MOVING_NOW
    ************/

    public void StartMovingNow()
    {
        state = GameState.MOVING_NOW;
    }

    public void Shot()
    {
        ChangeCopy(userStatus.GetHaveBallCount());
        StartCoroutine(AllBallsMoveStart());
    }

    private GameObject MakeBall()
    {
        GameObject ball = Instantiate(ballPrefab, userInput.touchGroundPos.position, Quaternion.identity, ballBox.transform);
        return ball;
    }

    private IEnumerator AllBallsMoveStart()
    {
        if (GetCopy() <= 0) yield break;
        int ballCount = GetCopy();
        while (ballCount != 0)
        {
            yield return ballMakeTimeDistance;
            GameObject ball = MakeBall();
            BallScript ballScript = ball.GetComponent<BallScript>();
            ballScript.SetSpeedXY(userInput.GetWay() / 15.0f, 0.1f);
            ballScript.Move();
            ballCount--;
        }
    }
    public void MainCameraMaterialChange(int gameLevel)
    {
        Camera.main.GetComponent<Skybox>().material = GetMaterialInMaterialBox(gameLevel);
    }

    /*********
    *CLEAN_UP
    *********/

    public void IsStartCleanUp()
    {
        if (state == GameState.GAMESET) return;
        DestroyCountPlus();

        if (GetCopy() == GetDestroyCount())
        {
            StartCleanUp();
            ChangeCopy(0);
        }
    }

    private void StartCleanUp()
    {
        //resets
        userInput.ApplyDicidePos();
        userInput.BoolGroundTouchReset();
        ResetDestroyCount();

        AllMoveBlocks();
        if (IsGameOver())
        {
            //Debug.Log("GameOver");
            StartGameOver();
        }
        else
        {
            RandomMakeBlocks(randomSeedBlockMake[userStatus.GetTurns() % 10]);
            userStatus.ResetGameLevelUpCount();
            userStatus.TurnProgress();
            StartBallAngle();
        }
    }

    bool IsGameOver()
    {
        return 1 <= Lines[7].transform.childCount;
    }

    private void RandomMakeBlocks(int value)
    {
        ShuffleRandomSeed();
        //Debug.Log(randomSeed);
        for (int i = 0; i < value; i++)
        {
            if (userStatus.GetGameLevelUpCount() != 0)
            {
                GameObject newItem = Instantiate(itemPrefab, new Vector3(posXs[randomSeedBlockPos[i]], Lines[0].transform.position.y, blockPosZ), Quaternion.identity, Lines[0].transform);
                newItem.name = "Item" + newItem.GetComponent<ItemScript>().GetItemStatus(userStatus.GetGameLevel());
                userStatus.MinusGameLevelUpCount();
                continue;
            }
            GameObject newBlock = Instantiate(blockPrefab, new Vector3(posXs[randomSeedBlockPos[i]], Lines[0].transform.position.y, blockPosZ), Quaternion.identity, Lines[0].transform);
            newBlock.name = "Block" + GetBlockMakeCount();
            BlockScript blockScript = newBlock.GetComponent<BlockScript>();
            blockScript.SetHitCount(userStatus.GetGameLevel());
            blockScript.SetLineIndex(0);
        }
        ChangeBlockMakeCount(GetBlockMakeCount() + 1);
    }

    private void AllMoveBlocks()
    {
        for (int i = 7; i >= 0; i--) MoveBlocks(i);
    }

    private void MoveBlocks(int lineIndex)
    {
        if (Lines[lineIndex].transform.childCount <= 0) return;
        while (Lines[lineIndex].transform.childCount != 0)
        {
            GameObject obj = Lines[lineIndex].transform.GetChild(0).gameObject;
            if (obj.transform.CompareTag("Block"))
            {
                BlockScript blockScript = obj.GetComponent<BlockScript>();
                blockScript.MoveLine();
                obj.transform.SetParent(Lines[blockScript.GetLineIndex()].transform);
                obj.transform.position = new Vector3(obj.transform.position.x, Lines[blockScript.GetLineIndex()].transform.position.y, blockPosZ);
            }
            else if (obj.transform.CompareTag("Item"))
            {
                ItemScript itemScript = obj.GetComponent<ItemScript>();
                itemScript.MoveLine();
                obj.transform.SetParent(Lines[itemScript.GetLineIndex()].transform);
                obj.transform.position = new Vector3(obj.transform.position.x, Lines[itemScript.GetLineIndex()].transform.position.y, blockPosZ);
            }
        }
    }

    /**********
    *GAME_OVER
    ***********/

    private void StartGameOver()
    {
        state = GameState.GAMEOVER;
        animesScript.CancelAnimeToLevelUp();
        animesScript.CancelAnimeToLevel9();
        fadeGameOver.FadeIn(2);
        TextChangeOnGAMEOVER();
        userInput.DisBallShadow();
        userInput.EnaGameOverPanel();
    }

    public void ReStart(int stateNumber)
    {
        //stateNumber -> 0 == GAMEOVER
        //stateNumber -> 1 == GAMESET

        //Ball
        userStatus.BallCountReset();
        userInput.ResetTouchRroundPos();
        userStatus.ResetHaveBallCount();

        //Block
        foreach (GameObject line in Lines)
        {
            //Debug.Log(line.name);
            foreach (Transform block in line.transform)
            {
                GameObject.Destroy(block.gameObject);
            }
        }

        //BlockBreakPoint
        userStatus.ChangeBlockBreakPoint(0);

        //gameLevel
        userStatus.ChangeGameLevel(1);
        userStatus.ChangeGameLevelText();

        //gameLevelUpCount
        userStatus.ResetGameLevelUpCount();

        //makeCount
        ChangeBlockMakeCount(0);

        //MainCamera
        MainCameraMaterialChange(0);

        //Fade
        if (stateNumber == 0)fadeGameOver.FadeOut(1);
        else if (stateNumber == 1)fadeGameSet.FadeOut(1);

        //Slider
        userStatus.ResetSlider();

        //Turns
        userStatus.ChangeTurns(1);

        //reStart
        state = GameState.START_GAME;
        StartCoroutine(StartGameAnime());
    }

    private void TextChangeOnGAMEOVER()
    {
        blocksOnGameOverText.text = userStatus.GetBlockBreakPoint().ToString();
        gameLevelOnGameOverText.text = userStatus.GetGameLevel().ToString();
        turnTextOnGAMEOVER.text = userStatus.GetTurns().ToString();
    }

    /********
    *GAME_SET
    *********/

    public void StartGameSet()
    {
        state = GameState.GAMESET;
        StopAllCoroutines();
        ResetBallBox();
        TextChangeOnGAMESET();
        animesScript.CancelAnimeToLevelUp();
        animesScript.GoAnimeOnGAMESET();
        userInput.DisBallShadow();
        StartCoroutine(CancelGameSetAnime());
        StartCoroutine(EnaPanelOnGAMESET());
    }

    private IEnumerator EnaPanelOnGAMESET()
    {
        yield return gameSetTime;
        fadeGameSet.FadeIn(1);
        userInput.EnaGameSetPanel();
    }

    private IEnumerator CancelGameSetAnime()
    {
        yield return cancelAnimeTime;
        animesScript.CancelAnimeToGameSet();
    }

    private void TextChangeOnGAMESET()
    {
        blocksOnGAMESET.text = userStatus.GetBlockBreakPoint().ToString();
        turnTextOnGAMESET.text = userStatus.GetTurns().ToString();
    }

    private void ResetBallBox()
    {
        if (ballBox.transform.childCount == 0 && state != GameState.GAMESET) return;
        foreach (Transform ball in ballBox.transform)
        {
            GameObject.Destroy(ball.gameObject);
        }
    }
}
