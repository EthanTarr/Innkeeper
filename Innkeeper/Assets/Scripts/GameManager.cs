using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public List<Transform> Tables;
    public List<Transform> DisSatisfiedCustomerCounters;

    public Dictionary<int, string> UnlockableFoods = new Dictionary<int, string>();

    //Spawning values
    public float MinSpawnTime = 60f;
    private float OriginalMinSpawnTime = 60f;
    public float MaxSpawnTime = 80f;
    private float OriginalMaxSpawnTime = 80f;
    public float SpawnTimerIncreaseRate = 40f;
    public float SpawnTimerIncreaseAmount = .92f;

    public int TimelineCount = 0;
    public int DayCount = 0;
    public int DayTimeLimit = 80; 
    public int DayStartDelay = 15;

    public List<Transform> FoodNeedingUnlocks;
    private List<string> UnlockedFoods;

    public int numOfSatisfiedCustomers = 0;
    public int numOfDisSatisfiedCustomers = 0;
    public float CustomerSatisfactionXpBonus = 50f;

    public bool canAnyMeal = false;

    public List<Transform> StorageTables;
    public List<Transform> CauldronPopups;
    public Transform CraftingPopup;
    public Transform DoorPopup;
    public Transform Customer;
    public Transform EndOfDayScreen;
    public Transform BlackBackground;
    public Transform GameOverScreen;
    public Transform UnlockedFoodScreen;
    public Transform LevelChoices;
    public Transform TitleScreen;
    public Transform MarketScreen;
    public Transform PurchaseBoxPupup;
    public Transform DashIndicator;
    public Transform AnyMealWillDoIndicator;
    public GameObject VoiceSlider;
    public GameObject DayCounter;
    public GameObject BlackFade;
    public GameObject SkillList;
    public GameObject PauseScreen;



    [HideInInspector] public List<Transform> Timers;
    [HideInInspector] public List<Transform> Customers = new List<Transform>();

    //Stats
    public float steps = 0;
    public float lifts = 0;
    public float chopped = 0;
    public float gathered = 0;
    public float cooked = 0;
    public float drakes = 0;
    public float antinium = 0;
    public float goblins = 0;
    public float customerWait = 0;
    public float customerSteps = 0;
    public float purchases = 0;
    public float ExpensiveFood = 0;
    public float MarketTime = 0;
    public float numofDisatisfiedCustomers = 0;
    public float numofSatisfiedCustomers = 0;
    public float leftovers = 0;
    public float cauldronFilled = 0;
    public float cauldronBoiled = 0;
    public float mealsServed = 0;



    // Start is called before the first frame update
    void Start()
    {

        populateUnlockableFoods(); //creates a dictionary with |level to unlock| as int as key and |food to unlock| as transform as value
        SaveLoad.SaveBlank();
    }

    // Update is called once per frame
    void Update()
    {
        //Day ending catch
        if (TimelineCount > DayTimeLimit)
        {
            BlackBackground.gameObject.SetActive(true);
            EndOfDayScreen.gameObject.SetActive(true);
            this.GetComponent<PlayerBehavior>().xp += numOfSatisfiedCustomers * CustomerSatisfactionXpBonus;
            numofDisatisfiedCustomers += numOfDisSatisfiedCustomers;
            numofSatisfiedCustomers += numOfSatisfiedCustomers;
            EndOfDayScreen.GetComponent<EndOfDayBehavior>().SetUpEndOfDay(numOfSatisfiedCustomers, numOfDisSatisfiedCustomers, 
                this.GetComponent<PlayerBehavior>().PreviousXp, this.GetComponent<PlayerBehavior>().xp);
            ResetInn();
            GetComponent<PlayerBehavior>().Level = GetComponent<PlayerBehavior>().xpToLevels(GetComponent<PlayerBehavior>().xp);
            findUnlockedFood();
            Debug.Log("Day Over!");
        }

        //Fail day catch
        if (numOfDisSatisfiedCustomers >= DisSatisfiedCustomerCounters.Count)
        {
            ResetInn();
            Debug.Log("GAME OVER!");
            BlackBackground.gameObject.SetActive(true);
            GameOverScreen.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Too many angry customers. The Inn is ruined! And in only " + DayCount + " day(s)";
            GameOverScreen.gameObject.SetActive(true);
        }

        //Ambient inn sounds catch
        GameObject crowdSound = GameObject.Find("AmbientCrowdSound");
        crowdSound.GetComponent<SoundManager>().MaxVolume = Customers.Count * .2f;
        crowdSound.GetComponent<SoundManager>().AudioControl(VoiceSlider.GetComponent<Slider>().value);
        if (Customers.Count > 1 && !crowdSound.GetComponent<AudioSource>().isPlaying)
        {
            crowdSound.GetComponent<AudioSource>().Play();
        }
        else if (Customers.Count <= 1)
        {
            crowdSound.GetComponent<AudioSource>().Stop();
        }

        //Any Meal Will Do Skill
        if(canAnyMeal && Input.GetKey(KeyCode.Z))
        {
            canAnyMeal = false;
            AnyMealWillDoIndicator.GetComponent<Image>().color = new Color(255, 255, 255, 100);
            StartCoroutine("AnyMealWillDoRecharge");
        }
    }

    public void loadGame()
    {
        SaveLoad.Load();
        start();
    }

    public void start()
    {
        DayCount++;
        DayCounter.GetComponent<Text>().text = DayCount + "";
        if (DayCount == 2)
        {
            DayTimeLimit = 180;
        }
        else
        {
            DayTimeLimit += 10;
        }
        this.GetComponent<PlayerBehavior>().PreviousXp = this.GetComponent<PlayerBehavior>().xp;
        OriginalMaxSpawnTime = MaxSpawnTime;
        OriginalMinSpawnTime = MinSpawnTime;
        StartCoroutine(CountTimeline());
        StartCoroutine(SpawnIncrease());
        StartCoroutine(SpawnCustomer());
        this.gameObject.GetComponent<PlayerBehavior>().controlMovement = true;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        SpawnTimerIncreaseAmount += -.01f;
        SaveLoad.Save();
    }

    public void restart()
    {
        ResetInn();
        SaveLoad.Load("/blankGame.ent");
        populateUnlockableFoods();
    }

    public bool StorageTableContains (Sprite Object)
    {
        foreach(Transform store in StorageTables)
        {
            if(store.GetComponent<StorageBehaviour>().contains(Object))
            {
                return true;
            }
        }
        return false;
    }

    public Transform StorageTableRetrieve (Sprite Object)
    {
        foreach (Transform store in StorageTables)
        {
            if (store.GetComponent<StorageBehaviour>().contains(Object))
            {
                return store.GetComponent<StorageBehaviour>().retrieve(Object);
            }
        }
        return null;
    }

    IEnumerator SpawnCustomer()
    {
        yield return new WaitForSeconds(DayStartDelay); //delay to start the day
        while (true)
        {
            float SpawnTime;
            List<Transform> emptyTables = findEmptyTable();
            int customerSpawn = Random.Range(1, (DayCount / 8) + 3);
            int count = 0;
            if (emptyTables.Count > 0)
            {
                while (count < customerSpawn)
                {
                    int bounds = 0;
                    while (!emptyTables[Random.Range(0, emptyTables.Count)].GetComponent<TableBehavior>().SpawnCustomer() && bounds < emptyTables.Count)
                    {
                        Debug.Log("Couldn't find a spot to spawn customers");
                        bounds++;
                    }
                    count++;
                    yield return new WaitForSeconds(3);
                }
            }
            SpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
            yield return new WaitForSeconds(SpawnTime); //wait for spawntime
        }
    }

    IEnumerator SpawnIncrease()
    {
        while (true)
        {
            MinSpawnTime *= SpawnTimerIncreaseAmount;
            MaxSpawnTime *= SpawnTimerIncreaseAmount;
            yield return new WaitForSeconds(SpawnTimerIncreaseRate);
        }
    }

    IEnumerator CountTimeline()
    {
        while(true)
        {
            TimelineCount++;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator AnyMealWillDoRecharge()
    {
        yield return new WaitForSeconds(60f - 2f * this.GetComponent<PlayerBehavior>().Level);
        canAnyMeal = true;
        AnyMealWillDoIndicator.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void createDisSatisfiedCustomerCounters()
    {
        Transform customerCounter = GameObject.Find("Upset Customer Counter").transform.GetChild(0);
        Transform newCounter = Instantiate(customerCounter, customerCounter.position + new Vector3(-50 * customerCounter.parent.childCount, 0, 0), customerCounter.transform.rotation);
        newCounter.parent = customerCounter.parent;
        DisSatisfiedCustomerCounters.Add(newCounter);
    }

    public List<bool> encodeActiveSkills()
    {
        List<bool> skillActives = new List<bool>();
        skillActives.Add(Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).gameObject.activeSelf);
        skillActives.Add(DashIndicator.gameObject.activeSelf);
        foreach (Transform popup in CauldronPopups)
        {
            skillActives.Add(popup.GetChild(5).gameObject.activeSelf);
        }
        foreach (Transform popup in CauldronPopups)
        {
            skillActives.Add(popup.GetChild(6).gameObject.activeSelf);
        }
        skillActives.Add(AnyMealWillDoIndicator.gameObject.activeSelf);
        return skillActives;
    }

    public void decodeActiveSkills(List<bool> skillActives)
    {
        Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).gameObject.SetActive(skillActives[0]);
        DashIndicator.gameObject.SetActive(skillActives[1]);
        foreach (Transform popup in CauldronPopups)
        {
            popup.GetChild(5).gameObject.SetActive(skillActives[2]);
        }
        foreach (Transform popup in CauldronPopups)
        {
            popup.GetChild(6).gameObject.SetActive(skillActives[3]);
        }
        AnyMealWillDoIndicator.gameObject.SetActive(skillActives[4]);
    }

    private void populateUnlockableFoods()
    {
        UnlockableFoods.Add(3, GetComponent<ResourceManager>().PastaBowl.name);
        UnlockableFoods.Add(6, GetComponent<ResourceManager>().DeAcidFly.name);
    }

    private void findUnlockedFood()
    {
        int targetLevel = -1;
        UnlockedFoods = new List<string>();
        List<int> removes = new List<int>();
        MarketScreen.GetChild(1).GetChild(0).GetChild(0).GetComponent<MarketBehavior>().LevelCheck(this.GetComponent<PlayerBehavior>().Level);
        foreach (int level in UnlockableFoods.Keys)
        {
            if(GetComponent<PlayerBehavior>().Level >= level)
            {
                targetLevel = level;
                removes.Add(level);
                UnlockedFoods.Add(UnlockableFoods[targetLevel]);
            }
        }
        if (targetLevel > -1)
        {
            UnlockedFoodScreen.GetChild(1).GetChild(0).GetComponent<Text>().text = UnlockedFoods[0] + "!";
            foreach(Transform food in FoodNeedingUnlocks)
            {
                if(food.name.Equals(UnlockedFoods[0]))
                {
                    UnlockedFoodScreen.GetChild(1).GetChild(1).GetComponent<Image>().sprite = food.GetComponent<SpriteRenderer>().sprite;
                }
            }
            UnlockedFoods.Remove(UnlockedFoods[0]);
            UnlockedFoodScreen.gameObject.SetActive(true);
        }
        foreach(int lev in removes)
        {
            UnlockableFoods.Remove(lev);
        }
    }

    public void Unlockfoods()
    {
        if (UnlockedFoods.Count > 0)
        {
            UnlockedFoodScreen.GetChild(1).GetChild(0).GetComponent<Text>().text = UnlockedFoods[0] + "!";
            foreach (Transform food in FoodNeedingUnlocks)
            {
                if (food.name.Equals(UnlockedFoods[0]))
                {
                    UnlockedFoodScreen.GetChild(1).GetChild(1).GetComponent<Image>().sprite = food.GetComponent<SpriteRenderer>().sprite;
                }
            }
            UnlockedFoods.Remove(UnlockedFoods[0]);
        }
        else
        {
            UnlockedFoodScreen.gameObject.SetActive(false);
            MarketScreen.GetChild(3).GetComponent<Button>().interactable = true;
        }
    }

    public List<int[]> tableToSave()
    {
        List<int[]> Tables = new List<int[]>();
        foreach(Transform table in StorageTables)
        {
            Tables.Add(table.GetComponent<StorageBehaviour>().Encode());
        }
        return Tables;
    }

    public void savetoTable(List<int[]> tables)
    {
        for (int i = 0; i < StorageTables.Count; i++)
        {
            StorageTables[i].GetComponent<StorageBehaviour>().Decode(tables[i]);
        }
    }

    private List<Transform> findEmptyTable()
    {
        List<Transform> emptyTables = new List<Transform>();
        foreach(Transform table in Tables)
        {
            if((!table.GetComponent<TableBehavior>().isStool && (table.GetComponent<TableBehavior>().CurrentCustomer == null || table.GetComponent<TableBehavior>().CurrentCustomer1 == null || 
                table.GetComponent<TableBehavior>().CurrentCustomer2 == null)) || (table.GetComponent<TableBehavior>().isStool && table.GetComponent<TableBehavior>().CurrentCustomer == null))
            {
                emptyTables.Add(table);
            }
        }
        return emptyTables;
    }

    public void FixTable()
    {
        Transform Table1 = GameObject.Find("Table").transform;
        if (!Table1.GetComponent<SpriteRenderer>().sprite.Equals(Tables[0].GetComponent<SpriteRenderer>().sprite))
        {
            Table1.GetComponent<SpriteRenderer>().sprite = Tables[0].GetComponent<SpriteRenderer>().sprite;
            Tables.Add(Table1);
        }
        else
        {
            Transform Table2 = GameObject.Find("Table (2)").transform;
            Table2.GetComponent<SpriteRenderer>().sprite = Tables[0].GetComponent<SpriteRenderer>().sprite;
            Tables.Add(Table2);
        }
    }

    public void FixStool()
    {
        Transform Stool1 = GameObject.Find("Stool (2)").transform;
        Transform Stool2 = GameObject.Find("Stool (3)").transform;
        if (!Stool1.GetComponent<SpriteRenderer>().sprite.Equals(Tables[1].GetComponent<SpriteRenderer>().sprite))
        {
            Stool1.GetComponent<SpriteRenderer>().sprite = Tables[1].GetComponent<SpriteRenderer>().sprite;
            Tables.Add(Stool1);
        }
        else if(!Stool2.GetComponent<SpriteRenderer>().sprite.Equals(Tables[1].GetComponent<SpriteRenderer>().sprite))
        {
            Stool2.GetComponent<SpriteRenderer>().sprite = Tables[1].GetComponent<SpriteRenderer>().sprite;
            Tables.Add(Stool2);
        }
        else
        {
            Transform Stool3 = GameObject.Find("Stool (4)").transform;
            Stool3.GetComponent<SpriteRenderer>().sprite = Tables[1].GetComponent<SpriteRenderer>().sprite;
            Tables.Add(Stool3);
        }
    }

    public void Sign()
    {
        Customer.GetComponent<CustomerBehavior>().DrakeChance -= 17;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += 33;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance -= 17;
        if (this.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Antinium"))
        {
            Customer.GetComponent<CustomerBehavior>().GoblinChance -= 1 * this.GetComponent<PlayerBehavior>().Level;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += 1 * this.GetComponent<PlayerBehavior>().Level;
        }
        else if (this.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Drake"))
        {
            Customer.GetComponent<CustomerBehavior>().GoblinChance -= 1 * this.GetComponent<PlayerBehavior>().Level;
            Customer.GetComponent<CustomerBehavior>().DrakeChance += 1 * this.GetComponent<PlayerBehavior>().Level;
        }
        else
        {
            LevelChoices.GetComponent<LevelManager>().AvaliableSkills.Add(LevelChoices.GetComponent<LevelManager>().SkillDictionary["Customer Preference - Goblin"]);
        }
    }

    public void ResetInn()
    {
        //reset player and camera position and disable them
        this.GetComponent<Animator>().SetFloat("Speed", 0);
        this.GetComponent<AudioSource>().Stop();
        this.transform.position = new Vector2(-385, 32);
        GameObject.Find("Main Camera").transform.position = new Vector2(-385, 32);
        this.gameObject.GetComponent<PlayerBehavior>().controlMovement = false;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

        //turn off skills list if open
        SkillList.SetActive(false);

        //stops the recharge of the dashing ability
        if(this.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Diner Dash"))
        {
            this.GetComponent<PlayerBehavior>().stopDashRecharge();
        }

        //stop counitng time, and spawining customers
        StopAllCoroutines();
        TimelineCount = 0;
        MinSpawnTime = OriginalMinSpawnTime;
        MaxSpawnTime = OriginalMaxSpawnTime;

        //stop resource gathering or processing
        this.GetComponent<ResourceManager>().stopInvokes();

        //get rid of the customers
        Customers = new List<Transform>();

        //reset customer counters
        numOfSatisfiedCustomers = 0;
        numOfDisSatisfiedCustomers = 0;

        //reset player lives
        GameObject customerCounterParent = GameObject.Find("Upset Customer Counter");
        for (int i = 0; i <  GameObject.Find("Upset Customer Counter").transform.childCount; i++)
        {
            customerCounterParent.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        //if the player doesn't have the Field of Preservation skill
        if (!this.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Field of Preservation"))
        {
            //get rid of stuff in player hands
            if (this.GetComponent<PlayerBehavior>().LeftHandObject != null)
            {
                leftovers += this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                this.GetComponent<PlayerBehavior>().MovementSpeed += Mathf.Max(this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemWeight -
                    this.GetComponent<PlayerBehavior>().strength, 0) * this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount = 0;
            }
            if (this.GetComponent<PlayerBehavior>().RightHandObject != null)
            {
                leftovers += this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                this.GetComponent<PlayerBehavior>().MovementSpeed += Mathf.Max(this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemWeight -
                    this.GetComponent<PlayerBehavior>().strength, 0) * this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount = 0;
            }
            this.GetComponent<PlayerBehavior>().checkHand();

            //get rid of objects on tables
            foreach (Transform storage in StorageTables)
            {
                if (storage.GetComponent<StorageBehaviour>().LeftObject != null)
                {
                    leftovers += storage.GetComponent<StorageBehaviour>().LeftObject.GetComponent<ItemBehavior>().ItemCount;
                    Destroy(storage.GetComponent<StorageBehaviour>().LeftObject.gameObject);
                }
                if (storage.GetComponent<StorageBehaviour>().CenterObject != null)
                {
                    leftovers += storage.GetComponent<StorageBehaviour>().CenterObject.GetComponent<ItemBehavior>().ItemCount;
                    Destroy(storage.GetComponent<StorageBehaviour>().CenterObject.gameObject);
                }
                if (storage.GetComponent<StorageBehaviour>().RightObject != null)
                {
                    leftovers += storage.GetComponent<StorageBehaviour>().RightObject.GetComponent<ItemBehavior>().ItemCount;
                    Destroy(storage.GetComponent<StorageBehaviour>().RightObject.gameObject);
                }
            }
        }

        //get rid of customers at tables
        foreach (Transform table in Tables)
        {
            if (table.GetComponent<TableBehavior>().CurrentCustomer != null)
            {
                if (table.GetComponent<TableBehavior>().CurrentCustomer.GetComponent<PopUpObjectBehavior>().Popup != null)
                {
                    Destroy(table.GetComponent<TableBehavior>().CurrentCustomer.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
                }
                Destroy(table.GetComponent<TableBehavior>().CurrentCustomer.gameObject);
            }
            if (table.GetComponent<TableBehavior>().CurrentCustomer1 != null)
            {
                if (table.GetComponent<TableBehavior>().CurrentCustomer1.GetComponent<PopUpObjectBehavior>().Popup != null)
                {
                    Destroy(table.GetComponent<TableBehavior>().CurrentCustomer1.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
                }
                Destroy(table.GetComponent<TableBehavior>().CurrentCustomer1.gameObject);
            }
            if (table.GetComponent<TableBehavior>().CurrentCustomer2 != null)
            {
                if (table.GetComponent<TableBehavior>().CurrentCustomer2.GetComponent<PopUpObjectBehavior>().Popup != null)
                {
                    Destroy(table.GetComponent<TableBehavior>().CurrentCustomer2.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
                }
                Destroy(table.GetComponent<TableBehavior>().CurrentCustomer2.gameObject);
            }
        }

        //stop random audio
        GameObject.Find("CraftingTable").GetComponent<AudioSource>().Stop();
        GameObject.Find("Door Creak").GetComponent<AudioSource>().Stop();

        //reset cauldron
        GameObject.Find("Cauldron").GetComponent<CauldronBehavior>().ResetCauldron();
        GameObject ExtraCauldron = GameObject.Find("Cauldron (1)");
        if (ExtraCauldron != null) {
            ExtraCauldron.GetComponent<CauldronBehavior>().ResetCauldron();
        }

        //reset player position and animations
        this.transform.position = new Vector2(-385, 32);
        this.GetComponent<SpriteRenderer>().sprite = this.GetComponent<PlayerBehavior>().FrontErin;
        this.GetComponent<SpriteRenderer>().flipX = false;
        this.GetComponent<Animator>().SetBool("Forward", true);
        this.GetComponent<Animator>().SetBool("Backward", false);
        this.GetComponent<Animator>().SetBool("Sideways", false);

        //get rid of all timers
        foreach (Transform timer in Timers)
        {
            if (timer != null)
            {
                Destroy(timer.gameObject);
            }
        }
        Timers = new List<Transform>();

        //check skills for level up increases
        foreach (string skill in this.GetComponent<PlayerBehavior>().PlayerSkills)
        {
            LevelChoices.GetComponent<LevelManager>().Calls[skill]();
        }
    }
}
