using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    public bool isReference;
    public GameObject settingsMenu;
    public int incrementAmount;
    // Timing buttons
    public GameObject sideText;
    public int sideDefault;
    static int sideValue;
    public int SideValue {get {return sideValue;} set {sideValue = value;}}
    public GameObject downText;
    public int downDefault;
    static int downValue;
    public int DownValue {get {return downValue;} set {downValue = value;}}
    public GameObject delayText;
    public int delayDefault;
    static int delayValue;
    public int DelayValue {get {return delayValue;} set {delayValue = value;}}
    //Keybinds

    // Start is called before the first frame update
    void Start()
    {
        if (isReference)
        {
            return;
        }
        settingsMenu.SetActive(false);
        if (sideValue == 0)
        {
            sideValue = sideDefault;
        }
        if (downValue == 0)
        {
            downValue = downDefault;
        }   
        if (delayValue == 0)
        {
            delayValue = delayDefault;
        }
        sideText.GetComponent<Text>().text = sideValue.ToString();
        downText.GetComponent<Text>().text = downValue.ToString();
        delayText.GetComponent<Text>().text = delayValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Begin()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void openSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void closeSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void decreaseValue(string type) // Unity is stupid and win't let you use multi argument methods for buttons.
    {
        this.manipulateValue(false , type);
    }
    
    public void increaseValue(string type) // See above comment
    {
        this.manipulateValue(true , type);
    }

    public void manipulateValue(bool isIncreasing, string type)
    {
        int increment = incrementAmount;
        if (!isIncreasing)
        {
            increment = increment * -1;
        }
        switch (type)
        {
            case "side":
            if (sideValue + increment <= 0 || sideValue + increment >= 1000)
            {
                return;
            }
                sideValue = sideValue + increment;
                sideText.GetComponent<Text>().text = sideValue.ToString();
                break;
            case "down":
                if (downValue + increment <= 0 || downValue + increment >= 1000)
                {
                    return;
                }
                downValue = downValue + increment;
                downText.GetComponent<Text>().text = downValue.ToString();
                break;
            case "delay":
                if (delayValue + increment <= 0 || delayValue + increment >= 1000)
                {
                    return;
                }
                delayValue = delayValue + increment;
                delayText.GetComponent<Text>().text = delayValue.ToString();
                break;
            default: 
                Debug.Log("Something went wrong while incrementing");
                break;
        }
    }

}
