using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CounterBehaviour : MonoBehaviour
{
    public int MinValue = 0;
    public int MaxValue = 99;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call when value changes to ensure that it lies within MinValue and MaxValue
    public void onChange()
    {
        int counter = -1; //Initialize Counter
        try
        {
            counter = int.Parse(GetComponent<Text>().text); //get current resource count from UI
        }
        catch (Exception e)
        {
            Debug.LogError(name + " Counter is not an int. " + e);
        }
        if(counter < MinValue) //If the changed value is below MinValue
        {
            GetComponent<Text>().text = MinValue + ""; //Set Value in Text to MinValue
        }
        else if(counter > MaxValue) //If the changed value is above MaxValue
        {
            GetComponent<Text>().text = MaxValue + ""; //Set Value in Text to MaxValue
        }
    }
}
