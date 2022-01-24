using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segmentCreator : MonoBehaviour
{
    public GameObject tileManagerObject;
    tileManager tileManagerScript;
    public int spawnX;
    public int spawnY;
    bool blockFalling;
    public bool BlockFalling {get {return blockFalling;} set {blockFalling = value;}}
    bool[,] squarePiece;
    bool[,] tPiece;
    bool[,] linePiece;
    bool[,] lPiece;
    bool[,] inverseLPiece;
    bool[,] diagPiece;
    bool[,] inverseDiagPiece;
    // variables for rotation calculation
    int currentPieceSize;
    public int CurrentPieceSize {get {return currentPieceSize;} set {currentPieceSize = value;}}
    int[] currentPieceCoord;
    public int[] CurrentPieceCoord {get {return currentPieceCoord;} set {currentPieceCoord = value;}}
    Color currentPieceColor;
    public Color CurrentPieceColor {get {return currentPieceColor;} set {currentPieceColor = value;}}
    int numberOfBlocks;
    public int NumberOfBlocks {get {return numberOfBlocks;}}

    bool holdingItem;
    public bool HoldingItem {get {return holdingItem;} set {holdingItem = value;}}
    string currentBlock;
    public string CurrentBlock {get {return currentBlock;} set {currentBlock = value;}}
    string currentHeldBlock;
    public string CurrentHeldBlock {get {return currentHeldBlock;} set {currentHeldBlock = value;}}

    public bool[] bag;
    // Start is called before the first frame update
    void Start()
    {
        holdingItem = false;
        bag = new bool[7];
        for (int i = 0; i < 7; ++i)
        {
            bag[i] = true;
        }
        currentPieceCoord = new int[2];
        currentPieceColor = Color.red;
        tileManagerScript = tileManagerObject.GetComponent<tileManager>();

        // BEGGNING OF DEFINITIONS FOR EACH PIECE

        /* squarePiece:
            [X] [X]
            [X] [X]
        */
        squarePiece = new bool[2,2] {{true , true},{true , true}};

        /* tPiece
            [ ] [ ] [ ]
            [ ] [X] [ ]
            [X] [X] [X]
        */
        tPiece = new bool[3,3] {{true,true,true},{false,true,false},{false,false,false}};

        /* linePiece
            [ ] [X] [ ] [ ]
            [ ] [X] [ ] [ ]
            [ ] [X] [ ] [ ]
            [ ] [X] [ ] [ ]
        */
        linePiece = new bool[4,4] {{false, true, false, false},{false, true, false, false},{false, true, false, false},{false, true, false, false}};

        /* lPiece
            [ ] [X] [ ]
            [ ] [X] [ ]
            [X] [X] [ ]
        */
        lPiece = new bool[3,3] {{true, true, false},{false,true,false},{false,true,false}};

        /* inverseLPiece
            [X] [ ] [ ]
            [X] [ ] [ ]
            [X] [X] [ ]
        */
        inverseLPiece = new bool[3,3] {{true, true, false},{true,false,false},{true,false,false}};

        /* diagPiece
            [ ] [X] [ ]
            [X] [X] [ ]
            [X] [ ] [ ]
        */
        diagPiece = new bool [3,3] {{true,false,false},{true,true,false},{false,true,false}};

        /*inverseDiagPiece
            [X] [ ] [ ]
            [X] [X] [ ]
            [ ] [X] [ ]
        */
        inverseDiagPiece = new bool[3,3] {{false,true,false},{true,true,false},{true,false,false}};
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string spawnBlock()
    {
        // bag selection
        for (int i = 0; i < 7; i++)
        {
            if (bag[i] == true)
            {
                break;
            }
            if (i == 6)
            {
                for (int j = 0; j < 7; j++)
                {
                    bag[j] = true;
                }
            }
        }
        int bagAmount = 0;
        for (int i = 0; i < 7; ++i)
        {
            if (bag[i] == true)
            {
                bagAmount++;
            }
        }
        int blockType = Random.Range(0 , bagAmount);
        bagAmount = 0;
        for (int i = 6; i >= 0; i--)
        {
            if (bagAmount == blockType && bag[i] == true)
            {
                blockType = i;
                break;
            }
            else if (bag[i] == true)
            {
                bagAmount++;
            }
        }
        bag[blockType] = false; 
        switch (blockType) 
        { // supposed to be blockType
            case 0:
                return "square";
            case 1:
                return "TPiece";
            case 2:
                return "LinePiece";
            case 3:
                return "LPiece";
            case 4:
                return "InvertedLPiece";
            case 5:
                return "diagPiece";
            case 6:
                return "invertedDiagPiece";
            default: 
                Debug.Log("Something went wrong in spawnBlock");
                return "square";
        }

    }

    public void SpawnSpecificBlock(string blockName)
    {
        int arraySize;
        bool[,] selectedBlock;
        switch (blockName)
        {
            case "square":
                arraySize = 2;
                numberOfBlocks = 4;
                selectedBlock = squarePiece;
                currentPieceColor = Color.yellow;
                currentBlock = "square";
                break;
            case "TPiece":
                arraySize = 3;
                numberOfBlocks = 4;
                selectedBlock = tPiece;
                currentPieceColor = Color.magenta;
                currentBlock = "TPiece";
                break;
            case "LinePiece":
                arraySize = 4;
                numberOfBlocks = 4;
                selectedBlock = linePiece;
                currentPieceColor = Color.cyan;
                currentBlock = "LinePiece";
                break;
            case "LPiece":
                arraySize = 3;
                numberOfBlocks = 4;
                selectedBlock = lPiece;
                currentPieceColor = Color.blue;
                currentBlock = "LPiece";
                break;
            case "InvertedLPiece":
                arraySize = 3;
                numberOfBlocks = 4;
                selectedBlock = inverseLPiece;
                currentPieceColor = new Color(1.0f, 0.647f, 0.0f);
                currentBlock = "InvertedLPiece";
                break;
            case "diagPiece":
                arraySize = 3;
                numberOfBlocks = 4;
                selectedBlock = diagPiece;
                currentPieceColor = Color.red;
                currentBlock = "diagPiece";
                break;
            case "invertedDiagPiece":
                arraySize = 3;
                numberOfBlocks = 4;
                selectedBlock = inverseDiagPiece;
                currentPieceColor = Color.green;
                currentBlock = "invertedDiagPiece";
                break;
            default: 
                arraySize = 2;
                numberOfBlocks = 4;
                selectedBlock = squarePiece;
                currentPieceColor = Color.white;
                currentBlock = "square";
                Debug.Log("Something went wrong in spawnBlock");
                break;
        }
        int[] offset = new int[2] {0,0}; // I think the offset always ends up as 
        if (arraySize == 4)
        {
            offset[0] = 1;
        }
        for (int i = 0; i < arraySize; i++)
        {
            for (int j = 0; j < arraySize; j++)
            {
                tileManagerScript.GravityBlocks[spawnX + i - offset[0] , spawnY + j - offset[1]] = selectedBlock[j,i];
            }
        }
        blockFalling = true;
        currentPieceSize = arraySize;
        currentPieceCoord[0] = spawnX - offset[0];
        currentPieceCoord[1] = spawnY - offset[1];
        tileManagerScript.showBlocks();
    }
}
