using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject square;
    public GameObject TPiece;
    public GameObject LinePiece;
    public GameObject LPiece;
    public GameObject InvertedLPiece;
    public GameObject diagPiece;
    public GameObject invertedDiagPiece;
    public GameObject heldBlockArea;
    GameObject currentHeldBlock;
    public GameObject[] nextBlocks;
    GameObject[] nextBlockObjects;
    string[] nextBlockNames;
    public GameObject segmentCreatorObject;
    segmentCreator creatorScript;

    // Start is called before the first frame update
    void Start()
    {
        nextBlockObjects = new GameObject[nextBlocks.Length];
        nextBlockNames = new string[nextBlocks.Length];
        creatorScript = segmentCreatorObject.GetComponent<segmentCreator>();
        for (int i = 0; i < nextBlocks.Length; i++)
        {
            nextBlockNames[i] = creatorScript.spawnBlock();
            SetHeldBlock(nextBlockNames[i] , i);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shiftNextBlocks()
    {
        for (int i = 0; i < nextBlocks.Length; i++)
        {
            if (i == 0)
            {
                creatorScript.SpawnSpecificBlock(nextBlockNames[i]);
            }
            if (i != nextBlocks.Length - 1)
            {
                nextBlockNames[i] = nextBlockNames[i + 1];
                Debug.Log(i);
                //Vector3 pastPosition = nextBlockObjects[i].GetComponent<Transform>().position;
                nextBlockObjects[i] = nextBlockObjects[i + 1];
                //nextBlockObjects[i].GetComponent<Transform>().position = pastPosition;
            }
            else
            {
                nextBlockNames[i] = creatorScript.spawnBlock();
                SetHeldBlock(nextBlockNames[i] , i);
            }
            if (i == 0)
            {
                Destroy(nextBlockObjects[i]);
            }
        }
    }

    public void SetHeldBlock(string blockName, int slot)
    {
        if (slot == -1 && currentHeldBlock != null)
        {
            Destroy(currentHeldBlock);
        }
        if (slot != -1 && nextBlockObjects[slot] != null)
        {
            Destroy(nextBlockObjects[slot]);
        }
        GameObject targetBlock;
        switch (blockName)
        {
            case "square":
                targetBlock = square;
                break;
            case "TPiece":
                targetBlock = TPiece;
                break;
            case "LinePiece":
                targetBlock = LinePiece;
                break;
            case "LPiece":
                targetBlock = LPiece;
                break;
            case "InvertedLPiece":
                targetBlock = InvertedLPiece;
                break;
            case "diagPiece":
                targetBlock = diagPiece;
                break;
            case "invertedDiagPiece":
                targetBlock = invertedDiagPiece;
                break;
            default: Debug.Log("Something went wrong when setting the held block");
                return;
        }
        if (slot == -1)
        {
            currentHeldBlock = Instantiate(targetBlock, heldBlockArea.GetComponent<Transform>());
        }
        else
        {
            nextBlockObjects[slot] = Instantiate(targetBlock , nextBlocks[slot].GetComponent<Transform>());
        }
        
    }

}
