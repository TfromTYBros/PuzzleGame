using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject[] Lines;
    [SerializeField] private List<int> randomSeed = new List<int> { 0, 1, 2, 3, 4 };
    [SerializeField] private int gameLevel = 1;

    //*************Block***************//
    private readonly List<float> posXs = new List<float> { -2.2f, -1.1f, 0.0f, 1.1f, 2.2f };
    private readonly float blockPosZ = 2.0f;
    public GameObject blockPrefab;
    [SerializeField] private int blockMakeCount = 1;

    //*************Balls***************//
    [SerializeField] public int ballsCount = 0;
    [SerializeField] private int copy = 0;
    public GameObject ballPrefab;
    public GameObject ballBox;
    Vector3 ballPos = new Vector3(0.0f, -3.5f, -5.0f);
    Vector3 outBallPos = new Vector3(-5.0f, 0.0f, -5.0f);
    [SerializeField] private int ballMakeCount = 0;
    WaitForSeconds ballMakeTimeDistance = new WaitForSeconds(0.1f);

    //***********CleanUP**************//
    [SerializeField] private int destroyCount = 0;

    //***********GameState**************//
    public enum GameState { START_GAME,BALL_ANGLE,MOVING_NOW,CLEAN_UP,GAMESET,GAMEOVER };
    public GameState state;

    //***********UserInput*************//
    UserInput userInput;

    void Start()
    {
        userInput = FindObjectOfType<UserInput>();
        state = GameState.START_GAME;
        StartGame();
    }

    void Update()
    {

    }

    /**********************
     *ゲッターセッターSeed
     **********************/
    private void ChangeGameLevel(int level)
    {
        gameLevel = level;
    }

    public int GetGameLevel()
    {
        return gameLevel;
    }

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

    private void ChangeBallsCount(int count)
    {
        ballsCount = count;
    }

    private int GetBallsCount()
    {
        return ballsCount;
    }

    private void ChangeBallMakeCount(int count)
    {
        ballMakeCount = count;
    }

    private int GetBallMakeCount()
    {
        return ballMakeCount;
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
        ChangeGameLevel(1);
        FirstMakeBlocks();
        ChangeBallsCount(1);
        userInput.EnaGuideBall();
        userInput.FirstDicidePos();
        StartBallAngle();
    }

    private void FirstMakeBlocks()
    {
        for (int i = 2; i >= 0; i--)
        {
            RandomSeedChange();
            for (int j = 0; j < 3; j++)
            {
                GameObject newBlock = Instantiate(blockPrefab, new Vector3(posXs[randomSeed[j]], Lines[i].transform.position.y, blockPosZ), Quaternion.identity, Lines[i].transform);
                newBlock.name = "Block" + GetBlockMakeCount();
                BlockScript blockScript = newBlock.GetComponent<BlockScript>();
                blockScript.SetHitCount(GetGameLevel());
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
        ChangeCopy(GetBallsCount());
        StartCoroutine(AllBallsMoveStart());
    }

    private GameObject MakeBall()
    {
        GameObject ball = Instantiate(ballPrefab, userInput.touchGroundPos.position, Quaternion.identity, ballBox.transform);
        /*
        ChangeBallsCount(GetBallsCount() + 1);
        ChangeBallMakeCount(GetBallMakeCount() + 1);*/
        return ball;
    }

    private IEnumerator AllBallsMoveStart()
    {
        if (GetCopy() <= 0) yield break;
        while (GetCopy() != 0)
        {
            yield return ballMakeTimeDistance;
            GameObject ball = MakeBall();
            BallScript ballScript = ball.GetComponent<BallScript>();
            ballScript.SetSpeedXY(userInput.GetWay() / 10,0.1f);
            ballScript.Move();
            ChangeCopy(GetCopy() - 1);
        }
    }

    /*********
    *CLEAN_UP
    *********/

    public void IsStartCleanUp()
    {
        DestroyCountPlus();

        if (GetBallsCount() == GetDestroyCount())
        {
            StartCleanUp();
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
            RandomMakeBlocks(3);
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
            GameObject newBlock = Instantiate(blockPrefab, new Vector3(posXs[randomSeed[i]], Lines[0].transform.position.y, blockPosZ), Quaternion.identity, Lines[0].transform);
            newBlock.name = "Block" + GetBlockMakeCount();
            BlockScript blockScript = newBlock.GetComponent<BlockScript>();
            blockScript.SetHitCount(GetGameLevel());
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
            GameObject Block = Lines[lineIndex].transform.GetChild(0).gameObject;
            BlockScript blockScript = Block.GetComponent<BlockScript>();
            blockScript.MoveLine();
            Block.transform.SetParent(Lines[blockScript.GetLineIndex()].transform);
            Block.transform.position = new Vector3(Block.transform.position.x, Lines[blockScript.GetLineIndex()].transform.position.y, blockPosZ);
        }
    }

    /**********
    *GAME_OVER
    ***********/

    private void StartGameOver()
    {
        state = GameState.GAMEOVER;
        userInput.EnaGameOverPanel();
    }

    public void ReStart()
    {
        //Ball
        ChangeBallsCount(0);
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

        //gameLevel
        ChangeGameLevel(1);

        //makeCount
        ChangeBlockMakeCount(0);

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
