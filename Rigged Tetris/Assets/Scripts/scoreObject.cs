using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreObject : MonoBehaviour
{
    public float upSpeed;
    public float maxTime;
    public float flashTime;
    float currentTime;
    Transform trans;
    public GameObject textObject;
    Text textScript;
    // Start is called before the first frame update
    void Awake()
    {
        trans = GetComponent<Transform>();
        textScript = textObject.GetComponent<Text>();
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("triggered");
        Vector2 pos = trans.position;
        pos.y = pos.y + upSpeed * Time.deltaTime;
        trans.position = pos;
        currentTime = currentTime + Time.deltaTime;
        Debug.Log(currentTime);
        if (currentTime >= maxTime)
        {
            Destroy(gameObject);
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
}
