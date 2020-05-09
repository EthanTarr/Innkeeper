using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronBehavior : MonoBehaviour
{
    public Transform HeldItem;
    public Transform Highlight;
    public Transform CauldronPopup;
    public Sprite EmptyCauldron;
    public Sprite WaterCauldron;
    public Sprite BoilingWaterCauldron;
    public Sprite PastaCauldron;

    public Transform GlassWater;
    public int WaterGain = 5;
    public Transform PastaBowl;
    public int PastaGain = 3;

    //Timer
    private Transform myTimer = null;
    public Transform Timer;
    public float TimeDelay = 10f;

    public bool isEmpty = true;
    public bool isUnboiledWater = false;
    public bool isBoiledWater = false;
    public bool isCookingPasta = false;
    public bool isCookedPasta = false;

    public List<AudioClip> CauldronSounds;

    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if(Player == null)
        {
            Debug.LogError(name + " could not find player on startup");
        }
        if (Highlight == null)
        {
            Debug.LogError(name + " could not find Highlight on startup");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void grabItem()
    {
        bool check = Player.GetComponent<PlayerBehavior>().GiveObject(HeldItem);
        if(check)
        {
            HeldItem = null;
        }
        Player.GetComponent<PlayerBehavior>().checkHand();
    }

    public void grabGlassWater()
    {
        Transform thisGlassWater = Instantiate(GlassWater, Player.position, GlassWater.rotation); //create gathered object on player
        thisGlassWater.name = GlassWater.name; //set new objects name to be the same as the original
        thisGlassWater.GetComponent<ItemBehavior>().ItemCount = WaterGain; //Set new objects count to be the corresponding GatherGain
        thisGlassWater.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
        bool check = Player.GetComponent<PlayerBehavior>().GiveObject(thisGlassWater); //set Player to hold object
        if (!check)
        {
            Debug.LogError(name + " attempted to give player " + thisGlassWater.name + " but player hand was full.");
        }
        Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI

        isBoiledWater = false;
        isEmpty = true;
        this.GetComponent<SpriteRenderer>().sprite = EmptyCauldron;
        this.GetComponent<AudioSource>().clip = CauldronSounds[1];
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().Play();
    }

    public void grabPastaBowl()
    {
        Transform thisPastaBowl = Instantiate(PastaBowl, Player.position, PastaBowl.rotation); //create gathered object on player
        thisPastaBowl.name = PastaBowl.name; //set new objects name to be the same as the original
        thisPastaBowl.GetComponent<ItemBehavior>().ItemCount = PastaGain; //Set new objects count to be the corresponding GatherGain
        thisPastaBowl.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
        bool check = Player.GetComponent<PlayerBehavior>().GiveObject(thisPastaBowl); //set Player to hold object
        if (!check)
        {
            Debug.LogError(name + " attempted to give player " + thisPastaBowl.name + " but player hand was full.");
        }
        Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI

        isCookedPasta = false;
        isEmpty = true;
        this.GetComponent<SpriteRenderer>().sprite = EmptyCauldron;
        this.GetComponent<AudioSource>().Stop();
    }

    public void MakePasta()
    {
        if (isBoiledWater)
        {
            isBoiledWater = false;
            isCookingPasta = true;
            Highlight.gameObject.SetActive(false);
            this.GetComponent<SpriteRenderer>().sprite = PastaCauldron;
            Transform left = Player.GetComponent<PlayerBehavior>().LeftHandObject;
            Transform right = Player.GetComponent<PlayerBehavior>().RightHandObject;
            int NoodleLoss = 3;
            if (left && left.name.Equals("Noodles"))
            {
                if(left.GetComponent<ItemBehavior>().ItemCount > NoodleLoss)
                {
                    left.GetComponent<ItemBehavior>().ItemCount += -NoodleLoss;
                    NoodleLoss = 0;
                }
                else
                {
                    NoodleLoss += -left.GetComponent<ItemBehavior>().ItemCount;
                    left.GetComponent<ItemBehavior>().ItemCount = 0;
                }
                Player.GetComponent<PlayerBehavior>().MovementSpeed += Mathf.Max(left.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength, 0) * (3 - NoodleLoss);
            }
            if (right && right.name.Equals("Noodles") && NoodleLoss > 0)
            {
                right.GetComponent<ItemBehavior>().ItemCount += -NoodleLoss;
                Player.GetComponent<PlayerBehavior>().MovementSpeed += Mathf.Max(right.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength, 0) * NoodleLoss;
            }
            Player.GetComponent<PlayerBehavior>().checkHand();

            myTimer = Instantiate(Timer, this.transform.position, Timer.rotation); //create timer
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(TimeDelay);
            Invoke("CookedPasta", TimeDelay);
        }
        else
        {
            Debug.LogError("Tried to fill cauldron with pasta when it wasn't boiling.");
        }
    }

    public void getWater()
    {
        if(isEmpty)
        {
            isEmpty = false;
            isUnboiledWater = true;
            Highlight.gameObject.SetActive(false);
            this.GetComponent<SpriteRenderer>().sprite = WaterCauldron;
            this.GetComponent<AudioSource>().clip = CauldronSounds[0];
            this.GetComponent<AudioSource>().loop = true;
            this.GetComponent<AudioSource>().Play();
            Player.GetComponent<GameManager>().cooked++;

            myTimer = Instantiate(Timer, this.transform.position, Timer.rotation); //create timer
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(TimeDelay);
            Invoke("boiledWater", TimeDelay);
        }
        else
        {
            Debug.LogError("Tried to fill cauldron with water when it wasn't empty.");
        }
    }

    private void boiledWater()
    {
        isUnboiledWater = false;
        isBoiledWater = true;
        //this.GetComponent<SpriteRenderer>().sprite = BoilingWaterCauldron;
        checkForHighlights();
    }

    private void CookedPasta()
    {
        isCookingPasta = false;
        isCookedPasta = true;
        checkForHighlights();
    }

    private void checkForHighlights()
    {
        Transform LeftHand = Player.GetComponent<PlayerBehavior>().LeftHandObject;
        Transform RightHand = Player.GetComponent<PlayerBehavior>().RightHandObject;
        if ((isEmpty && ((LeftHand != null && LeftHand.name.Equals("Water")) || (RightHand != null && RightHand.name.Equals("Water")))) ||
            isBoiledWater || (isCookedPasta && (LeftHand == null || RightHand == null)))
        {
            Highlight.gameObject.SetActive(true);
            if (isBoiledWater)
            {
                CauldronPopup.gameObject.SetActive(true);
                if (LeftHand == null || RightHand == null)
                {
                    CauldronPopup.GetChild(4).GetComponent<Button>().interactable = true;
                }
                else
                {
                    CauldronPopup.GetChild(4).GetComponent<Button>().interactable = false;
                }

                int requiredNoodleCount = 3;
                if ((Player.GetComponent<PlayerBehavior>().Level > 2) &&
                    ((LeftHand != null && LeftHand.name.Equals("Noodles") && LeftHand.GetComponent<ItemBehavior>().ItemCount >= requiredNoodleCount) || 
                    (RightHand != null && RightHand.name.Equals("Noodles") && RightHand.GetComponent<ItemBehavior>().ItemCount >= requiredNoodleCount) ||
                    (LeftHand != null && RightHand != null && LeftHand.name.Equals("Noodles") && RightHand.name.Equals("Noodles") && 
                    ((LeftHand.GetComponent<ItemBehavior>().ItemCount + RightHand.GetComponent<ItemBehavior>().ItemCount) >= requiredNoodleCount))))
                {
                    CauldronPopup.GetChild(3).GetComponent<Button>().interactable = true;
                }
                else
                {
                    CauldronPopup.GetChild(3).GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkForHighlights();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Highlight.gameObject.SetActive(false);
        CauldronPopup.gameObject.SetActive(false);

        GameObject Tooltip = GameObject.Find("Tool Tip");
        if (Tooltip != null)
        {
            Tooltip.SetActive(false);
        }
    }
}
