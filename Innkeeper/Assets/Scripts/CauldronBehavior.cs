using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronBehavior : MonoBehaviour
{
    public Transform Highlight;
    public Transform CauldronPopup;
    public Sprite EmptyCauldron;
    public Sprite WaterCauldron;
    public Sprite BoilingWaterCauldron;
    public Sprite PastaCauldron;
    public Sprite CookingPastaCauldron;

    //Timer
    private Transform myTimer = null;
    public Transform Timer;
    

    public bool isEmpty = true;
    public bool isUnboiledWater = false;
    public bool isBoiledWater = false;
    public bool isCookingPasta = false;
    public bool isCookedPasta = false;

    public List<AudioClip> CauldronSounds;

    private Transform Player;

    private bool isCollidingWithPlayer = false;

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

    public void ResetCauldron()
    {
        isEmpty = true;
        isUnboiledWater = false;
        isBoiledWater = false;
        isCookingPasta = false;
        isCookedPasta = false;
        this.GetComponent<Animator>().enabled = false;
        this.GetComponent<SpriteRenderer>().sprite = EmptyCauldron;
        this.GetComponent<AudioSource>().Stop();
        CancelInvoke();
    }

    public void grabGlassWater()
    {
        Transform thisGlassWater = Instantiate(Player.GetComponent<ResourceManager>().GlassWater, Player.position, Player.GetComponent<ResourceManager>().GlassWater.rotation); //create gathered object on player
        thisGlassWater.name = Player.GetComponent<ResourceManager>().GlassWater.name; //set new objects name to be the same as the original
        thisGlassWater.GetComponent<ItemBehavior>().ItemCount = Player.GetComponent<ResourceManager>().WaterGlassGain; //Set new objects count to be the corresponding GatherGain
        thisGlassWater.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
        if (!Player.GetComponent<PlayerBehavior>().GiveObject(thisGlassWater))
        {
            Destroy(thisGlassWater.gameObject);
        }
        else
        {
            Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI

            isBoiledWater = false;
            isEmpty = true;
            CauldronPopup.GetChild(4).GetComponent<Button>().interactable = false;
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.GetComponent<Animator>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = EmptyCauldron;
            this.GetComponent<AudioSource>().clip = CauldronSounds[1];
            this.GetComponent<AudioSource>().loop = false;
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void grabPastaBowl()
    {
        Transform thisPastaBowl = Instantiate(Player.GetComponent<ResourceManager>().PastaBowl, Player.position, Player.GetComponent<ResourceManager>().PastaBowl.rotation); //create gathered object on player
        thisPastaBowl.name = Player.GetComponent<ResourceManager>().PastaBowl.name; //set new objects name to be the same as the original
        thisPastaBowl.GetComponent<ItemBehavior>().ItemCount = Player.GetComponent<ResourceManager>().PastaGain; //Set new objects count to be the corresponding GatherGain
        thisPastaBowl.transform.localScale = new Vector2(3, 3); //adjust the size of the new object
        if (!Player.GetComponent<PlayerBehavior>().GiveObject(thisPastaBowl))
        {
            Destroy(thisPastaBowl.gameObject);
        }
        else
        {
            Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand UI

            isCookedPasta = false;
            isEmpty = true;
            CauldronPopup.GetChild(3).GetComponent<Button>().interactable = false;
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.GetComponent<Animator>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = EmptyCauldron;
            this.GetComponent<AudioSource>().Stop();
        }
    }

    public void MakePasta()
    {
        if (isBoiledWater)
        {
            isBoiledWater = false;
            isCookingPasta = true;
            Highlight.gameObject.SetActive(false);
            this.GetComponent<Animator>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = CookingPastaCauldron;
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

            this.GetComponent<AudioSource>().Stop();

            myTimer = Instantiate(Timer, this.transform.position, Timer.rotation); //create timer
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(Player.GetComponent<ResourceManager>().CookingTimeDelay);
            Invoke("CookedPasta", Player.GetComponent<ResourceManager>().CookingTimeDelay);
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
            this.GetComponent<AudioSource>().clip = CauldronSounds[1];
            this.GetComponent<AudioSource>().loop = false;
            this.GetComponent<AudioSource>().Play();
            this.transform.GetChild(1).gameObject.SetActive(false);
            Player.GetComponent<GameManager>().cooked++;
            Player.GetComponent<GameManager>().cauldronFilled++;

            myTimer = Instantiate(Timer, this.transform.position, Timer.rotation); //create timer
            Player.GetComponent<GameManager>().Timers.Add(myTimer);
            myTimer.GetComponent<TimerBehavior>().startCounting(Player.GetComponent<ResourceManager>().CookingTimeDelay);
            Invoke("boiledWater", Player.GetComponent<ResourceManager>().CookingTimeDelay);
        }
        else
        {
            Debug.LogError("Tried to fill cauldron with water when it wasn't empty.");
        }
    }


    public void boiledWater()
    {
        isUnboiledWater = false;
        isBoiledWater = true;
        if(myTimer != null)
        {
            Destroy(myTimer.gameObject);
        }
        this.GetComponent<SpriteRenderer>().sprite = BoilingWaterCauldron;
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<AudioSource>().clip = CauldronSounds[0];
        this.GetComponent<AudioSource>().loop = true;
        this.GetComponent<AudioSource>().Play();
        Player.GetComponent<GameManager>().cauldronBoiled++;
        if (isCollidingWithPlayer)
        {
            checkForHighlights();
        }
    }

    private void CookedPasta()
    {
        isCookingPasta = false;
        isCookedPasta = true;
        this.GetComponent<SpriteRenderer>().sprite = PastaCauldron;
        this.GetComponent<AudioSource>().clip = CauldronSounds[0];
        this.GetComponent<AudioSource>().loop = true;
        this.GetComponent<AudioSource>().Play();
        if (isCollidingWithPlayer)
        {
            checkForHighlights();
        }
    }

    private void checkForHighlights()
    {
        Transform LeftHand = Player.GetComponent<PlayerBehavior>().LeftHandObject;
        Transform RightHand = Player.GetComponent<PlayerBehavior>().RightHandObject;
        if ((isEmpty && ((LeftHand != null && LeftHand.name.Equals("Water")) || (RightHand != null && RightHand.name.Equals("Water")))) ||
            isBoiledWater || (isCookedPasta && (LeftHand == null || RightHand == null || LeftHand.name.Equals("Pasta") || RightHand.name.Equals("Pasta")) &&
                    GameObject.Find("Player").GetComponent<PlayerBehavior>().MovementSpeed > ((.3f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 3)))
        {
            Highlight.gameObject.SetActive(true);
        }

        if (isBoiledWater)
        {

            if ((LeftHand == null || RightHand == null || LeftHand.name.Equals("WaterGlass") || RightHand.name.Equals("WaterGlass")) &&
                GameObject.Find("Player").GetComponent<PlayerBehavior>().MovementSpeed > ((.1f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 5))
            {
                CauldronPopup.GetChild(4).GetComponent<Button>().interactable = true;
            }
            else
            {
                CauldronPopup.GetChild(4).GetComponent<Button>().interactable = false;
            }

            int requiredNoodleCount = 3;
            if (((LeftHand != null && LeftHand.name.Equals("Noodles") && LeftHand.GetComponent<ItemBehavior>().ItemCount >= requiredNoodleCount) ||
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
        else
        {
            CauldronPopup.GetChild(4).GetComponent<Button>().interactable = false;
            CauldronPopup.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkForHighlights();
        CauldronPopup.gameObject.SetActive(true);
        isCollidingWithPlayer = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        checkForHighlights();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Highlight.gameObject.SetActive(false);
        CauldronPopup.gameObject.SetActive(false);
        isCollidingWithPlayer = false;

        GameObject Tooltip = GameObject.Find("Tool Tip");
        if (Tooltip != null)
        {
            Tooltip.SetActive(false);
        }
    }
}
