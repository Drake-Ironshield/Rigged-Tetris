using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public int incrementAmount;
    // Timing buttons
    public GameObject sideText;
    public int sideDefault;
    static int sideValue;
    public GameObject downText;
    public int downDefault;
    static int downValue;
    public GameObject delayText;
    public int delayDefault;
    static int delayValue;
    //Keybinds

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);
        sideValue = sideDefault;
        downValue = downDefault;
        delayValue = delayDefault;
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

    public void increaseOrDecreaseValue(bool isIncreasing, string type)
    {
        int increment = incrementAmount;
        if (!isIncreasing)
        {
            increment = increment * -1;
        }
        switch (type)
        {
            case "side":
            if (sideValue + increment < 0 || sideValue + increment > 1000)
            {
                return;
            }
                sideValue = sideValue + increment;
                sideText.GetComponent<Text>().text = sideValue.ToString();
                break;
            case "down":
            case "delay":
            default: 
                Debug.Log("Something went wrong while incrementing");
                break;
        }
    }
}
