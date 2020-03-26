using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResourceManager : MonoBehaviour
{
    //Timer
    private Transform myTimer = null;
    public Transform Timer;
    public float TimeDelay = 4f; //length of gathering time in seconds

    public int FruitGain = 5; //number of fruits gained with each gather action
    public int WaterGain = 3; //number of water usages gained with each gather action
    public int BlueFruitJuiceGain = 1; //number of Blue Fruit Juice created with each create action
    public int AcidFlyGain = 10; //number of Acid Flys created with each create action

    public Transform BlueFruit;
    public Transform Water;
    public Transform BlueFruitJuice;
    public Transform AcidFly;

    private GameObject BlueFruitCounter; //UI counter object for Blue Fruits
    private GameObject WaterCounter; //UI counter object for Water
    private GameObject BlueFruitJuiceCounter; //UI counter for Blue Fruit Juice
    private GameObject AcidFlyCounter; //UI counter for AcidFlys

    private Transform Player; //Player Transform

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if (Player == null)
        {
            Debug.LogError(name + " could not find Player on startup.");
        }
        BlueFruitCounter = GameObject.Find("Blue Fruit UI Counter"); //Grab Blue Fruit UI Counter Object
        if (BlueFruitCounter == null) //check for Blue fruit Counter object
        {
            Debug.LogError(name + " could not find Blue Fruit UI on startup.");
        }
        WaterCounter = GameObject.Find("Water UI Counter"); //Grab Water UI Counter Object
        if (WaterCounter == null) //check for Water Counter object
        {
            Debug.LogError(name + " could not find Water UI on startup.");
        }
        BlueFruitJuiceCounter = GameObject.Find("Blue Fruit Juice UI Counter"); //Grab Blue Fruit Juice UI Counter Object
        if (BlueFruitJuiceCounter == null) //check for Blue fruit Juice Counter object
        {
            Debug.LogError(name + " could not find Blue Fruit Juice UI on startup.");
        }
        AcidFlyCounter = GameObject.Find("Acid Fly UI Counter"); //Grab Acid Fly UI Counter Object
        if (AcidFlyCounter == null) //check for Acid Fly Counter object
        {
            Debug.LogError(name + " could not find Acid Fly UI on startup.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Gather takes in a gather object counter as a GameObject and an amount of gain that object will have as an int
    private void Gather(Transform GatherObject)
    {
        if (myTimer == null) //if timer isnt created
        {
            Debug.LogError(name + " Refil Timer is null on endTime() startup.");
        }
        else
        {
            Destroy(myTimer.gameObject); //destroy timer
            Player.GetComponent<PlayerBehavior>().controlMovement = true; //Allow the player from moving the Player character
            if (GatherObject == null) //Check for Gather Object
            {
                Debug.LogError(name + " GatherObject could not be found after timer destruction.");
            }
            else
            {
                /*int counter = -1; //Initialize Counter
                try
                {
                    counter = int.Parse(GatherObject.GetComponent<Text>().text); //get current object count from UI
                }
                catch (Exception e)
                {
                    Debug.LogError(name + " GatherObject Counter is not an int. " + e);
                }
                GatherObject.GetComponent<Text>().text = counter + ObjectGain + ""; //add to and save new object count
                GatherObject.GetComponent<CounterBehaviour>().onChange(); //signify that the value has been changed*/
                Transform GatheredObject = Instantiate(GatherObject, Player.position, BlueFruit.rotation);
                GatheredObject.transform.localScale = new Vector2(2, 2);
                Player.GetComponent<PlayerBehavior>().HandObject = GatheredObject;
            }
        }
    }

    //Destroys timer and adds fruits to fruit counter
    private void endBlueFruitGather()
    {
        Gather(BlueFruit);
        /*if (BlueFruitCounter == null) //check for Blue fruit Counter object
        {
            Debug.LogError(name + " could not find Blue Fruit Counter UI object.");
        } else
        {
            Gather(BlueFruitCounter, FruitGain);
        }*/
    }

    //Destroys timer and creates water in UI
    private void endWaterGather()
    {
        Gather(Water);
        /*if(WaterCounter == null) //check for Water Counter object
        {
            Debug.LogError(name + " could not find Water Counter UI object.");
        } else
        {
            Gather(WaterCounter, WaterGain);
        }*/
    }

    //Destroys timer and creates water in UI
    private void endAcidFlyGather()
    {
        Gather(AcidFly);
        /*if (AcidFlyCounter == null) //check for Acid Fly Counter object
        {
            Debug.LogError(name + " could not find Acid fly Counter UI object.");
        }
        else
        {
            Gather(AcidFlyCounter, AcidFlyGain);
        }*/
    }

    //Destroys timer and creates blue fruit juice in UI
    private void endBlueFruitJuiceCreation()
    {
        Gather(BlueFruitJuice);
        /*if(BlueFruitJuiceCounter == null) //check for Blue fruit Juice Counter object
        {
            Debug.LogError(name + " could not find Water Counter UI object.");
        } else
        {
            Gather(BlueFruitJuiceCounter, BlueFruitJuiceGain);
        }*/
    }

    // places timer on Table Area and calls function to increase Water
    public void CreateBlueFruitJuice()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            int BlueFruitCount = -1; //Initialize blue fruit Counter
            try
            {
                BlueFruitCount = int.Parse(BlueFruitCounter.GetComponent<Text>().text); //get current object count from UI
            }
            catch (Exception e)
            {
                Debug.LogError(name + " Blue Fruit Counter is not an int. " + e);
            }
            int WaterCount = -1; //Initialize water Counter
            try
            {
                WaterCount = int.Parse(WaterCounter.GetComponent<Text>().text); //get current object count from UI
            }
            catch (Exception e)
            {
                Debug.LogError(name + " Water Counter is not an int. " + e);
            }
            if (BlueFruitCount > 0 && WaterCount > 0)
            {
                myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer

                BlueFruitCounter.GetComponent<Text>().text = BlueFruitCount + -1 + ""; //add to and save new object count
                BlueFruitCounter.GetComponent<CounterBehaviour>().onChange(); //signify that the value has been changed

                WaterCounter.GetComponent<Text>().text = WaterCount + -1 + ""; //add to and save new object count
                WaterCounter.GetComponent<CounterBehaviour>().onChange(); //signify that the value has been changed

                Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character

                Invoke("endBlueFruitJuiceCreation", TimeDelay); //run function endBlueFruitJuiceCreation() after TimerDelay time
            }
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Water
    public void GatherWater()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Invoke("endWaterGather", TimeDelay); //run function endTime() after TimerDelay time
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Blue Fruits
    public void GatherBlueFruits()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Invoke("endBlueFruitGather", TimeDelay); //run function endTime() after TimerDelay time
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Acid Flys
    public void GatherAcidFlys()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Invoke("endAcidFlyGather", TimeDelay); //run function endTime() after TimerDelay time
        }
    }
}
