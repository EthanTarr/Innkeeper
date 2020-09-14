using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class ResourceManager : MonoBehaviour
{
    //Timer
    private Transform myTimer = null;
    public Transform Timer;
    //public float TimeDelay = 4f; //length of gathering time in seconds

    public float CraftingTimeDelay = 5f;
    public float GatheringTimeDelay = 3f;
    public float CookingTimeDelay = 10f;

    public Transform KitchenTable;

    public int FruitGain = 5; //number of fruits gained with each gather action
    public int WaterGain = 3; //number of water usages gained with each gather action
    public int BlueFruitJuiceGain = 1; //number of Blue Fruit Juice created with each create action
    public int AcidFlyGain = 10; //number of Acid Flys created with each create action
    public int SlicedBlueFruitGain = 2; //number of Sliced Blue Fruit created with each create action
    public int NoodleGain = 5; //number of Noodles created with each create action
    public int DeAcidFlyGain = 3; //number of DeAcid Flys created with each create action
    public int WaterGlassGain = 5;
    public int PastaGain = 3;

    public List<Transform> Foods;

    public Transform BlueFruit;
    public Transform Water;
    public Transform BlueFruitJuice;
    public Transform AcidFly;
    public Transform BlueFruitSlice;
    public Transform Noodle;
    public Transform DeAcidFly;
    public Transform GlassWater;
    public Transform PastaBowl;

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
        Gather(BlueFruit, FruitGain);
    }

    //Destroys timer and creates water
    private void endWaterGather()
    {
        Gather(Water, WaterGain);
    }

    //Destroys timer and creates water
    private void endAcidFlyGather()
    {
        Gather(AcidFly, AcidFlyGain);
    }

    //Destroys timer and creates blue fruit juice
    private void endBlueFruitJuiceCreation()
    {
        Gather(BlueFruitJuice, BlueFruitJuiceGain);
    }

    //Destroys timer and creates sliced blue fruit
    private void endSlicedBlueFruitCreation()
    {
        Gather(BlueFruitSlice, SlicedBlueFruitGain);
    }

    //Destroys timer and creates noodles
    private void endNoodleGather()
    {
        Gather(Noodle, NoodleGain);
    }

    //Destroys timer and creates noodles
    private void endDeAcidFlyCreation()
    {
        Gather(DeAcidFly, DeAcidFlyGain);
    }

    public void buyBlueFruit()
    {
        BuyFood(BlueFruit);
    }

    public void buyWater()
    {
        BuyFood(Water);
    }

    public void buyAcidFly()
    {
        BuyFood(AcidFly);
    }

    public void buyNoodle()
    {
        BuyFood(Noodle);
    }

    // Gather takes in a gather object counter as a GameObject and an amount of gain that object will have as an int
    private void Gather(Transform GatherObject, int GatherGain)
    {
        GameObject.Find("CraftingTable").GetComponent<AudioSource>().Stop();
        Player.GetComponent<PlayerBehavior>().controlMovement = true; //Allow the player from moving the Player character
        Player.GetComponent<CapsuleCollider2D>().enabled = true;
        if (GatherObject == null) //Check for Gather Object
        {
            Debug.LogError(name + " GatherObject could not be found after timer destruction.");
        }
        else
        {
            Transform GatheredObject = Instantiate(GatherObject, Player.position, GatherObject.rotation); //create gathered object on player
            GatheredObject.name = GatherObject.name; //set new objects name to be the same as the original
            GatheredObject.GetComponent<ItemBehavior>().ItemCount = GatherGain; //Set new objects count to be the corresponding GatherGain
            if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Ready To Cook") && !KitchenTable.GetComponent<StorageBehaviour>().isFull())
            {
                GatheredObject.transform.localScale = new Vector2(5, 5); //adjust the size of the new object
                if (KitchenTable.GetComponent<StorageBehaviour>().LeftObject == null)
                {
                    KitchenTable.GetComponent<StorageBehaviour>().LeftObject = GatheredObject;
                    GatheredObject.position = new Vector2(KitchenTable.transform.position.x - KitchenTable.GetComponent<SpriteRenderer>().bounds.size.x / 3, 
                        KitchenTable.GetComponent<StorageBehaviour>().Highlight.transform.position.y);
                }
                else if (KitchenTable.GetComponent<StorageBehaviour>().CenterObject == null)
                {
                    KitchenTable.GetComponent<StorageBehaviour>().CenterObject = GatheredObject;
                    GatheredObject.position = new Vector2(KitchenTable.transform.position.x, KitchenTable.GetComponent<StorageBehaviour>().Highlight.transform.position.y);
                }
                else
                {
                    KitchenTable.GetComponent<StorageBehaviour>().RightObject = GatheredObject;
                    GatheredObject.position = new Vector2(KitchenTable.transform.position.x + KitchenTable.GetComponent<SpriteRenderer>().bounds.size.x / 3, 
                        KitchenTable.GetComponent<StorageBehaviour>().Highlight.transform.position.y);
                }
            }
            else
            {
                GatheredObject.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
                bool check = Player.GetComponent<PlayerBehavior>().GiveObject(GatheredObject); //set Player to hold object
                if (!check)
                {
                    Debug.LogError(name + " attempted to give player " + GatherObject.name + " but player hand was full.");
                }
            }
            Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI
        }
    }

    private void CraftItem (Dictionary<Transform, int> Ingredients, string endCall, float TimeDelay)
    {
        foreach(Transform ingredient in Ingredients.Keys)
        {
            ingredient.GetComponent<ItemBehavior>().ItemCount += -Ingredients[ingredient];
        }

        myTimer = Instantiate(Timer, Player.transform.position, Timer.rotation); //create timer
        Player.GetComponent<GameManager>().Timers.Add(myTimer);
        myTimer.GetComponent<TimerBehavior>().startCounting(TimeDelay);

        Player.GetComponent<PlayerBehavior>().controlMovement = false; //Disallow the player from moving the Player character
        Player.GetComponent<CapsuleCollider2D>().enabled = false;

        GameObject.Find("CraftingTable").GetComponent<AudioSource>().Play();
        this.GetComponent<GameManager>().chopped++;
        Invoke(endCall, TimeDelay); //run function endBlueFruitJuiceCreation() after TimerDelay time
    }

    private void BuyFood (Transform BuyObject)
    {
        Player.GetComponent<PlayerBehavior>().controlMovement = true; //Allow the player from moving the Player character
        Player.GetComponent<CapsuleCollider2D>().enabled = true;
        if (BuyObject == null) //Check for Gather Object
        {
            Debug.LogError(name + " BuyObject could not be found.");
        }
        else
        {
            Transform BoughtObject = Instantiate(BuyObject, Player.position, BuyObject.rotation); //create gathered object on player
            BoughtObject.name = BuyObject.name; //set new objects name to be the same as the original
            BoughtObject.GetComponent<ItemBehavior>().ItemCount = 1;
            Player.GetComponent<PlayerBehavior>().money -= BoughtObject.GetComponent<ItemBehavior>().ItemValue;
            Player.GetComponent<GameManager>().purchases++;
            BoughtObject.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
            bool check = Player.GetComponent<PlayerBehavior>().GiveObject(BoughtObject); //set Player to hold object
            if (!check)
            {
                Debug.LogError(name + " attempted to give player " + BoughtObject.name + " but player hand was full.");
            }
            Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI
        }
    }

    private bool checkObject2(Dictionary<string, int> DesiredIngredients, Dictionary<Transform, int> GatheredObjects, Transform CraftingSurface, string endcall)
    {
        if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.GetComponent<ItemBehavior>().ItemCount))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name]);
            CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
            (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().RightObject.GetComponent<ItemBehavior>().ItemCount))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name]);
            CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
            (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.GetComponent<ItemBehavior>().ItemCount))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name]);
            CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
            return true;
        }
        return false;
    }

    private bool checkObject (Transform Ingredient, Dictionary<string, int> DesiredIngredients, Transform CraftingSurface, string endcall)
    {
        if (Ingredient != null && DesiredIngredients.ContainsKey(Ingredient.name) && DesiredIngredients[Ingredient.name] <= Ingredient.GetComponent<ItemBehavior>().ItemCount)
        {
            Dictionary<Transform, int> GatheredObjects = new Dictionary<Transform, int>();
            GatheredObjects.Add(Ingredient, DesiredIngredients[Ingredient.name]);
            DesiredIngredients.Remove(Ingredient.name);
            if (DesiredIngredients.Count == 0)
            {
                CraftItem(GatheredObjects, endcall, CraftingTimeDelay);
                return true;
            }
            else if (DesiredIngredients.Count == 1)
            {
                return checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
            }
            else
            {
                if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)) && 
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name] <= 
                    CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.GetComponent<ItemBehavior>().ItemCount))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name]);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name);
                    return checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().RightObject.GetComponent<ItemBehavior>().ItemCount))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name]);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name);
                    return checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.GetComponent<ItemBehavior>().ItemCount))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name]);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name);
                    checkObject2(DesiredIngredients, GatheredObjects, CraftingSurface, endcall);
                    return true;
                }
                return false;
            }
        }
        return false;
    }







    public void CreateSlicedBlueFruit(Transform CraftingSurface)
    {
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().LeftHandObject.name.Equals("Sliced Blue Fruit") ||
            Player.GetComponent<PlayerBehavior>().RightHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject.name.Equals("Sliced Blue Fruit"))) //Check for if timer isnt running
        {
            bool Created = false;
            Dictionary<string, int> Ingredients = new Dictionary<string, int>();
            Ingredients.Add("Blue Fruit", 1);
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
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().LeftHandObject.name.Equals("DeAcid Fly") || 
            Player.GetComponent<PlayerBehavior>().RightHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject.name.Equals("DeAcid Fly"))) //Check for if timer isnt running
        {
            bool Created = false;
            Dictionary<string, int> Ingredients = new Dictionary<string, int>();
            Ingredients.Add("Acid Fly", 1);
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
        if (myTimer == null && (Player.GetComponent<PlayerBehavior>().LeftHandObject == null || Player.GetComponent<PlayerBehavior>().LeftHandObject.name.Equals("Blue Fruit Juice") ||
            Player.GetComponent<PlayerBehavior>().RightHandObject == null || Player.GetComponent<PlayerBehavior>().RightHandObject.name.Equals("Blue Fruit Juice"))) //Check for if timer isnt running
        {
            bool Created = false;
            Dictionary<string, int> Ingredients = new Dictionary<string, int>();
            Ingredients.Add("Blue Fruit", 1);
            Ingredients.Add("WaterGlass", 3);
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
            this.GetComponent<GameManager>().gathered++;
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
            this.GetComponent<GameManager>().gathered++;
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
            this.GetComponent<GameManager>().gathered++;
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
            this.GetComponent<GameManager>().gathered++;
            Invoke("endNoodleGather", GatheringTimeDelay); //run function endTime() after TimerDelay time
        }
    }

    public void FruitBasket()
    {
        FruitGain++;
        Player.GetComponent<GameManager>().DoorPopup.GetChild(3).GetComponent<Info>().Description =
            "<b>Gather Blue Fruits</b> - Gather <color=#F7D64A>(" + FruitGain +
            ")</color> Blue Fruits from a nearby orchird of trees. They seem edible.";
    }

    public void Bucket()
    {
        WaterGain++;
        Player.GetComponent<GameManager>().DoorPopup.GetChild(4).GetComponent<Info>().Description =
            "<b>Gather Water</b> - Gather <color=#F7D64A>(" + WaterGain +
            ")</color> Buckets of Water from the nearby stream.";
    }

    public void Jar()
    {
        DeAcidFlyGain++;
        Player.GetComponent<GameManager>().CraftingPopup.GetChild(3).GetComponent<Info>().Description =
            "<b>Seperate Acid from Flies</b> - Violently shake <color=#F7D64A>(1)</color> Jar of Acid Flies to kill the fragile creatures. Then seperate the fly corpses from the acid and place them into <color=#F7D64A>(" +
            DeAcidFlyGain + ")</color> bowls.";
    }

    public void Strainer()
    {
        BlueFruitJuiceGain++;
        Player.GetComponent<GameManager>().CraftingPopup.GetChild(5).GetComponent<Info>().Description =
            "<b>Blue Fruit Juice</b> - Juice <color=#F7D64A>(1)</color> blue fruit into <color=#F7D64A>(" + BlueFruitJuiceGain +
            ")</color> glasses of water to create <color=#F7D64A>(3)</color> glasses of blue fruit juice.";
    }

    public void Flour()
    {
        PastaGain++;
        foreach (Transform cauldronPopup in Player.GetComponent<GameManager>().CauldronPopups)
        {
            cauldronPopup.GetChild(3).GetComponent<Info>().Description =
            "<b>Boil some pasta</b> - Use <color=#F7D64A>(3)</color> Dried Noodles from your hand to boil it into <color=#F7D64A>(" + PastaGain +
            ")</color> Bowls of Pasta.";
        }
    }

    public void Knives()
    {
        CraftingTimeDelay -= 1;
        if(CraftingTimeDelay < 0)
        {
            CraftingTimeDelay = 0;
        }
    }

    public void stopInvokes()
    {
        CancelInvoke();
    }
}
