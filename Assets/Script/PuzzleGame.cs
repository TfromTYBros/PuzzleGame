using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public GameObject[] Lines;
    private readonly List<float> PosXs = new List<float>{ -2.2f, -1.1f, 0.0f, 1.1f, 2.2f };
    private readonly float BlockPosZ = 2.0f;
    public GameObject BlockPrefab;
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AllMoveBlocks();
        }
    }

    private void StartGame()
    {
        MakeBlocks();
    }

    private void MakeBlocks()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject newBlock = Instantiate(BlockPrefab, new Vector3(PosXs[i], Lines[i].transform.position.y, BlockPosZ), Quaternion.identity, Lines[i].transform);
            newBlock.name = "Block" + i;
            newBlock.GetComponent<BlockScript>().SetLineIndex(i);
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
