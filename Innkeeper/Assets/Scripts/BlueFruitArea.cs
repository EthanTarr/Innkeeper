using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlueFruitArea : MonoBehaviour
{
    //Timer
    private static Transform myTimer = null;
    public Transform Timer;
    public float TimeDelay = 4f; //length of gathering time in seconds

    public int FruitGain = 5; //number of fruits gained with each gather action

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Creates timer and invokes 'endtime' function after TimeDelay seconds
    void OnMouseDown()
    {
        if (myTimer == null)
        {
            myTimer = Instantiate(Timer, transform.position, Timer.rotation);
            Invoke("endTime", TimeDelay);
        }
    }

    //Destroys timer and adds fruits to fruit counter
    void endTime()
    {
        if (myTimer == null)
        {
            Debug.LogError("Blue Fruit Patch Timer is null");
        }
        else
        {
            Destroy(myTimer.gameObject); //Destroy timer

            GameObject BlueFruitCounter = GameObject.Find("Blue Fruit UI Counter");
            if(BlueFruitCounter == null)
            {
                Debug.LogError("Blue Fruit UI Counter could not be found");
            } else
            {
                int counter = -1;
                try
                {
                    counter = int.Parse(BlueFruitCounter.GetComponent<Text>().text); //get current blue fruit count
                }
                catch (Exception e)
                {
                    Debug.LogError("Blue Fruit Counter is not an int. " + e);
                }
                BlueFruitCounter.GetComponent<Text>().text = counter + FruitGain + ""; //add to and save new blue fruit count
            }
        }
    }
}
