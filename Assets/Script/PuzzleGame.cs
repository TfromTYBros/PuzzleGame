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

    /************
     *START_GAME
     ***********/

    private void StartGame()
    {
        ChangeGameLevel(1);
        FirstMakeBlocks();
        //MakeBall();
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

    /*
    private void MakeBall()
    {
        Instantiate(ballPrefab, outBallPos, Quaternion.identity, ballBox.transform);
        ChangeBallsCount(GetBallsCount() + 1);
        ChangeBallMakeCount(GetBallMakeCount() + 1);
    }
    */

    /***********
    *BALL_ANGLE
    ***********/

    public void StartBallAngle()
    {
        state = GameState.BALL_ANGLE;
    }

    /**************
    *BALL_MOVETIME
    **************/

    public void StartMovingNow()
    {
        state = GameState.MOVING_NOW;
    }

    public void Shot()
    {
        //Test
        ChangeCopy(GetBallsCount()+5);
        StartCoroutine(AllBallsMoveStart());
    }

    private GameObject MakeBall()
    {
        GameObject ball = Instantiate(ballPrefab, userInput.touchGroundPos.position, Quaternion.identity, ballBox.transform);
        ChangeBallsCount(GetBallsCount() + 1);
        ChangeBallMakeCount(GetBallMakeCount() + 1);
        return ball;
    }

    private IEnumerator AllBallsMoveStart()
    {
        if (GetCopy() <= 0) yield break;
        while (GetCopy() != 0)
        {
            yield return ballMakeTimeDistance;
            GameObject ball = MakeBall();
            ball.GetComponent<BallScript>().Move();
            ChangeCopy(GetCopy() - 1);
        }
    }

    /*********
    *CLEAN_UP
    *********/

    private void CleanUp()
    {
        AllMoveBlocks();
        if (IsGameOver())
        {
            StartGameOver();
        }
        else
        {
            RandomMakeBlocks(3);
        }
    }

    bool IsGameOver()
    {
        return 0 <= Lines[8].transform.childCount;
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
        AllReset();
    }

    private void AllReset()
    {
        //Ball
        ChangeBallsCount(0);

        //Block
        foreach (GameObject line in Lines)
        {
            while (line.transform.childCount != 0)
            {
                Destroy(line.transform.GetChild(0));
            }
        }

        //gameLevel
        ChangeGameLevel(1);

        //makeCount
        ChangeBlockMakeCount(0);
    }

    /********
    *GAME_SET
    *********/

    private void StartGameSet()
    {
        state = GameState.GAMESET;
        AllReset();
    }

}
