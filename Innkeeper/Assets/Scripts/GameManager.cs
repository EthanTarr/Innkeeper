using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Transform> Tables;
    public List<Transform> DisSatisfiedCustomerCounters;

    public Dictionary<int, Transform> UnlockableFoods = new Dictionary<int, Transform>();

    //Spawning values
    public float MinSpawnTime = 60f;
    public float MaxSpawnTime = 80f;
    public float SpawnTimerIncreaseRate = 40f;
    public float SpawnTimerIncreaseAmount = .92f;
    private float startingIncreaseAmount;

    public int TimelineCount = 0;
    public int DayCount = 0;
    public int DayTimeLimit = 80;
    private int startingDayTimeLimit; 
    public int DayStartDelay = 15;
    private int startingDayStartDelay;

    public int numOfSatisfiedCustomers = 0;
    public int numOfDisSatisfiedCustomers = 0;
    public float CustomerSatisfactionXpBonus = 50f;

    public List<Transform> StorageTables;
    public Transform Customer;
    public Transform EndOfDayScreen;
    public Transform BlackBackground;
    public Transform GameOverScreen;
    public Transform UnlockedFoodScreen;
    public GameObject VoiceSlider;
    public GameObject DayCounter;
    public GameObject BlackFade;
    public GameObject SkillList;



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

    

    // Start is called before the first frame update
    void Start()
    {
        startingIncreaseAmount = SpawnTimerIncreaseAmount; //to use when reseting the game
        startingDayTimeLimit = DayTimeLimit;
        startingDayStartDelay = DayStartDelay;

        populateUnlockableFoods(); //creates a dictionary with |level to unlock| as int as key and |food to unlock| as transform as value
    }

    // Update is called once per frame
    void Update()
    {
        //Day ending catch
        if (TimelineCount > DayTimeLimit)
        {
            BlackBackground.gameObject.SetActive(true);
            EndOfDayScreen.gameObject.SetActive(true);
            this.gameObject.GetComponent<PlayerBehavior>().controlMovement = false;
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            this.GetComponent<Animator>().SetFloat("Speed", 0);
            this.GetComponent<AudioSource>().Stop();
            this.transform.position = new Vector2(-385, 32);
            GameObject.Find("Main Camera").transform.position = new Vector2(-385, 32);
            SkillList.SetActive(false);
            this.GetComponent<PlayerBehavior>().xp += numOfSatisfiedCustomers * CustomerSatisfactionXpBonus;
            EndOfDayScreen.GetComponent<EndOfDayBehavior>().SetUpEndOfDay(numOfSatisfiedCustomers, numOfDisSatisfiedCustomers, 
                this.GetComponent<PlayerBehavior>().PreviousXp, this.GetComponent<PlayerBehavior>().xp);
            ResetInn();
            StopAllCoroutines();
            TimelineCount = 0;
            GetComponent<PlayerBehavior>().Level = GetComponent<PlayerBehavior>().xpToLevels(GetComponent<PlayerBehavior>().xp);
            findUnlockedFood();
            Debug.Log("Day Over!");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            BlackFade.GetComponent<FadeBehavior>().ToggleFade();
        }

        //Fail day catch
        if (numOfDisSatisfiedCustomers >= 3)
        {
            this.gameObject.GetComponent<PlayerBehavior>().controlMovement = false;
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            ResetInn();
            StopAllCoroutines();
            TimelineCount = 0;
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
            DayTimeLimit += 20;
        }
        this.GetComponent<PlayerBehavior>().PreviousXp = this.GetComponent<PlayerBehavior>().xp;
        StartCoroutine(CountTimeline());
        StartCoroutine(SpawnIncrease());
        StartCoroutine(SpawnCustomer());
        this.gameObject.GetComponent<PlayerBehavior>().controlMovement = true;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        SpawnTimerIncreaseAmount += -.01f;
    }

    public void restart()
    {
        DayCount = 0;
        this.GetComponent<PlayerBehavior>().PreviousXp = 0;
        this.GetComponent<PlayerBehavior>().xp = 0;
        this.GetComponent<PlayerBehavior>().Level = 0;
        this.GetComponent<PlayerBehavior>().PlayerSkills = new List<string>();
        SpawnTimerIncreaseAmount = startingIncreaseAmount;
        DayTimeLimit = startingDayTimeLimit;
        DayStartDelay = startingDayStartDelay;
        UnlockableFoods = new Dictionary<int, Transform>();
        populateUnlockableFoods();
        ResetInn();
    }

    IEnumerator SpawnCustomer()
    {
        yield return new WaitForSeconds(DayStartDelay); //delay to start the day
        while (true)
        {
            float SpawnTime;
            List<Transform> emptyTables = findEmptyTable();
            int customerSpawn = Random.Range(1, (DayCount / 4) + 3);
            int count = 0;
            if (emptyTables.Count > 0)
            {
                while (count < customerSpawn)
                {
                    while (!emptyTables[Random.Range(0, emptyTables.Count - 1)].GetComponent<TableBehavior>().SpawnCustomer())
                    {
                        Debug.Log("Couldn't find a spot to spawn customers");
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

    private void populateUnlockableFoods()
    {
        UnlockableFoods.Add(3, GetComponent<ResourceManager>().PastaBowl);
        UnlockableFoods.Add(6, GetComponent<ResourceManager>().DeAcidFly);
    }

    private void findUnlockedFood()
    {
        int targetLevel = -1;
        foreach (int level in UnlockableFoods.Keys)
        {
            if(GetComponent<PlayerBehavior>().Level >= level)
            {
                targetLevel = level;
            }
        }
        if (targetLevel > -1)
        {
            UnlockedFoodScreen.GetChild(1).GetChild(0).GetComponent<Text>().text = UnlockableFoods[targetLevel].name + "!";
            UnlockedFoodScreen.GetChild(1).GetChild(1).GetComponent<Image>().sprite = UnlockableFoods[targetLevel].GetComponent<SpriteRenderer>().sprite;
            UnlockableFoods.Remove(targetLevel);
            UnlockedFoodScreen.gameObject.SetActive(true);
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

    public void ResetInn()
    {
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

        //get rid of stuff in player hands
        if (this.GetComponent<PlayerBehavior>().LeftHandObject != null)
        {
            this.GetComponent<PlayerBehavior>().MovementSpeed += Mathf.Max(this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemWeight -
                this.GetComponent<PlayerBehavior>().strength, 0) * this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
            this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount = 0;
        }
        if (this.GetComponent<PlayerBehavior>().RightHandObject != null)
        {
            this.GetComponent<PlayerBehavior>().MovementSpeed += Mathf.Max(this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemWeight -
                this.GetComponent<PlayerBehavior>().strength, 0) * this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount;
            this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount = 0;
        }
        this.GetComponent<PlayerBehavior>().checkHand();

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

        //get rid of objects on tables
        foreach (Transform storage in StorageTables)
        {
            if (storage.GetComponent<StorageBehaviour>().LeftObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().LeftObject.gameObject);
            }
            if (storage.GetComponent<StorageBehaviour>().CenterObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().CenterObject.gameObject);
            }
            if (storage.GetComponent<StorageBehaviour>().RightObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().RightObject.gameObject);
            }
        }

        //stop random audio
        GameObject.Find("CraftingTable").GetComponent<AudioSource>().Stop();
        GameObject.Find("Door Creak").GetComponent<AudioSource>().Stop();

        //reset cauldron
        GameObject.Find("Cauldron").GetComponent<CauldronBehavior>().ResetCauldron();
        GameObject.Find("Cauldron (1)").GetComponent<CauldronBehavior>().ResetCauldron();

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
            this.GetComponent<PlayerBehavior>().LevelChoices.GetComponent<LevelManager>().Calls[skill]();
        }
    }
}
