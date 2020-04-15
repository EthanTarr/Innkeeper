using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> Tables;

    public float MinSpawnTime = 5f;
    public float MaxSpawnTime = 25f;

    public int TimelineCount = 0;

    public float SpawnTimerIncreaseRate = 30f;
    public float SpawnTimerIncreaseAmount = .2f;

    public int numOfSatisfiedCustomers = 0;
    public int numOfDisSatisfiedCustomers = 0;
    public float CustomerSatisfactionXpBonus = 50f;

    public List<Transform> StorageTables;
    public Transform Customer;
    public Transform EndOfDayScreen;
    public Transform BlackBackground;

    [HideInInspector] public List<Transform> Timers;

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
        if(TimelineCount > 200)
        {
            this.gameObject.GetComponent<PlayerBehavior>().controlMovement = false;
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
    }

    public void start()
    {
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
            List<Transform> emptyTables = findEmptyTable();
            if(emptyTables.Count > 0)
            {
                emptyTables[Random.Range(0, emptyTables.Count - 1)].GetComponent<TableBehavior>().SpawnCustomer();
            }
            float SpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
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
            if(table.GetComponent<TableBehavior>().CurrentCustomer == null)
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
