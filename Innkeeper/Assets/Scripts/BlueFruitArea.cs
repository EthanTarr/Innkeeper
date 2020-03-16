using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlueFruitArea : MonoBehaviour
{
    //Timer
    [HideInInspector] public Transform myTimer = null;
    public Transform Timer;
    public float TimeDelay = 4f; //length of gathering time in seconds

    public int FruitGain = 5; //number of fruits gained with each gather action
    public int WaterGain = 3; //number of water usages gained with each gather action

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Gather takes in a gather object counter as a GameObject and an amount of gain that object will have as an int
    private void Gather(GameObject GatherObject, int ObjectGain)
    {
        if (myTimer == null) //if timer isnt created
        {
            Debug.LogError(name + " Refil Timer is null on endTime() startup.");
        }
        else
        {
            Destroy(myTimer.gameObject); //destroy timer

            if (GatherObject == null) //Check for Gather Object
            {
                Debug.LogError(name + " GatherObject could not be found after timer destruction.");
            }
            else
            {
                int counter = -1; //Initialize Counter
                try
                {
                    counter = int.Parse(GatherObject.GetComponent<Text>().text); //get current object count from UI
                }
                catch (Exception e)
                {
                    Debug.LogError(name + " GatherObject Counter is not an int. " + e);
                }
                GatherObject.GetComponent<Text>().text = counter + ObjectGain + ""; //add to and save new object count
            }
        }
    }

    //Destroys timer and adds fruits to fruit counter
    public void endBlueFruitGather()
    {
        GameObject BlueFruitCounter = GameObject.Find("Blue Fruit UI Counter"); //Grab Blue Fruit UI Counter Object
        Gather(BlueFruitCounter, FruitGain);
    }

    //Destroys timer and creates four blue fruits in their spaces
    public void endWaterGather()
    {
        GameObject WaterCounter = GameObject.Find("Water UI Counter"); //Grab Water UI Counter Object
        Gather(WaterCounter, WaterGain);
    }
}
