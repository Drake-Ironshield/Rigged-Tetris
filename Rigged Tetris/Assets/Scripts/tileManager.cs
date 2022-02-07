using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class tileManager : MonoBehaviour
{
    public int areaWidth;
    public int areaHeight;
    public float shiftTime = 1;
    public float bottemLeftX;
    public float bottemLeftY;
    float controlTickRate = .1F;
    float downControlTickRate;
    float controlOffset = .2F;
    public Tile emptySpot;
    public Tile filledSpot;
    public GameObject UIObject;
    UI UIscript;
    MainMenu keybindScript;
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
    public float levelSpeedUp;
    public GameObject ghostPrefab;
    GameObject[] ghostBlocks;
    public GameObject[] GhostBlocks {get {return ghostBlocks;} set {ghostBlocks = value;}}

    bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        ghostBlocks = new GameObject[0];
        isGameOver = false;
        hasHeld = false;
        UIscript = UIObject.GetComponent<UI>();
        keybindScript = UIObject.GetComponent<MainMenu>();
        creator = tileSpawner.GetComponent<segmentCreator>();
        controlTickRate = keybindScript.SideValue / 1000f;
        controlOffset = keybindScript.DelayValue / 1000f;
        downControlTickRate = keybindScript.DownValue / 1000f;
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
    {   if (downControlTickRate > shiftTime)
        {
            downControlTickRate = shiftTime;
        }
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
            else
            {
                return;
            }
        }

        if (UIscript.IsPauseOpen)
        {
            return;
        }

        // Down Arrow Pressed
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.shift();
            controlledDown = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            controlDownTimer += Time.deltaTime;
            if (controlDownTimer >= downControlTickRate)
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
            UIscript.shiftNextBlocks();
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
                this.updateGhost();
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
                Vector3Int tileSpot = new Vector3Int((int)bottemLeftX + j - 1, (int)bottemLeftY + i - 1, 0); // Doesn't work without the minus one, I do not know why...
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

    public bool shiftRight()
    {
        bool[,] revert = (bool[,])gravityBlocks.Clone();
        bool revertChange = false;
        for (int i = 0; i < areaHeight; i++)
        {
            if (gravityBlocks[areaWidth - 1,i] == true)
            {
                return false;
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
            return false;
        }
        creator.CurrentPieceCoord[0]++;
        this.updateGhost();
        this.showBlocks();
        return true;
    }

    public bool shiftLeft()
    {
        bool[,] revert = (bool[,])gravityBlocks.Clone();
        bool revertChange = false;
        for (int i = 0; i < areaHeight; i++)
        {
            if (gravityBlocks [0 , i] == true)
            {
                return false;
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
            return false;
        }
        creator.CurrentPieceCoord[0]--;
        this.updateGhost();
        this.showBlocks();
        return true;
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
            this.checkFail();
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
        float scoreSpawnPos = 0;
        int tetrisAmount = 0;
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
                if (scoreSpawnPos == 0)
                {
                    scoreSpawnPos = bottemLeftY + i + 2;
                }
                tetrisAmount++;
            }
            checkTetris = true;
        }
        if (tetrisAmount == 0)
        {
            return;
        }
        UIscript.addScore(tetrisAmount , scoreSpawnPos);
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

        /* Data for rotating
                    [01][05][09][13]  -> [13][14][15][16]  [0,3 -> 0,0][1,3 -> 0,1][2,3 -> 0,2][3,3 -> 0,3]
                    [02][06][10][14]  -> [09][10][11][12]  [0,2 -> 1,0][1,2 -> 1,1][2,2 -> 1,2][3,2 -> 1,3]
                    [03][07][11][15]  -> [05][06][07][08]  [0,1 -> 2,0][1,1 -> 2,1][2,1 -> 2,2][3,1 -> 2,3]
                    [04][08][12][16]  -> [01][02][03][04]  [0,0 -> 3,0][1,0 -> 3,1][2,0 -> 3,2][3,0 -> 3,3]
        */

        if (creator.CurrentPieceCoord[1] < creator.CurrentPieceSize - 1)
        {
            return;
        }
        bool doShiftLeft = false;
        bool doShiftRight = false;
        bool continueLoop = false;
        do
        { 
            continueLoop = false;
            doShiftLeft = false;
            doShiftRight = false;
            if (creator.CurrentPieceCoord[0] < 0) 
            {
                doShiftRight = true;
                continueLoop = true;
            }

            if (creator.CurrentPieceSize - 1 + creator.CurrentPieceCoord[0] >= areaWidth)
            {
                doShiftLeft = true;
                continueLoop = true;
            }
            
            if (doShiftLeft)
            {
                if (!this.shiftLeft())
                {
                    return;
                }
            }
            if (doShiftRight)
            {
                if (!this.shiftRight())
                {
                    return;
                }
            }
        } while (continueLoop);

        for (int i = 0; i < creator.CurrentPieceSize; i++)
        {
            for (int j = 0; j < creator.CurrentPieceSize; j++)
            {
                if (gravityBlocks[creator.CurrentPieceCoord[0] + i, creator.CurrentPieceCoord[1] + j] == true && stableBlocks[creator.CurrentPieceCoord[0] + (creator.CurrentPieceSize - j - 1) , creator.CurrentPieceCoord[1] + i] == true) 
                {
                    return;
                }
            }   
        }
        bool[,] reference = (bool[,])gravityBlocks.Clone();
        for (int i = 0; i < creator.CurrentPieceSize; i++)
        {
            for (int j = 0; j < creator.CurrentPieceSize; j++)
            {
                //gravityBlocks[creator.CurrentPieceCoord[0] + i , creator.CurrentPieceCoord[1] + j] = reference[creator.CurrentPieceCoord[0] + (creator.CurrentPieceSize - j - 1) , creator.CurrentPieceCoord[1] + i];
                gravityBlocks[creator.CurrentPieceCoord[0] + (creator.CurrentPieceSize - j - 1) , creator.CurrentPieceCoord[1] + i] = reference[creator.CurrentPieceCoord[0] + i, creator.CurrentPieceCoord[1] + j];
            }
        }
        this.updateGhost();
        this.showBlocks();
    }

    public void updateGhost() //needs to detect if there are already ghost blocks working, and if there are repurpose them and get rid of the excess.
    {
        if (!creator.BlockFalling)
        {
            for (int i = 0; i < ghostBlocks.Length; i++)
            {
                ghostBlocks[i].SetActive(false);
            }
            return;
        }
        if (ghostBlocks.Length > creator.NumberOfBlocks)
        {
            for (int i = creator.NumberOfBlocks - 1; i < ghostBlocks.Length; i++)
            {
                Destroy(ghostBlocks[i]);
                ghostBlocks[i] = null;
            }
            
        }
        if (ghostBlocks.Length != creator.NumberOfBlocks)
        {
            GameObject[] newArray = new GameObject[creator.NumberOfBlocks];
            for (int i = 0; i < ghostBlocks.Length; i++)
            {
                newArray[i] = ghostBlocks[i];
                if (ghostBlocks[i] != null)
                {
                    ghostBlocks[i].SetActive(false);
                }
            }
            ghostBlocks = newArray;
        }
        int fakeDistance = 0;
        int distance = 20;
        int amountOfBlocks = 0;
        float[ , ] blockPositions = new float[creator.NumberOfBlocks, 2];
        for (int i = 0; i < areaHeight; i++)
        {
            for (int j = 0; j < areaWidth; j++)
            {
                if (gravityBlocks[j , i] == true)
                {
                    blockPositions[amountOfBlocks , 0] = j + bottemLeftX;
                    blockPositions[amountOfBlocks , 1] = i + bottemLeftY;
                    amountOfBlocks++;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (stableBlocks[j , k] != true)
                        {
                            fakeDistance++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (fakeDistance < distance)
                    {
                        distance = fakeDistance;
                    }
                    fakeDistance = 0;
                }
            }
        }
        for (int i = 0; i < creator.NumberOfBlocks; i++)
        {   
            if (ghostBlocks[i] == null)
            {
                ghostBlocks[i] = Instantiate(ghostPrefab);
            }
            Vector3 newPosition = ghostBlocks[i].GetComponent<Transform>().position;
            newPosition.y = blockPositions[i , 1] - distance;
            newPosition.x = blockPositions[i , 0];
            Color newColor = creator.CurrentPieceColor;
            newColor.a = 0.25f;
            ghostBlocks[i].GetComponent<SpriteRenderer>().color = newColor;
            ghostBlocks[i].SetActive(true);
            ghostBlocks[i].GetComponent<Transform>().position = newPosition;
        }

    }

    public void checkFail()
    {
        for (int i = 0; i < areaWidth; i++)
        {
            if (stableBlocks[i , 20])
            {
                UIscript.gameOverObject.SetActive(true);
                isGameOver = true;
            }
        }
    }
}
