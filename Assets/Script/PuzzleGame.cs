using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject[] Lines;
    [SerializeField] private List<int> randomSeed = new List<int> { 0, 1, 2, 3, 4 };
    [SerializeField] private List<int> randomSeed3_5 = new List<int> { 3, 4, 5 };

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

    //***********GAMEOVER**************//
    public Fade fadeGameOver;

    //***********GameState**************//
    public enum GameState { START_GAME,BALL_ANGLE,MOVING_NOW,CLEAN_UP,GAMESET,GAMEOVER };
    public GameState state;

    //***********UserInput*************//
    UserInput userInput;

    //***********UserStatus************//
    UserStatus userStatus;

    void Start()
    {
        userInput = FindObjectOfType<UserInput>();
        userStatus = FindObjectOfType<UserStatus>();
        state = GameState.START_GAME;
        StartGame();
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

    private void RandomSeedChange3_5()
    {
        for (int i = 0; i < 3; i++)
        {
            int value = Random.Range(0, 3);
            int temp = randomSeed[i];
            randomSeed[i] = randomSeed[value];
            randomSeed[value] = temp;
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


    /************
     *START_GAME
     ***********/

    private void StartGame()
    {
        FirstMakeBlockAndItems();
        userInput.EnaGuideBall();
        userInput.FirstDicidePos();
        StartBallAngle();
    }

    private void FirstMakeBlockAndItems()
    {
        for (int i = 2; i >= 0; i--)
        {
            RandomSeedChange();
            for (int j = 0; j < 3; j++)
            {
                if (i == 0 && j == 0)
                {
                    GameObject newItem = Instantiate(itemPrefab, new Vector3(posXs[randomSeed[j]], Lines[i].transform.position.y, blockPosZ), Quaternion.identity, Lines[i].transform);
                    newItem.name = "Item" + newItem.GetComponent<ItemScript>().GetItemStatus(userStatus.GetGameLevel());
                    continue;
                }
                GameObject newBlock = Instantiate(blockPrefab, new Vector3(posXs[randomSeed[j]], Lines[i].transform.position.y, blockPosZ), Quaternion.identity, Lines[i].transform);
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
            ballScript.SetSpeedXY(userInput.GetWay() / 10,0.1f);
            ballScript.Move();
            ballCount--;
        }
    }

    /*********
    *CLEAN_UP
    *********/

    public void IsStartCleanUp()
    {
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
        userInput.BoolGroundTouchReset();
        ResetDestroyCount();

        AllMoveBlocks();
        if (IsGameOver())
        {
            Debug.Log("GameOver");
            StartGameOver();
        }
        else
        {
            RandomSeedChange3_5();
            RandomMakeBlocks(randomSeed3_5[0]);
            userStatus.ResetGameLevelUpCount();
            StartBallAngle();
        }
    }

    bool IsGameOver()
    {
        return 1 <= Lines[7].transform.childCount;
    }

    private void RandomMakeBlocks(int value)
    {
        RandomSeedChange();
        //Debug.Log(randomSeed);
        for (int i = 0; i < value; i++)
        {
            if (userStatus.GetGameLevelUpCount() != 0)
            {
                GameObject newItem = Instantiate(itemPrefab, new Vector3(posXs[randomSeed[i]], Lines[0].transform.position.y, blockPosZ), Quaternion.identity, Lines[0].transform);
                newItem.name = "Item" + newItem.GetComponent<ItemScript>().GetItemStatus(userStatus.GetGameLevel());
                userStatus.MinusGameLevelUpCount();
                continue;
            }
            GameObject newBlock = Instantiate(blockPrefab, new Vector3(posXs[randomSeed[i]], Lines[0].transform.position.y, blockPosZ), Quaternion.identity, Lines[0].transform);
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
        fadeGameOver.FadeIn(1);
        userInput.EnaGameOverPanel();
    }

    public void ReStart()
    {
        //Ball
        userStatus.BallCountReset();
        userInput.ResetPos();

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

        //gameLevelUpCount
        userStatus.ResetGameLevelUpCount();

        //makeCount
        ChangeBlockMakeCount(0);

        //Score
        userStatus.ScoreReset();

        //reStart
        StartGame();
    }

    /********
    *GAME_SET
    *********/

    private void StartGameSet()
    {
        state = GameState.GAMESET;
    }

}
