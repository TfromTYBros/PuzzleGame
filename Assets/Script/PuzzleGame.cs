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
        FirstMakeBlocks();
    }

    private void FirstMakeBlocks()
    {
        //ゲーム初回用に変更しよう。
        for (int i = 2; i >= 0; i--)
        {
            RandomSeedChange();
            for (int j = 0; j < 3; j++)
            {
                GameObject newBlock = Instantiate(BlockPrefab, new Vector3(PosXs[RandomSeed[j]], Lines[i].transform.position.y, BlockPosZ), Quaternion.identity, Lines[i].transform);
                newBlock.name = "Block" + i;
                newBlock.GetComponent<BlockScript>().SetLineIndex(i);
            }
        }
    }

    private void RandomMakeBlocks(int value)
    {
        RandomSeedChange();
        //Debug.Log(RandomSeed);
        for (int i = 0; i < value; i++)
        {
            GameObject newBlock = Instantiate(BlockPrefab, new Vector3(PosXs[RandomSeed[i]], Lines[0].transform.position.y, BlockPosZ), Quaternion.identity, Lines[0].transform);
            newBlock.name = "Block" + i;
            newBlock.GetComponent<BlockScript>().SetLineIndex(0);
        }
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
            blockScript.MoveLineIndex();
            Block.transform.SetParent(Lines[blockScript.GetLineIndex()].transform);
            Block.transform.position = new Vector3(Block.transform.position.x, Lines[blockScript.GetLineIndex()].transform.position.y, BlockPosZ);
        }
    }
}
