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
    //public float TimeDelay = 4f; //length of gathering time in seconds

    public float CraftingTimeDelay = 5f;
    public float CookingTimeDelay = 10f;
    public float GatheringTimeDelay = 3f;

    public int FruitGain = 5; //number of fruits gained with each gather action
    public int WaterGain = 3; //number of water usages gained with each gather action
    public int BlueFruitJuiceGain = 1; //number of Blue Fruit Juice created with each create action
    public int AcidFlyGain = 10; //number of Acid Flys created with each create action
    public int SlicedBlueFruitGain = 2; //number of Sliced Blue Fruit created with each create action
    public int PastaGain = 1; //number of Patsa created with each create action
    public int NoodleGain = 5; //number of Noodles created with each create action
    public int DeAcidFlyGain = 3; //number of DeAcid Flys created with each create action

    public Transform BlueFruit;
    public Transform Water;
    public Transform BlueFruitJuice;
    public Transform AcidFly;
    public Transform BlueFruitSlice;
    public Transform Pasta;
    public Transform Noodle;
    public Transform DeAcidFly;

    private Transform Player; //Player Transform
    private Transform Cauldron;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if (Player == null)
        {
            Debug.LogError(name + " could not find Player on startup.");
        }
        Cauldron = GameObject.Find("Cauldron").transform;
        if (Player == null)
        {
            Debug.LogError(name + " could not find Cauldron on startup.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Destroys timer and adds fruits
    private void endBlueFruitGather()
    {
        Gather(BlueFruit, FruitGain, false);
    }

    //Destroys timer and creates water
    private void endWaterGather()
    {
        Gather(Water, WaterGain, false);
    }

    //Destroys timer and creates water
    private void endAcidFlyGather()
    {
        Gather(AcidFly, AcidFlyGain, false);
    }

    //Destroys timer and creates blue fruit juice
    private void endBlueFruitJuiceCreation()
    {
        Gather(BlueFruitJuice, BlueFruitJuiceGain, false);
    }

    //Destroys timer and creates sliced blue fruit
    private void endSlicedBlueFruitCreation()
    {
        Gather(BlueFruitSlice, SlicedBlueFruitGain, false);
    }

    //Destroys timer and creates pasta
    private void endPastaCreation()
    {
        Gather(Pasta, PastaGain, true);
    }

    //Destroys timer and creates noodles
    private void endNoodleGather()
    {
        Gather(Noodle, NoodleGain, false);
    }

    //Destroys timer and creates noodles
    private void endDeAcidFlyCreation()
    {
        Gather(DeAcidFly, DeAcidFlyGain, false);
    }

    // Gather takes in a gather object counter as a GameObject and an amount of gain that object will have as an int
    private void Gather(Transform GatherObject, int GatherGain, bool Cooking)
    {
        Player.GetComponent<PlayerBehavior>().controlMovement = true; //Allow the player from moving the Player character
        Player.GetComponent<CapsuleCollider2D>().enabled = true;
        if (GatherObject == null) //Check for Gather Object
        {
            Debug.LogError(name + " GatherObject could not be found after timer destruction.");
        }
        else
        {
            if (Cooking)
            {
                Transform GatheredObject = Instantiate(GatherObject, Cauldron.position, GatherObject.rotation); //create gathered object on player
                Cauldron.GetComponent<CauldronBehavior>().HeldItem = GatheredObject;
                Cauldron.GetComponent<Collider2D>().enabled = true;
                Cauldron.GetComponent<SpriteRenderer>().sprite = Cauldron.GetComponent<CauldronBehavior>().PastaCauldron;
                GatheredObject.name = GatherObject.name; //set new objects name to be the same as the original
                GatheredObject.GetComponent<ItemBehavior>().ItemCount = GatherGain; //Set new objects count to be the corresponding GatherGain
                GatheredObject.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
            }
            else
            {
                Transform GatheredObject = Instantiate(GatherObject, Player.position, GatherObject.rotation); //create gathered object on player
                GatheredObject.name = GatherObject.name; //set new objects name to be the same as the original
                GatheredObject.GetComponent<ItemBehavior>().ItemCount = GatherGain; //Set new objects count to be the corresponding GatherGain
                GatheredObject.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
                bool check = Player.GetComponent<PlayerBehavior>().GiveObject(GatheredObject); //set Player to hold object
                if (!check)
                {
                    Debug.LogError(name + " attempted to give player " + GatherObject.name + " but player hand was full.");
                }
                Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI
            }
        }
    }

    private void CraftItem (List<Transform> Ingredients, string endCall, float TimeDelay)
    {
        foreach(Transform ingredient in Ingredients)
        {
            ingredient.GetComponent<ItemBehavior>().ItemCount += -1;
        }

        myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer
        Player.GetComponent<GameManager>().Timers.Add(myTimer);
        myTimer.GetComponent<TimerBehavior>().startCounting(TimeDelay);

        Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
        Player.GetComponent<CapsuleCollider2D>().enabled = false;

        Invoke(endCall, TimeDelay); //run function endBlueFruitJuiceCreation() after TimerDelay time
    }

    private void CookItem(List<Transform> Ingredients, string endCall, float TimeDelay)
    {
        foreach (Transform ingredient in Ingredients)
        {
            ingredient.GetComponent<ItemBehavior>().ItemCount += -1;
        }

        Transform aTimer = Instantiate(Timer, Cauldron.transform.position, Timer.rotation); //create timer
        Player.GetComponent<GameManager>().Timers.Add(aTimer);
        aTimer.GetComponent<TimerBehavior>().startCounting(TimeDelay);

        Invoke(endCall, TimeDelay); //run function endBlueFruitJuiceCreation() after TimerDelay time
    }

    private Boolean checkObject2(List<string> DesiredIngredients, List<Transform> GatheredObjects, Transform CraftingSurface, string endcall)
    {
        if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject);
            if (CraftingSurface.name.Equals("CraftingTable"))
            {
                CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
            }
            else if (CraftingSurface.name.Equals("CraftingCauldron"))
            {
                CookItem(GatheredObjects, endcall, CookingTimeDelay);
            }
            else
            {
                Debug.LogError(name + " could not indentify crafting surface on checkObject2 : " + CraftingSurface.name);
            }
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
            (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject);
            if (CraftingSurface.name.Equals("CraftingTable"))
            {
                CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
            }
            else if (CraftingSurface.name.Equals("CraftingCauldron"))
            {
                CookItem(GatheredObjects, endcall, CookingTimeDelay);
            }
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
            (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject);
            if (CraftingSurface.name.Equals("CraftingTable"))
            {
                CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
            }
            else if (CraftingSurface.name.Equals("CraftingCauldron"))
            {
                CookItem(GatheredObjects, endcall, CookingTimeDelay);
            }
            return true;
        }
        return false;
    }

    private Boolean checkObject (Transform Ingredient, List<string> DesiredIngredients, Transform CraftingSurface, string endcall)
    {
        if (Ingredient != null && DesiredIngredients.Contains(Ingredient.name))
        {
            List<Transform> GatheredObjects = new List<Transform>();
            GatheredObjects.Add(Ingredient);
            DesiredIngredients.Remove(Ingredient.name);
            if (DesiredIngredients.Count == 0)
            {
                if (CraftingSurface.name.Equals("CraftingTable"))
                {
                    CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
                }
                else if (CraftingSurface.name.Equals("CraftingCauldron"))
                {
                    CookItem(GatheredObjects, endcall, CookingTimeDelay);
                }
                return true;
            }
            else if (DesiredIngredients.Count == 1)
            {
                return checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
            }
            else
            {
                if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name);
                    return checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name);
                    return checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name);
                    checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
                    return true;
                }
                return false;
            }
        }
        return false;
    }






    public void CreatePasta(Transform CraftingSurface)
    {
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject == null)) //Check for if timer isnt running
        {
            bool Created = false;
            List<string> Ingredients = new List<string>();
            Ingredients.Add("Water");
            Ingredients.Add("Noodles");
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, Ingredients, CraftingSurface, "endPastaCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, Ingredients, CraftingSurface, "endPastaCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, Ingredients, CraftingSurface, "endPastaCreation");
            }
        }
    }

    public void CreateSlicedBlueFruit(Transform CraftingSurface)
    {
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject == null)) //Check for if timer isnt running
        {
            bool Created = false;
            List<string> Ingredients = new List<string>();
            Ingredients.Add("Blue Fruit");
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, Ingredients, CraftingSurface, "endSlicedBlueFruitCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, Ingredients, CraftingSurface, "endSlicedBlueFruitCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, Ingredients, CraftingSurface, "endSlicedBlueFruitCreation");
            }
        }
    }

    public void CreateDeAcidFlys(Transform CraftingSurface)
    {
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject == null)) //Check for if timer isnt running
        {
            bool Created = false;
            List<string> Ingredients = new List<string>();
            Ingredients.Add("Acid Fly");
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, Ingredients, CraftingSurface, "endDeAcidFlyCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, Ingredients, CraftingSurface, "endDeAcidFlyCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, Ingredients, CraftingSurface, "endDeAcidFlyCreation");
            }
        }
    }

    // places timer on Table Area and calls function to increase Water
    public void CreateBlueFruitJuice(Transform CraftingSurface)
    {
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject == null)) //Check for if timer isnt running
        {
            bool Created = false;
            List<string> Ingredients = new List<string>();
            Ingredients.Add("Blue Fruit");
            Ingredients.Add("Water");
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, Ingredients, CraftingSurface, "endBlueFruitJuiceCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, Ingredients, CraftingSurface, "endBlueFruitJuiceCreation");
            }
            if (!Created)
            {
                Created = checkObject(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, Ingredients, CraftingSurface, "endBlueFruitJuiceCreation");
            }
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Water
    public void GatherWater()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(GatheringTimeDelay);
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Player.GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("endWaterGather", GatheringTimeDelay); //run function endTime() after TimerDelay time
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Blue Fruits
    public void GatherBlueFruits()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(GatheringTimeDelay);
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Player.GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("endBlueFruitGather", GatheringTimeDelay); //run function endTime() after TimerDelay time
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Acid Flys
    public void GatherAcidFlys()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(GatheringTimeDelay);
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Player.GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("endAcidFlyGather", GatheringTimeDelay); //run function endTime() after TimerDelay time
        }
    }

    // places timer on Blue Fruit Area and calls function to increase Acid Flys
    public void GatherNoodles()
    {
        if (myTimer == null) //Check for if timer isnt running
        {
            myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer on player
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(GatheringTimeDelay);
            Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
            Player.GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("endNoodleGather", GatheringTimeDelay); //run function endTime() after TimerDelay time
        }
    }

    public void stopInvokes()
    {
        CancelInvoke();
    }
}
