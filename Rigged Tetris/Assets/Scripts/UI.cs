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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeldBlock(string blockName)
    {
        if (currentHeldBlock != null)
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
        currentHeldBlock = Instantiate(targetBlock, heldBlockArea.GetComponent<Transform>());
    }

}
