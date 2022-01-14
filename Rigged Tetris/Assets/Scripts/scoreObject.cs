using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreObject : MonoBehaviour
{
    public float upSpeed;
    public float maxTime;
    public float flashTime;
    float flashTimer;
    float currentTime;
    Transform trans;
    public GameObject textObject;
    Text textScript;
    // Start is called before the first frame update
    void Awake()
    {
        flashTimer = 0;
        trans = GetComponent<Transform>();
        textScript = textObject.GetComponent<Text>();
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = trans.position;
        pos.y = pos.y + upSpeed * Time.deltaTime;
        trans.position = pos;
        currentTime = currentTime + Time.deltaTime;
        flashTimer = flashTimer + Time.deltaTime;
        if (currentTime >= maxTime)
        {
            Destroy(gameObject);
        }
        if (flashTimer >= flashTime)
        {
            this.changeColor();
            flashTimer = 0;

        }
    }

    public void startUp(int score)
    {
        string sign;
        if (score < 0)
        {
            sign = "-";
        }
        else
        {
            sign = "+";
        }
        string finalProduct = sign + score.ToString();
        textScript.text = finalProduct;
    }

    public void changeColor()
    {
        int colorType = Random.Range(0 , 8);
        switch (colorType)
        {
            case 0:
                textScript.color = Color.black;
                break;
            case 1:
                textScript.color = Color.blue;
                break;
            case 2:
                textScript.color = Color.cyan;
                break;
            case 3:
                textScript.color = Color.gray;
                break;
            case 4:
                textScript.color = Color.green;
                break;
            case 5:
                textScript.color = Color.magenta;
                break;
            case 6:
                textScript.color = Color.red;
                break;
            case 7:
                textScript.color = Color.white;
                break;
            case 8:
                textScript.color = Color.yellow;
                break;
            default:
                Debug.Log("Something went wrong in color change: " + colorType);
                break;
        } 
    }
}
