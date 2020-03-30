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

    public Transform CraftingTable;

    private Transform Player; //Player Transform

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if (Player == null)
        {
            Debug.LogError(name + " could not find Player on startup.");
        }

        if (CraftingTable == null)
        {
            Debug.LogError(name + " could not find crafting table on startup.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Destroys timer and adds fruits to fruit counter
    private void endBlueFruitGather()
    {
        Gather(BlueFruit, FruitGain);
    }

    //Destroys timer and creates water in UI
    private void endWaterGather()
    {
        Gather(Water, WaterGain);
    }

    //Destroys timer and creates water in UI
    private void endAcidFlyGather()
    {
        Gather(AcidFly, AcidFlyGain);
    }

    //Destroys timer and creates blue fruit juice in UI
    private void endBlueFruitJuiceCreation()
    {
        Gather(BlueFruitJuice, BlueFruitJuiceGain);
    }

    // Gather takes in a gather object counter as a GameObject and an amount of gain that object will have as an int
    private void Gather(Transform GatherObject, int GatherGain)
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
                Transform GatheredObject = Instantiate(GatherObject, Player.position, BlueFruit.rotation); //create gathered object on player
                GatheredObject.name = GatherObject.name; //set new objects name to be the same as the original
                GatheredObject.GetComponent<ItemBehavior>().ItemCount = GatherGain; //Set new objects count to be the corresponding GatherGain
                GatheredObject.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
                Player.GetComponent<PlayerBehavior>().GiveObject(GatheredObject); //set Player to hold object
                Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI
            }
        }
    }

    private void CraftItem (List<Transform> Ingredients, string endCall)
    {
        foreach(Transform ingredient in Ingredients)
        {
            ingredient.GetComponent<ItemBehavior>().ItemCount += -1;
        }

        myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer

        Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character

        Invoke(endCall, TimeDelay); //run function endBlueFruitJuiceCreation() after TimerDelay time
    }

    private Boolean checkObject (Transform Ingredient)
    {
        if (Ingredient != null && (Ingredient.name == BlueFruit.name || Ingredient.name == Water.name))
        {
            List<Transform> GatheredObjects = new List<Transform>();
            GatheredObjects.Add(Ingredient);
            if (CraftingTable.GetComponent<StorageBehaviour>().CenterObject != null && CraftingTable.GetComponent<StorageBehaviour>().CenterObject.name != GatheredObjects[0].name &&
                (CraftingTable.GetComponent<StorageBehaviour>().CenterObject.name == BlueFruit.name || CraftingTable.GetComponent<StorageBehaviour>().CenterObject.name == Water.name))
            {
                GatheredObjects.Add(CraftingTable.GetComponent<StorageBehaviour>().CenterObject);
                CraftItem(GatheredObjects, "endBlueFruitJuiceCreation");
                return true;
            }
            else if (CraftingTable.GetComponent<StorageBehaviour>().RightObject != null && CraftingTable.GetComponent<StorageBehaviour>().RightObject.name != GatheredObjects[0].name &&
                (CraftingTable.GetComponent<StorageBehaviour>().RightObject.name == BlueFruit.name || CraftingTable.GetComponent<StorageBehaviour>().RightObject.name == Water.name))
            {
                GatheredObjects.Add(CraftingTable.GetComponent<StorageBehaviour>().RightObject);
                CraftItem(GatheredObjects, "endBlueFruitJuiceCreation");
                return true;
            }
            else if (CraftingTable.GetComponent<StorageBehaviour>().LeftObject != null && CraftingTable.GetComponent<StorageBehaviour>().LeftObject.name != GatheredObjects[0].name &&
                (CraftingTable.GetComponent<StorageBehaviour>().LeftObject.name == BlueFruit.name || CraftingTable.GetComponent<StorageBehaviour>().LeftObject.name == Water.name))
            {
                GatheredObjects.Add(CraftingTable.GetComponent<StorageBehaviour>().LeftObject);
                CraftItem(GatheredObjects, "endBlueFruitJuiceCreation");
                return true;
            }
        }
        return false;
    }



    // places timer on Table Area and calls function to increase Water
    public void CreateBlueFruitJuice()
    {
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject == null)) //Check for if timer isnt running
        {
            bool Created = false;
            if (!Created)
            {
                Created = checkObject(CraftingTable.GetComponent<StorageBehaviour>().LeftObject);
            }
            if (!Created)
            {
                Created = checkObject(CraftingTable.GetComponent<StorageBehaviour>().CenterObject);
            }
            if (!Created)
            {
                Created = checkObject(CraftingTable.GetComponent<StorageBehaviour>().RightObject);
            }

                /*int BlueFruitCount = -1; //Initialize blue fruit Counter
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
                }*/
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
