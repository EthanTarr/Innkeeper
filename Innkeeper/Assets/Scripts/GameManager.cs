using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Transform> Tables;
    public List<Transform> DisSatisfiedCustomerCounters;

    public float MinSpawnTime = 5f;
    public float MaxSpawnTime = 25f;

    public int TimelineCount = 0;
    public int DayCount = 0;

    public float SpawnTimerIncreaseRate = 30f;
    public float SpawnTimerIncreaseAmount = .2f;

    public int numOfSatisfiedCustomers = 0;
    public int numOfDisSatisfiedCustomers = 0;
    public float CustomerSatisfactionXpBonus = 50f;

    public List<Transform> StorageTables;
    public Transform Customer;
    public Transform EndOfDayScreen;
    public Transform BlackBackground;
    public Transform GameOverScreen;
    public GameObject VoiceSlider;
    public GameObject DayCounter;

    [HideInInspector] public List<Transform> Timers;
    [HideInInspector] public List<Transform> Customers = new List<Transform>();

    public float steps = 0;
    public float lifts = 0;
    public float chopped = 0;
    public float gathered = 0;
    public float cooked = 0;
    public float drakes = 0;
    public float antinium = 0;
    public float goblins = 0;
    public float customerWait = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(Customer == null)
        {
            Debug.LogError(name + " could not find Customer Prefab on startup");
        }
        if( EndOfDayScreen == null)
        {
            Debug.LogError(name + " could not find EndOfDayScreen on startup");
        }
        if (BlackBackground == null)
        {
            Debug.LogError(name + " could not find Black Background on startup");
        }
        start();
    }

    // Update is called once per frame
    void Update()
    {
        if(TimelineCount > 500)
        {
            this.gameObject.GetComponent<PlayerBehavior>().controlMovement = false;
            this.transform.position = new Vector2(-385, 32);
            GameObject.Find("Main Camera").transform.position = new Vector2(-385, 32);
            ResetInn();
            StopAllCoroutines();
            TimelineCount = 0;
            Debug.Log("GAME OVER!");
            BlackBackground.gameObject.SetActive(true);
            EndOfDayScreen.gameObject.SetActive(true);
            this.GetComponent<PlayerBehavior>().xp += numOfSatisfiedCustomers * CustomerSatisfactionXpBonus;
            EndOfDayScreen.GetComponent<EndOfDayBehavior>().SetUpEndOfDay(numOfSatisfiedCustomers, numOfDisSatisfiedCustomers, 
                this.GetComponent<PlayerBehavior>().PreviousXp, this.GetComponent<PlayerBehavior>().xp);
        }

        if(numOfDisSatisfiedCustomers >= 3)
        {
            this.gameObject.GetComponent<PlayerBehavior>().controlMovement = false;
            ResetInn();
            StopAllCoroutines();
            TimelineCount = 0;
            Debug.Log("GAME OVER!");
            BlackBackground.gameObject.SetActive(true);
            GameOverScreen.gameObject.SetActive(true);
        }

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
        this.GetComponent<PlayerBehavior>().PreviousXp = this.GetComponent<PlayerBehavior>().xp;
        numOfSatisfiedCustomers = 0;
        numOfDisSatisfiedCustomers = 0;
        StartCoroutine(CountTimeline());
        StartCoroutine(SpawnIncrease());
        StartCoroutine(SpawnCustomer());
        this.gameObject.GetComponent<PlayerBehavior>().controlMovement = true;
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            float SpawnTime;
            List<Transform> emptyTables = findEmptyTable();
            if (emptyTables.Count > 0)
            {
                while (!emptyTables[Random.Range(0, emptyTables.Count - 1)].GetComponent<TableBehavior>().SpawnCustomer())
                {
                    Debug.Log("Couldn't find a spot to spawn customers");
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
        this.GetComponent<ResourceManager>().stopInvokes();
        Customer.GetComponent<CustomerBehavior>().DrakeChance = Customer.GetComponent<CustomerBehavior>().OriginalDrakeChance;
        Customer.GetComponent<CustomerBehavior>().GoblinChance = Customer.GetComponent<CustomerBehavior>().OriginalGoblinChance;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance = Customer.GetComponent<CustomerBehavior>().OriginalAntiniumChance;
        foreach(Transform table in Tables)
        {
            if (table.GetComponent<TableBehavior>().CurrentCustomer != null)
            {
                Destroy(table.GetComponent<TableBehavior>().CurrentCustomer.gameObject);
                if (table.GetComponent<TableBehavior>().CurrentCustomer.GetComponent<PopUpObjectBehavior>().Popup.gameObject != null)
                {
                    Destroy(table.GetComponent<TableBehavior>().CurrentCustomer.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
                }
            }
            if (table.GetComponent<TableBehavior>().CurrentCustomer1 != null)
            {
                Destroy(table.GetComponent<TableBehavior>().CurrentCustomer1.gameObject);
                if (table.GetComponent<TableBehavior>().CurrentCustomer1.GetComponent<PopUpObjectBehavior>().Popup.gameObject != null)
                {
                    Destroy(table.GetComponent<TableBehavior>().CurrentCustomer1.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
                }
            }
            if (table.GetComponent<TableBehavior>().CurrentCustomer2 != null)
            {
                Destroy(table.GetComponent<TableBehavior>().CurrentCustomer2.gameObject);
                if (table.GetComponent<TableBehavior>().CurrentCustomer2.GetComponent<PopUpObjectBehavior>().Popup.gameObject != null)
                {
                    Destroy(table.GetComponent<TableBehavior>().CurrentCustomer2.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
                }
            }
        }
        foreach (Transform storage in StorageTables)
        {
            if (storage.GetComponent<StorageBehaviour>().LeftObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().LeftObject.gameObject);
            }
            else if (storage.GetComponent<StorageBehaviour>().CenterObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().CenterObject.gameObject);
            }
            else if (storage.GetComponent<StorageBehaviour>().RightObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().RightObject.gameObject);
            }
        }
        foreach (Transform timer in Timers)
        {
            if (timer != null)
            {
                Destroy(timer.gameObject);
            }
            else
            {
                Timers.Remove(timer);
            }
        }
        if (this.GetComponent<PlayerBehavior>().LeftHandObject != null)
        {
            this.GetComponent<PlayerBehavior>().MovementSpeed += -Mathf.Max(this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - 
                this.GetComponent<PlayerBehavior>().strength, 0) * this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
            this.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemCount = 0;
        }
        if (this.GetComponent<PlayerBehavior>().RightHandObject != null)
        {
            this.GetComponent<PlayerBehavior>().MovementSpeed += -Mathf.Max(this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemWeight -
                this.GetComponent<PlayerBehavior>().strength, 0) * this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount;
            this.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemCount = 0;
        }
        this.GetComponent<PlayerBehavior>().checkHand();
    }
}
