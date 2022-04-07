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
    //text objects for binds
    public GameObject[] bindTextObj;
    // default values for binds
    public string shiftRightBindDefault;
    public string shiftLeftBindDefault;
    public string shiftDownBindDefault;
    public string placeBindDefault;
    public string rotateBindDefault;
    public string storeBindDefault;
    //binds themselves
    static bool keyBindsAssigned = false;
    static KeyCode shiftRightBind;
    public KeyCode ShiftRightBind {get {return shiftRightBind;} set {shiftRightBind = value;}}
    static KeyCode shiftLeftBind;
    public KeyCode ShiftLeftBind {get {return shiftLeftBind;} set {shiftLeftBind = value;}}
    static KeyCode shiftDownBind;
    public KeyCode ShiftDownBind {get {return shiftDownBind;} set {shiftDownBind = value;}}
    static KeyCode placeBind;
    public KeyCode PlaceBind {get {return placeBind;} set {placeBind = value;}}
    static KeyCode rotateBind;
    public KeyCode RotateBind {get {return rotateBind;} set {rotateBind = value;}}
    static KeyCode storeBind;
    public KeyCode StoreBind {get {return storeBind;} set {storeBind = value;}}
    // For this scripts use only
    GameObject currentButton;
    string keybindStored;
    KeyCode userInput;

    // Start is called before the first frame update
    void Start()
    {
        if (isReference)
        {
            return;
        }
        settingsMenu.SetActive(false);
        // initilizing Shift Speed
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
        // Initilizing Keybinds
        if (keyBindsAssigned)
        {
            return;
        }
        shiftRightBind = (KeyCode)System.Enum.Parse(typeof(KeyCode), shiftRightBindDefault);
        shiftLeftBind = (KeyCode)System.Enum.Parse(typeof(KeyCode), shiftLeftBindDefault);
        shiftDownBind = (KeyCode)System.Enum.Parse(typeof(KeyCode), shiftDownBindDefault);
        placeBind = (KeyCode)System.Enum.Parse(typeof(KeyCode), placeBindDefault);
        rotateBind = (KeyCode)System.Enum.Parse(typeof(KeyCode), rotateBindDefault);
        storeBind = (KeyCode)System.Enum.Parse(typeof(KeyCode), storeBindDefault);
        keyBindsAssigned = true;
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

    // Keybind changing
    void OnGUI() // Use .shift to detect if it is the shift key, then use IsButtonDown for the two key codes to figure out which one is being pressed down. Set Keycode the the right one.
    {
        if (!isReference && Event.current.isKey)
        {
            userInput = Event.current.keyCode;
        }
    }

    IEnumerator findKeyPressed()
    {
        float maxTime = 5F;
        float time = 0;
        userInput = KeyCode.None;
        while (true)
        {
            Debug.Log(userInput);
            if (userInput != KeyCode.None)
            {
                this.changeKeyBind();
                break;
            }
            time += Time.deltaTime;
            if (time > maxTime)
            {
                Debug.Log("Took to long");
                currentButton.GetComponent<Text>().text = keybindStored;
                yield break;
            }
            yield return null;
        }
    }

    public void startChangeKeyBind(GameObject button)
    {
        currentButton = button;
        keybindStored = currentButton.GetComponent<Text>().text;
        currentButton.GetComponent<Text>().text = "-";
        StartCoroutine(findKeyPressed());
    }

    public void changeKeyBind()
    {
        Debug.Log(userInput.ToString());
        switch(currentButton.gameObject.name)
        {
            case "Shift Right Text":
                        shiftRightBind = userInput;
                        break;
            case "Shift Left Text":
                        shiftLeftBind = userInput;
                        break;
            case "Shift Down Text":
                        shiftDownBind = userInput;
                        break;
            case "Place Text":
                        placeBind = userInput;
                        break;
            case "Rotate Text":
                        rotateBind = userInput;
                        break;
            case "Store Text":
                        storeBind = userInput;
                        break;
            default:
                    Debug.Log("Something went wrong");
                    return;
        }
        currentButton.GetComponent<Text>().text = userInput.ToString();
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
