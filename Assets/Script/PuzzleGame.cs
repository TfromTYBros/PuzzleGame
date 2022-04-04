using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject[] Lines;
    private readonly List<float> PosXs = new List<float>{ -2.2f, -1.1f, 0.0f, 1.1f, 2.2f };
    private readonly float BlockPosZ = 2.0f;
    public GameObject BlockPrefab;
    [SerializeField] private List<int> RandomSeed = new List<int> { 0,1,2,3,4};

    [SerializeField] private int GameLevel = 1;
    [SerializeField] private int MakeCount = 1;

    //Balls
    public List<GameObject> Balls;
    public GameObject BallPrefab;
    Vector3 BallPos = new Vector3(0.0f, -3.5f, -5.0f);
    [SerializeField] private int BallMakeCount = 0;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AllMoveBlocks();
            RandomMakeBlocks(3);
        }
    }

    private void StartGame()
    {
        ChangeGameLevel(1);
        FirstMakeBlocks();
    }

    private void ChangeGameLevel(int level)
    {
        GameLevel = level;
    }

    public int GetGameLevel()
    {
        return GameLevel;
    }

    private void ChangeMakeCount(int count)
    {
        MakeCount = count;
    }

    private int GetMakeCount()
    {
        return MakeCount;
    }

    private void FirstMakeBlocks()
    {
        for (int i = 2; i >= 0; i--)
        {
            RandomSeedChange();
            for (int j = 0; j < 3; j++)
            {
                GameObject newBlock = Instantiate(BlockPrefab, new Vector3(PosXs[RandomSeed[j]], Lines[i].transform.position.y, BlockPosZ), Quaternion.identity, Lines[i].transform);
                newBlock.name = "Block" + GetMakeCount();
                BlockScript blockScript = newBlock.GetComponent<BlockScript>();
                blockScript.SetHitCount(GetGameLevel());
                blockScript.SetLineIndex(i);
            }
            ChangeMakeCount(GetMakeCount() + 1);
        }
    }

    private void RandomMakeBlocks(int value)
    {
        RandomSeedChange();
        //Debug.Log(RandomSeed);
        for (int i = 0; i < value; i++)
        {
            GameObject newBlock = Instantiate(BlockPrefab, new Vector3(PosXs[RandomSeed[i]], Lines[0].transform.position.y, BlockPosZ), Quaternion.identity, Lines[0].transform);
            newBlock.name = "Block" + GetMakeCount();
            BlockScript blockScript = newBlock.GetComponent<BlockScript>();
            blockScript.SetHitCount(GetGameLevel());
            blockScript.SetLineIndex(0);
        }
        ChangeMakeCount(GetMakeCount() + 1);
    }

    private void RandomSeedChange()
    {
        for (int i = 0; i < 5; i++)
        {
            int value = Random.Range(0, 5);
            int temp = RandomSeed[i];
            RandomSeed[i] = RandomSeed[value];
            RandomSeed[value] = temp;
        }
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
            Block.transform.position = new Vector3(Block.transform.position.x, Lines[blockScript.GetLineIndex()].transform.position.y, BlockPosZ);
        }
    }

    private void MakeBall()
    {
        Instantiate(BallPrefab,BallPos,)
    }

    private void ChangeBallMakeCount(int count)
    {
        BallMakeCount = count;
    }

    private void GetBallMakeCount()
    {

    }
}
