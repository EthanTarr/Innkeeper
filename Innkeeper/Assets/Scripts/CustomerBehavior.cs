using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBehavior : MonoBehaviour
{
    //Timer
    private Transform myTimer = null;
    public Transform Timer;

    public List<Sprite> Customers;

    public float MovementSpeed = 15f; //movement speed of the customer character
    public float LifeTimer = 60f; //seconds customer will live
    public List<Transform> PossibleMeals;

    public Transform Table;

    [HideInInspector] public int OriginalDrakeChance = 33;
    [HideInInspector] public int OriginalGoblinChance = 33;
    [HideInInspector] public int OriginalAntiniumChance = 33;
    public int DrakeChance = 33;
    public int GoblinChance = 33;
    public int AntiniumChance = 33;

    private Vector2 Destination;

    private bool hasntArrived = true;
    private bool returning = false;
    private List<Vector2> Path = null;
    private int node = 1;

    private Transform Player;
    private int thisCustomer;

    private float sitTime;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if (Player == null)
        {
            Debug.LogError(name + " could not find Player on startup");
        }

        Player.GetComponent<GameManager>().Customers.Add(this.transform);

        int CustomerChance = Random.Range(0, DrakeChance + GoblinChance + AntiniumChance);
        if(CustomerChance < DrakeChance)
        {
            thisCustomer = 0;
        }
        else if (CustomerChance < DrakeChance + GoblinChance)
        {
            thisCustomer = 3;
        }
        else
        {
            thisCustomer = 6;
        }
        this.GetComponent<SpriteRenderer>().sprite = Customers[thisCustomer];
        List<Sprite> Meals = new List<Sprite>();
        if (Customers[thisCustomer].name.Equals("drake")) //drake
        {
            Meals.Add(PossibleMeals[0].GetComponent<SpriteRenderer>().sprite);
            Meals.Add(PossibleMeals[1].GetComponent<SpriteRenderer>().sprite);
            Meals.Add(PossibleMeals[3].GetComponent<SpriteRenderer>().sprite);
            if (Player.GetComponent<PlayerBehavior>().Level > 2)
            {
                Meals.Add(PossibleMeals[4].GetComponent<SpriteRenderer>().sprite);
            }
            Player.GetComponent<GameManager>().drakes++;
            /*
            LifeTimer = LifeTimer;
            this.GetComponents<BoxCollider2D>()[0].offset = new Vector2(0f, 0f);
            this.GetComponents<BoxCollider2D>()[0].size = new Vector2(2.8f, 5.6f);
            this.GetComponents<BoxCollider2D>()[1].offset = new Vector2(0f, 1.4f);
            this.GetComponents<BoxCollider2D>()[1].size = new Vector2(1.6f, 2.2f);
            */
        }
        else if(Customers[thisCustomer].name.Equals("FrontAntinium"))  //antinium
        {
            Meals.Add(PossibleMeals[1].GetComponent<SpriteRenderer>().sprite);
            if (Player.GetComponent<PlayerBehavior>().Level > 5)
            {
                Meals.Add(PossibleMeals[2].GetComponent<SpriteRenderer>().sprite);
            }
            Meals.Add(PossibleMeals[3].GetComponent<SpriteRenderer>().sprite);
            Player.GetComponent<GameManager>().antinium++;
            LifeTimer = LifeTimer - 15f;
            /*
            this.GetComponents<BoxCollider2D>()[0].offset = new Vector2(0f, 0f);
            this.GetComponents<BoxCollider2D>()[0].size = new Vector2(2.8f, 5.6f);
            this.GetComponents<BoxCollider2D>()[1].offset = new Vector2(0f, 1.4f);
            this.GetComponents<BoxCollider2D>()[1].size = new Vector2(1.6f, 2.2f);
            */
        }
        else if(Customers[thisCustomer].name.Equals("FrontGoblin")) //goblin
        {
            Meals.Add(PossibleMeals[0].GetComponent<SpriteRenderer>().sprite);
            Meals.Add(PossibleMeals[1].GetComponent<SpriteRenderer>().sprite);
            if (Player.GetComponent<PlayerBehavior>().Level > 5)
            {
                Meals.Add(PossibleMeals[2].GetComponent<SpriteRenderer>().sprite);
            }
            Meals.Add(PossibleMeals[3].GetComponent<SpriteRenderer>().sprite);
            if (Player.GetComponent<PlayerBehavior>().Level > 2)
            {
                Meals.Add(PossibleMeals[4].GetComponent<SpriteRenderer>().sprite);
            }
            Player.GetComponent<GameManager>().goblins++;
            LifeTimer = LifeTimer + 15f;
            this.GetComponents<BoxCollider2D>()[0].offset = new Vector2(0f, 0.45f);
            this.GetComponents<BoxCollider2D>()[0].size = new Vector2(2.5f, 4f);
            this.GetComponents<BoxCollider2D>()[1].offset = new Vector2(0f, 1.1f);
            this.GetComponents<BoxCollider2D>()[1].size = new Vector2(1.6f, .5f);
        }
        for(int i = 0; i < GetComponent<PopUpObjectBehavior>().Popup.GetChild(0).GetChild(0).GetChild(0).childCount; i++)
        {
            if (Meals.Count > 0)
            {
                Sprite chosen = GetComponent<PopUpObjectBehavior>().Popup.GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetComponent<CustomerRequestBehavior>().SetItem(Meals);
                Meals.Remove(chosen);
            }
        }

        MovementSpeed += (Player.GetComponent<GameManager>().DayCount * .5f) + Player.GetComponent<GameManager>().TimelineCount * .05f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs((transform.position - (Vector3)Destination).magnitude) > .1f) //if customer is farther than .1 from destination (Optimize)
        {
            Vector2 move = Vector2.MoveTowards(transform.position, Destination, MovementSpeed * Time.deltaTime);
            Vector2 moveTowards = Destination - (Vector2)transform.position;
            if (Mathf.Abs(moveTowards.x) > Mathf.Abs(moveTowards.y))
            {
                this.GetComponent<SpriteRenderer>().sprite = Customers[thisCustomer + 1];
                if (moveTowards.x < 0)
                {
                    this.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else
            {
                if (moveTowards.y < 0)
                {
                    this.GetComponent<SpriteRenderer>().sprite = Customers[thisCustomer];
                    this.GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = Customers[thisCustomer + 2];
                    this.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            transform.position = move; //move customer towards destination
        }
        else if (Path.Count > node && !returning)
        {
            Destination = Path[node];
            node++;
        }
        else if (node >= 0 && returning)
        {
            Destination = Path[node];
            node--;
        }
        else if (hasntArrived)
        {
            hasntArrived = !hasntArrived;
            ArrivedAtDestination();
        }
    }

    public void setPath(List<Vector2> Path)
    {
        this.Path = Path;
        Destination = Path[node];
        node++;
    }

    private void ArrivedAtDestination()
    {
        if (!returning)
        {
            myTimer = Instantiate(Timer, this.transform.position, Timer.rotation); //create timer
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(LifeTimer);
            sitTime = Time.time;
            Invoke("DisapointedCustomer", LifeTimer);
        }
        else
        {
            Player.GetComponent<GameManager>().Customers.Remove(this.transform);
            Destroy(this.gameObject);
        }
    }

    private void DisapointedCustomer()
    {
        Player.GetComponent<GameManager>().numOfDisSatisfiedCustomers++;
        for (int i = 0; i < Player.GetComponent<GameManager>().DisSatisfiedCustomerCounters.Count; i++) {
            if (Player.GetComponent<GameManager>().DisSatisfiedCustomerCounters[i].GetComponent<Image>().color.a == (100/255f))
            {
                Player.GetComponent<GameManager>().DisSatisfiedCustomerCounters[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                break;
            }
        }
        SendCustomerAway();
    }

    public void SendCustomerAway()
    {
        CancelInvoke();
        Player.GetComponent<GameManager>().customerWait += Time.time - sitTime;
        if (myTimer != null)
        {
            Player.GetComponent<GameManager>().Timers.Remove(myTimer);
            Destroy(myTimer.gameObject);
        }
        Destroy(this.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
        this.GetComponent<BoxCollider2D>().enabled = false;
        returning = true;
        hasntArrived = true;
        node--;
        Destination = Path[node];
        node--;
    }
}
