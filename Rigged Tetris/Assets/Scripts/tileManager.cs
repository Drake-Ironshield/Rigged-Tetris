using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileManager : MonoBehaviour
{
    public int areaWidth;
    public int areaHeight;
    public float shiftTime = 1;
    public float bottemLeftX;
    public float bottemLeftY;
    public float controlTickRate = .1F;
    public float controlOffset = .2F;
    public Tile emptySpot;
    public Tile filledSpot;
    public GameObject UIObject;
    UI UIscript;
    bool[,] stableBlocks;
    bool[,] gravityBlocks;
    Color[,] blockColors;
    public bool[,] GravityBlocks {get {return gravityBlocks;} set {gravityBlocks = value;}}
    bool[,] filledBlocks;
    Tilemap map;
    float timer;
    float controlDownTimer;
    float controlRightTimer;
    float controlLeftTimer;
    bool controlledDown;
    public GameObject tileSpawner;
    segmentCreator creator;
    bool hasHeld;

    // Start is called before the first frame update
    void Start()
    {
        hasHeld = false;
        UIscript = UIObject.GetComponent<UI>();
        creator = tileSpawner.GetComponent<segmentCreator>();
        controlDownTimer = 0;
        controlRightTimer = 0;
        controlLeftTimer = 0;
        controlledDown = false;
        timer = 0;
        stableBlocks = new bool[areaWidth , areaHeight + 3];
        gravityBlocks = new bool[areaWidth, areaHeight + 3];
        filledBlocks = new bool[areaWidth, areaHeight + 3];
        blockColors = new Color[areaWidth, areaHeight + 3];
        areaHeight = areaHeight + 3;
        for (int i = 0; i < areaWidth; ++i)
        {
            for (int j = 0; j < areaHeight; ++j)
            {
                blockColors[i,j] = Color.white;
            }
        }
        map = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {   
        // Down Arrow Pressed
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.shift();
            controlledDown = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            controlDownTimer += Time.deltaTime;
            if (controlDownTimer >= controlTickRate)
            {
                this.shift();
                controlDownTimer = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            controlDownTimer = 0;
            controlledDown = false;
        }
        // Right Arrow Pressed
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.shiftRight();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            controlRightTimer += Time.deltaTime;
            if (controlRightTimer >= controlTickRate)
            {
                this.shiftRight();
                controlRightTimer = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            controlRightTimer = controlOffset * -1;
        }
        // Left Arrow Pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.shiftLeft();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            controlLeftTimer += Time.deltaTime;
            if (controlLeftTimer >= controlTickRate)
            {
                this.shiftLeft();
                controlLeftTimer = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            controlLeftTimer = controlOffset * -1;
        }
        // Space Pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (tileSpawner.GetComponent<segmentCreator>().BlockFalling)
            {
                this.shift();
            }
            tileSpawner.GetComponent<segmentCreator>().spawnBlock();
        }
        // Z Pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.rotate();
        }
        // C Pressed
        if (Input.GetKeyDown(KeyCode.C) && !hasHeld)
        {
            string heldBlock = creator.CurrentHeldBlock;
            for (int i = 0; i < areaWidth; ++i)
            {
                for (int j = 0; j < areaHeight; ++j)
                {
                    gravityBlocks[i,j] = false;
                }
            }
            UIscript.SetHeldBlock(creator.CurrentBlock , -1);
            creator.CurrentHeldBlock = creator.CurrentBlock;
            if (creator.HoldingItem)
            {
                creator.SpawnSpecificBlock(heldBlock);
            }
            else
            {
                creator.BlockFalling = false;
            }
            creator.HoldingItem = true;
            hasHeld = true;
            this.showBlocks();
        }
        // automatic movement/tile spawning
        if (!controlledDown)
        {
            timer += Time.deltaTime;
        }
        if (timer >= shiftTime && !controlledDown)
        {
            this.shift();
            timer = 0;
            if (!tileSpawner.GetComponent<segmentCreator>().BlockFalling)
            {
                UIscript.shiftNextBlocks();
            }
        }
    }

    public void showBlocks()
    {
        for (int i = 0; i < areaHeight - 3; i++)
        {
            for (int j = 0; j < areaWidth; j++)
            {
                Vector3Int tileSpot = new Vector3Int((int)bottemLeftX + j, (int)bottemLeftY + i, 0);
                if (stableBlocks[j , i] == true || gravityBlocks[j , i] == true)
                {
                    map.SetTile(tileSpot, filledSpot);
                }
                else
                {
                    map.SetTile(tileSpot, emptySpot);
                }
                if (gravityBlocks[j , i] == true)
                {
                    map.SetColor(tileSpot, creator.CurrentPieceColor);
                }
                else
                {
                    map.SetColor(tileSpot, blockColors[j , i]);
                }
            }
        }
    }

    public void shiftRight()
    {
        bool[,] revert = (bool[,])gravityBlocks.Clone();
        bool revertChange = false;
        for (int i = 0; i < areaHeight; i++)
        {
            if (gravityBlocks[areaWidth - 1,i] == true)
            {
                return;
            }
        }
        for (int i = areaWidth - 1; i >= 0; i--)
        {
            for (int j = areaHeight - 1; j >= 0; j--)
            {
                if (i == 0)
                {
                    gravityBlocks[i , j] = false;
                }
                else
                {
                    if (stableBlocks[i , j] && gravityBlocks[i - 1 , j] == true)
                    {
                        revertChange = true;
                    }
                    gravityBlocks[i , j] = gravityBlocks[i - 1 , j];
                }
            }
        }
        if (revertChange)
        {
            gravityBlocks = revert;
        }
        creator.CurrentPieceCoord[0]++;
        this.showBlocks();
    }

    public void shiftLeft()
    {
        bool[,] revert = (bool[,])gravityBlocks.Clone();
        bool revertChange = false;
        for (int i = 0; i < areaHeight; i++)
        {
            if (gravityBlocks [0 , i] == true)
            {
                return;
            }
        }
        for (int i = 0; i < areaWidth; i++)
        {
            for (int j = 0; j < areaHeight; j++)
            {
                if (i == areaWidth - 1)
                {
                    gravityBlocks[i,j] = false;
                }
                else
                {
                    if (stableBlocks[i , j] && gravityBlocks[i + 1 , j] == true)
                    {
                        revertChange = true;
                    }
                    gravityBlocks[i,j] = gravityBlocks[i + 1 , j];
                }
            }
        }
        if (revertChange)
        {
            gravityBlocks = revert;
        }
        creator.CurrentPieceCoord[0]--;
        this.showBlocks();
    }

    public void shift()
    {
        bool place = false;
        for (int i = 0; i < areaHeight; i++)
        {
            for (int j = 0; j < areaWidth; j++)
            {
                if (gravityBlocks[j , i] == true)
                {
                    if (i == 0)
                    {
                        place = true;
                        tileSpawner.GetComponent<segmentCreator>().BlockFalling = false;
                    }
                    else if (stableBlocks[j , i - 1])
                    {
                        place = true;
                        tileSpawner.GetComponent<segmentCreator>().BlockFalling = false;
                    }
                }
            }
        }
        for (int i = 0; i < areaHeight; i++)
        {
            for (int j = 0; j < areaWidth; j++)
            {
                if (place == true)
                {
                    if (gravityBlocks[j , i] == true)
                    {
                        gravityBlocks[j , i] = false;
                        stableBlocks[j , i] = true;
                        blockColors[j , i] = creator.CurrentPieceColor;
                    }
                }
                else
                {
                    if (i == areaHeight - 1)
                    {
                        gravityBlocks[j, i] = false;
                    }
                    else
                    {
                        gravityBlocks[j , i] = gravityBlocks[j , i + 1];
                        gravityBlocks[j , i + 1] = false;
                    }
                    
                }
            }
        }
        if (place == true)
        {
            this.checkTetris();
            hasHeld = false;
        }
        if (creator.BlockFalling)
        {
            creator.CurrentPieceCoord[1]--;
        }
        this.showBlocks();
    }

    public void checkTetris()
    {
        //finding the tetris
        bool[] isTetris = new bool[areaHeight];
        bool checkTetris = true;
        for (int i = 0; i < areaHeight; i++)
        {
            for (int j = 0; j < areaWidth; j++)
            {
                if (stableBlocks[j,i] == false)
                {
                    checkTetris = false;
                }
            }
            if (checkTetris == true)
            {
                isTetris[i] = true;
            }
            checkTetris = true;
        }
        // dealing with the tetris
        for (int i = 0; i < areaHeight; i++)
        {
            if (isTetris[i] == true)
            {
                isTetris[i] = false;
                for (int j = 0; j < areaWidth; j++)
                {
                    stableBlocks[j , i] = false;
                    blockColors[j , i] = Color.white;
                }
                for (int j = i; j < areaHeight; j++)
                {
                    for (int k = 0; k < areaWidth; k++)
                    {
                        if (j == areaHeight - 1)
                        {
                            stableBlocks[k , j] = false;
                            blockColors[k , j] = Color.white;
                            isTetris[j] = false;
                        }
                        else
                        {
                            stableBlocks[k , j] = stableBlocks[k , j + 1];
                            blockColors[k , j] = blockColors[ k , j + 1];
                            isTetris[j] = isTetris[j + 1];
                        }
                    }
                }
                i--;
            }
        }     
    }

    public void rotate()
    {
        bool[,] revert = (bool[,])gravityBlocks.Clone();
        bool[,] reference = (bool[,])gravityBlocks.Clone();
        bool revertChange = false;
        if (creator.CurrentPieceCoord[0] < 0)
        {
            this.shiftRight();
        }
        if (creator.CurrentPieceCoord[0] >= areaWidth - creator.CurrentPieceSize + 1)
        {
            this.shiftLeft();
        }
        /*switch (creator.CurrentPieceSize)
        {
            case 2:
                   [1][2] ->  [2][4] [0,1 -> 0,0][1,1 -> 0,1]
                   [3][4] ->  [1][3] [0,0 -> 1,0][1,0 -> 1,1]
                
                Debug.Log(creator.CurrentPieceCoord[0] + " , " + creator.CurrentPieceCoord[1]);
                gravityBlocks[creator.CurrentPieceCoord[0] , creator.CurrentPieceCoord[1]] = revert[creator.CurrentPieceCoord[0] , creator.CurrentPieceCoord[1] + 1];
                gravityBlocks[creator.CurrentPieceCoord[0] , creator.CurrentPieceCoord[1] + 1] = revert[creator.CurrentPieceCoord[0] + 1 , creator.CurrentPieceCoord[1] + 1];
                gravityBlocks[creator.CurrentPieceCoord[0] + 1 , creator.CurrentPieceCoord[1]] = revert[creator.CurrentPieceCoord[0] , creator.CurrentPieceCoord[1]];
                gravityBlocks[creator.CurrentPieceCoord[0] + 1 , creator.CurrentPieceCoord[1] + 1] = revert[creator.CurrentPieceCoord[0] + 1 , creator.CurrentPieceCoord[1]];
                break;
            case 3:
                
                      [1][4][7]  -> [7][8][9] [0,2 -> 0,0][1,2 -> 0,1][2,2 -> 0,2]
                      [2][5][8]  -> [4][5][6] [0,1 -> 1,0][1,1 -> 1,1][2,1 -> 1,2]
                      [3][6][9]  -> [1][2][3] [0,0 -> 2,0][1,0 -> 2,1][2,0 -> 2,2]
                
                
                break;
            case 4:
                
                    [01][05][09][13]  -> [13][14][15][16]  [0,3 -> 0,0][1,3 -> 0,1][2,3 -> 0,2][3,3 -> 0,3]
                    [02][06][10][14]  -> [09][10][11][12]  [0,2 -> 1,0][1,2 -> 1,1][2,2 -> 1,2][3,2 -> 1,3]
                    [03][07][11][15]  -> [05][06][07][08]  [0,1 -> 2,0][1,1 -> 2,1][2,1 -> 2,2][3,1 -> 2,3]
                    [04][08][12][16]  -> [01][02][03][04]  [0,0 -> 3,0][1,0 -> 3,1][2,0 -> 3,2][3,0 -> 3,3]
                

                break;
            default:
                Debug.Log("Something went wrong in tileManager.rotate");
                return;
        }*/

        for (int i = 0; i < creator.CurrentPieceSize; i++) // working, but doesn't feel like the rotation in the main game, going to check the rotation point for those and go from there
        {
            for (int j = 0; j < creator.CurrentPieceSize; j++)
            {
                if (gravityBlocks[creator.CurrentPieceCoord[0] + i, creator.CurrentPieceCoord[1] + j] == true && stableBlocks[creator.CurrentPieceCoord[0] + (creator.CurrentPieceSize - j - 1) , creator.CurrentPieceCoord[1] + i] == true)
                {
                    revertChange = true;
                }
                gravityBlocks[creator.CurrentPieceCoord[0] + i , creator.CurrentPieceCoord[1] + j] = reference[creator.CurrentPieceCoord[0] + (creator.CurrentPieceSize - j - 1) , creator.CurrentPieceCoord[1] + i];
            }
        }


        if (revertChange)
        {
            gravityBlocks = revert;
            Debug.Log("reverting Change");
        }
        this.showBlocks();
    }
}
