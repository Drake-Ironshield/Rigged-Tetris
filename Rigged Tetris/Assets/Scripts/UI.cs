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
    public GameObject gameOverObject;

    // Start is called before the first frame update
    void Start()
    {
        gameOverObject.SetActive(false);
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
        GameObject spawnedObject = null;
        for (int i = 0; i < nextBlocks.Length; i++)
        {
            if (i == 0)
            {
                spawnedObject = nextBlockObjects[i];
                creatorScript.SpawnSpecificBlock(nextBlockNames[i]);
            }
            if (i != nextBlocks.Length - 1)
            {
                nextBlockNames[i] = nextBlockNames[i + 1];
                nextBlockObjects[i] = nextBlockObjects[i + 1];
                nextBlockObjects[i].GetComponent<Transform>().position = nextBlocks[i].GetComponent<Transform>().position;
            }
            else
            {
                nextBlockNames[i] = creatorScript.spawnBlock();
                SetHeldBlock(nextBlockNames[i] , i);
            }
        }
        Destroy(spawnedObject);
    }

    public void SetHeldBlock(string blockName, int slot)
    {
        if (slot == -1 && currentHeldBlock != null)
        {
            Destroy(currentHeldBlock);
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
