using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerBehavior : MonoBehaviour
{
    [HideInInspector] public bool controlMovement = true;

    public float MovementSpeed = 15f; //movement speed of the player character
    private Vector2 Destination;
    private Vector2 PreviousDestination;

    public Transform LeftHandObject;
    public Transform RightHandObject;

    public Sprite FrontErin;
    public Sprite SideErin;
    public Sprite BackErin;

    public float PreviousXp = 0;
    public float xp = 0;
    public int Level = 0;
    
    [HideInInspector] public int[] LevelMilestones;

    public float strength = 0;

    public int money = 0;

    public List<string> PlayerSkills = new List<string>();
    public List<string> Purchases = new List<string>();

    public Vector3 HandOffset = new Vector3(3, -1, 0);

    private GameObject Cauldron;
    private GameObject StorageObject;
    private GameObject Trash;
    private GameObject LeftHandUIImage;
    private GameObject RightHandUIImage;

    private int leftHandNum = 99;
    private int rightHandNum = 99;

    public bool canDash = false;

    
    
    // Start is called before the first frame update
    void Start()
    {
        LevelMilestones = new int[21];
        LevelMilestones[0] = -1;
        LevelMilestones[1] = 200;
        LevelMilestones[2] = 475;
        LevelMilestones[3] = 725;
        LevelMilestones[4] = 950;
        LevelMilestones[5] = 1250;
        LevelMilestones[6] = 1750;
        LevelMilestones[7] = 2300;
        LevelMilestones[8] = 2900;
        LevelMilestones[9] = 3550;
        LevelMilestones[10] = 4250;
        LevelMilestones[11] = 5000;
        LevelMilestones[12] = 5800;
        LevelMilestones[13] = 6650;
        LevelMilestones[14] = 7550;
        LevelMilestones[15] = 8500;
        LevelMilestones[16] = 9500;
        LevelMilestones[17] = 10550;
        LevelMilestones[18] = 11650;
        LevelMilestones[19] = 12800;
        LevelMilestones[20] = int.MaxValue;


        Destination = transform.position; //find destination position
        LeftHandUIImage = GameObject.Find("LeftHandUI");
        if (LeftHandUIImage == null)
        {
            Debug.LogError(name + " could not find Left Hand Image on Startup.");
        }
        RightHandUIImage = GameObject.Find("RightHandUI");
        if (RightHandUIImage == null)
        {
            Debug.LogError(name + " could not find Right Hand Image on Startup.");
        }
        checkHand();
    }

    private void FixedUpdate()
    {
        if (controlMovement)
        {
            //To prevent player from not being able to move
            float oldMovement = MovementSpeed;
            if (MovementSpeed < 0.1f)
            {
                MovementSpeed = 0.1f;
            }

            //calculating where to move
            PreviousDestination = transform.position;
            Destination = transform.position;

            if (canDash && Input.GetKeyDown(KeyCode.E))
            {
                MovementSpeed += 20;
                canDash = false;
                Debug.Log("Dash amount " + MovementSpeed);
                this.GetComponent<GameManager>().DashIndicator.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
                StartCoroutine("DashRecharge");
                if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                {
                    Destination += new Vector2(0, MovementSpeed);
                }
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                Destination += new Vector2(0, MovementSpeed);
                this.GetComponent<SpriteRenderer>().sprite = BackErin;
                this.GetComponent<SpriteRenderer>().flipX = false;
                this.GetComponent<Animator>().SetBool("Forward", false);
                this.GetComponent<Animator>().SetBool("Backward", true);
                this.GetComponent<Animator>().SetBool("Sideways", false);
                HandOffset.x = 3;
                leftHandNum = 99;
                rightHandNum = 99;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Destination += new Vector2(0, -MovementSpeed);
                this.GetComponent<SpriteRenderer>().sprite = FrontErin;
                this.GetComponent<SpriteRenderer>().flipX = false;
                this.GetComponent<Animator>().SetBool("Forward", true);
                this.GetComponent<Animator>().SetBool("Backward", false);
                this.GetComponent<Animator>().SetBool("Sideways", false);
                HandOffset.x = 3;
                leftHandNum = 105;
                rightHandNum = 105;
            }
            if (Input.GetKey(KeyCode.A))
            {
                Destination += new Vector2(-MovementSpeed, 0);
                this.GetComponent<SpriteRenderer>().sprite = SideErin;
                this.GetComponent<SpriteRenderer>().flipX = true;
                this.GetComponent<Animator>().SetBool("Forward", false);
                this.GetComponent<Animator>().SetBool("Backward", false);
                this.GetComponent<Animator>().SetBool("Sideways", true);
                HandOffset.x = 2;
                leftHandNum = 99;
                rightHandNum = 105;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Destination += new Vector2(MovementSpeed, 0);
                this.GetComponent<SpriteRenderer>().sprite = SideErin;
                this.GetComponent<SpriteRenderer>().flipX = false;
                this.GetComponent<Animator>().SetBool("Forward", false);
                this.GetComponent<Animator>().SetBool("Backward", false);
                this.GetComponent<Animator>().SetBool("Sideways", true);
                HandOffset.x = 2;
                leftHandNum = 105;
                rightHandNum = 99;
            }
            
            //moveing to location
            this.GetComponent<Rigidbody2D>().MovePosition(Destination);

            //set moving animation
            this.GetComponent<Animator>().SetFloat("Speed", (PreviousDestination - Destination).magnitude);
            this.GetComponent<Animator>().speed = (.85f + (PreviousDestination - Destination).magnitude);

            //return movement to how it was
            MovementSpeed = oldMovement;

            //set moving audio
            if ((PreviousDestination - Destination).magnitude > .1f && !this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().pitch = 1f + (PreviousDestination - Destination).magnitude;
                this.GetComponent<AudioSource>().Play();
                this.GetComponent<GameManager>().steps += (PreviousDestination - Destination).magnitude; //stat
            }
            else if((PreviousDestination - Destination).magnitude < .1f)
            {
                this.GetComponent<AudioSource>().Stop();
            }
            else if(this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<GameManager>().steps += (PreviousDestination - Destination).magnitude; //stat
            }

            //stat
            if(LeftHandObject)
            {
                this.GetComponent<GameManager>().lifts += LeftHandObject.GetComponent<ItemBehavior>().ItemWeight * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
            }
            if (RightHandObject)
            {
                this.GetComponent<GameManager>().lifts += RightHandObject.GetComponent<ItemBehavior>().ItemWeight * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
            }
        }
        moveHandObject(leftHandNum, rightHandNum);
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetMouseButtonDown(1))
        {
            if (StorageObject != null)
            {
                if (!(LeftHandObject == null && RightHandObject == null && StorageObject.GetComponent<StorageBehaviour>().GatherObject() == null))
                {
                    Transform tableObject = StorageObject.GetComponent<StorageBehaviour>().GatherObject();
                    if (tableObject == null)
                    {
                        if (LeftHandObject != null)
                        {
                            LeftHandObject.transform.localScale = new Vector2(5, 5);
                            StorageObject.GetComponent<StorageBehaviour>().PlaceObject(LeftHandObject);
                            MovementSpeed += Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                            LeftHandObject = null;
                            checkHand();
                        }
                        else
                        {
                            RightHandObject.transform.localScale = new Vector2(5, 5);
                            StorageObject.GetComponent<StorageBehaviour>().PlaceObject(RightHandObject);
                            MovementSpeed += Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                            RightHandObject = null;
                            checkHand();
                        }
                    }
                    else if (LeftHandObject != null && LeftHandObject.name.Equals(tableObject.name) && 
                        (tableObject.GetComponent<ItemBehavior>().ItemCount < tableObject.GetComponent<ItemBehavior>().ItemMax))
                    {
                        if (tableObject.GetComponent<ItemBehavior>().ItemMax > tableObject.GetComponent<ItemBehavior>().ItemCount + LeftHandObject.GetComponent<ItemBehavior>().ItemCount)
                        {
                            tableObject.GetComponent<ItemBehavior>().ItemCount += LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                            MovementSpeed += Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                            Destroy(LeftHandObject.gameObject);
                            LeftHandObject = null;
                        } 
                        else
                        {
                            int ItemsSetDown = tableObject.GetComponent<ItemBehavior>().ItemMax - tableObject.GetComponent<ItemBehavior>().ItemCount;
                            LeftHandObject.GetComponent<ItemBehavior>().ItemCount -= ItemsSetDown;
                            tableObject.GetComponent<ItemBehavior>().ItemCount = tableObject.GetComponent<ItemBehavior>().ItemMax;
                            MovementSpeed += Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * ItemsSetDown;
                        }
                        checkHand();
                    }
                    else if (RightHandObject != null && RightHandObject.name.Equals(tableObject.name) &&
                        (tableObject.GetComponent<ItemBehavior>().ItemCount < tableObject.GetComponent<ItemBehavior>().ItemMax))
                    {
                        if (tableObject.GetComponent<ItemBehavior>().ItemMax > tableObject.GetComponent<ItemBehavior>().ItemCount + RightHandObject.GetComponent<ItemBehavior>().ItemCount)
                        {
                            tableObject.GetComponent<ItemBehavior>().ItemCount += RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                            MovementSpeed += Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                            Destroy(RightHandObject.gameObject);
                            RightHandObject = null;
                        }
                        else
                        {
                            int ItemsSetDown = tableObject.GetComponent<ItemBehavior>().ItemMax - tableObject.GetComponent<ItemBehavior>().ItemCount;
                            RightHandObject.GetComponent<ItemBehavior>().ItemCount -= ItemsSetDown;
                            tableObject.GetComponent<ItemBehavior>().ItemCount = tableObject.GetComponent<ItemBehavior>().ItemMax;
                            MovementSpeed += Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * ItemsSetDown;
                        }
                        checkHand();
                    }
                    else
                    {
                        if (LeftHandObject == null)
                        {
                            LeftHandObject = tableObject;
                            StorageObject.GetComponent<StorageBehaviour>().RemoveObject();
                            LeftHandObject.transform.position = this.transform.position + new Vector3(-HandOffset.x, HandOffset.y, 0);
                            LeftHandObject.transform.localScale = new Vector2(3, 3);
                            MovementSpeed += -Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                            checkHand();
                        }
                        else if (RightHandObject == null)
                        {
                            RightHandObject = tableObject;
                            StorageObject.GetComponent<StorageBehaviour>().RemoveObject();
                            RightHandObject.transform.position = this.transform.position + HandOffset;
                            RightHandObject.transform.localScale = new Vector2(3, 3);
                            MovementSpeed += -Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                            checkHand();
                        }
                    }
                }
            }
            else if (Cauldron != null && Cauldron.GetComponent<CauldronBehavior>().Highlight.gameObject.activeSelf)
            {
                if (Cauldron.GetComponent<CauldronBehavior>().isEmpty)
                {
                    if (LeftHandObject != null && LeftHandObject.name.Equals("Water"))
                    {
                        LeftHandObject.GetComponent<ItemBehavior>().ItemCount += -1;
                        MovementSpeed += Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0);
                        checkHand();
                        Cauldron.GetComponent<CauldronBehavior>().getWater();
                    }
                    else if (RightHandObject != null && RightHandObject.name.Equals("Water"))
                    {
                        RightHandObject.GetComponent<ItemBehavior>().ItemCount += -1;
                        MovementSpeed += Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0);
                        checkHand();
                        Cauldron.GetComponent<CauldronBehavior>().getWater();
                    }
                }
                else if (Cauldron.GetComponent<CauldronBehavior>().isCookedPasta && (LeftHandObject == null || RightHandObject == null))
                {
                    Cauldron.GetComponent<CauldronBehavior>().grabPastaBowl();
                }
            }
            else if (Trash != null && Trash.GetComponent<TrashBehavior>().Highlight.gameObject.activeSelf)
            {
                if (LeftHandObject != null)
                {
                    MovementSpeed += Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                    Destroy(LeftHandObject.gameObject);
                    LeftHandObject = null;
                    checkHand();
                }
                else
                {
                    MovementSpeed += Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                    Destroy(RightHandObject.gameObject);
                    RightHandObject = null;
                    checkHand();
                }
            }
            else if (LeftHandObject != null && RightHandObject == null && controlMovement)
            {
                RightHandObject = Instantiate(LeftHandObject, this.transform.position + HandOffset, LeftHandObject.rotation);
                RightHandObject.name = LeftHandObject.name;
                RightHandObject.GetComponent<ItemBehavior>().ItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount / 2;
                LeftHandObject.GetComponent<ItemBehavior>().ItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount / 2 + LeftHandObject.GetComponent<ItemBehavior>().ItemCount % 2;
                RightHandObject.transform.localScale = new Vector2(3, 3);
                checkHand();
            }
            else if (LeftHandObject == null && RightHandObject != null && controlMovement)
            {
                LeftHandObject = Instantiate(RightHandObject, this.transform.position + new Vector3(-HandOffset.x, HandOffset.y, 0), RightHandObject.rotation);
                LeftHandObject.name = RightHandObject.name;
                LeftHandObject.GetComponent<ItemBehavior>().ItemCount = RightHandObject.GetComponent<ItemBehavior>().ItemCount / 2;
                RightHandObject.GetComponent<ItemBehavior>().ItemCount = RightHandObject.GetComponent<ItemBehavior>().ItemCount / 2 + RightHandObject.GetComponent<ItemBehavior>().ItemCount % 2;
                LeftHandObject.transform.localScale = new Vector2(3, 3);
                checkHand();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Storage"))
        {
            StorageObject = collision.gameObject;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Cauldron"))
        {
            Cauldron = collision.gameObject;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            Trash = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Storage"))
        {
            StorageObject = null;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Cauldron"))
        {
            Cauldron = null;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            Trash = null;
        }
    }

    public bool GiveObject(Transform ItemForPlayer)
    {
        /*   Forces items to be the max at most if given to the player
        if (ItemForPlayer.GetComponent<ItemBehavior>().ItemCount > ItemForPlayer.GetComponent<ItemBehavior>().ItemMax)
        {
            ItemForPlayer.GetComponent<ItemBehavior>().ItemCount = ItemForPlayer.GetComponent<ItemBehavior>().ItemMax;
        }
        */
        if (LeftHandObject == null && !(RightHandObject != null && RightHandObject.name.Equals(ItemForPlayer.name) && 
            (RightHandObject.GetComponent<ItemBehavior>().ItemCount + ItemForPlayer.GetComponent<ItemBehavior>().ItemCount <= RightHandObject.GetComponent<ItemBehavior>().ItemMax)))
        {
            LeftHandObject = ItemForPlayer;
            LeftHandObject.transform.position = this.transform.position + new Vector3(-HandOffset.x, HandOffset.y, 0);
            MovementSpeed += -Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
            if(this.gameObject.GetComponent<SpriteRenderer>().sprite.Equals(BackErin) || 
                (this.gameObject.GetComponent<SpriteRenderer>().sprite.Equals(SideErin) && this.gameObject.GetComponent<SpriteRenderer>().flipX))
            {
                LeftHandObject.GetComponent<SpriteRenderer>().sortingOrder = 99;
            }
            else
            {
                LeftHandObject.GetComponent<SpriteRenderer>().sortingOrder = 105;
            }
            return true;
        }
        else if ((LeftHandObject.name.Equals(ItemForPlayer.name) && (LeftHandObject.GetComponent<ItemBehavior>().ItemCount < LeftHandObject.GetComponent<ItemBehavior>().ItemMax)) &&
            !(RightHandObject == null && LeftHandObject.GetComponent<ItemBehavior>().ItemCount + ItemForPlayer.GetComponent<ItemBehavior>().ItemCount > LeftHandObject.GetComponent<ItemBehavior>().ItemMax))
        {
            if(LeftHandObject.GetComponent<ItemBehavior>().ItemCount + ItemForPlayer.GetComponent<ItemBehavior>().ItemCount < ItemForPlayer.GetComponent<ItemBehavior>().ItemMax)
            {
                LeftHandObject.GetComponent<ItemBehavior>().ItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount + ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
                MovementSpeed += -Mathf.Max(ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            } 
            else
            {
                MovementSpeed += -Mathf.Max(ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * (ItemForPlayer.GetComponent<ItemBehavior>().ItemMax - LeftHandObject.GetComponent<ItemBehavior>().ItemCount);
                LeftHandObject.GetComponent<ItemBehavior>().ItemCount = ItemForPlayer.GetComponent<ItemBehavior>().ItemMax;
            }
            Destroy(ItemForPlayer.gameObject);
            return true;
        }
        else if (RightHandObject == null)
        {
            RightHandObject = ItemForPlayer;
            RightHandObject.transform.position = this.transform.position + new Vector3(HandOffset.x, HandOffset.y, 0);
            MovementSpeed += -Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.Equals(BackErin) ||
                (this.gameObject.GetComponent<SpriteRenderer>().sprite.Equals(SideErin) && !this.gameObject.GetComponent<SpriteRenderer>().flipX))
            {
                RightHandObject.GetComponent<SpriteRenderer>().sortingOrder = 99;
            }
            else
            {
                RightHandObject.GetComponent<SpriteRenderer>().sortingOrder = 105;
            }
            return true;
        }
        else if (RightHandObject.name.Equals(ItemForPlayer.name) && (RightHandObject.GetComponent<ItemBehavior>().ItemCount < RightHandObject.GetComponent<ItemBehavior>().ItemMax))
        {
            if (RightHandObject.GetComponent<ItemBehavior>().ItemCount + ItemForPlayer.GetComponent<ItemBehavior>().ItemCount < ItemForPlayer.GetComponent<ItemBehavior>().ItemMax)
            {
                RightHandObject.GetComponent<ItemBehavior>().ItemCount = Mathf.Min(RightHandObject.GetComponent<ItemBehavior>().ItemCount + ItemForPlayer.GetComponent<ItemBehavior>().ItemCount, ItemForPlayer.GetComponent<ItemBehavior>().ItemMax);
                MovementSpeed += -Mathf.Max(ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            }
            else
            {
                MovementSpeed += -Mathf.Max(ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * (ItemForPlayer.GetComponent<ItemBehavior>().ItemMax - RightHandObject.GetComponent<ItemBehavior>().ItemCount);
                RightHandObject.GetComponent<ItemBehavior>().ItemCount = ItemForPlayer.GetComponent<ItemBehavior>().ItemMax;
            }
            Destroy(ItemForPlayer.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void checkHand()
    {
        if (LeftHandObject != null)
        {
            int handItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
            if (handItemCount > 0)
            {
                LeftHandUIImage.transform.GetChild(5).GetComponent<Text>().text = handItemCount + "";
                LeftHandUIImage.transform.GetChild(4).GetComponent<Image>().sprite = LeftHandObject.GetComponent<SpriteRenderer>().sprite;
                LeftHandUIImage.SetActive(true);
            }
            else
            {
                LeftHandUIImage.SetActive(false);
            }
        }
        else
        {
            LeftHandUIImage.SetActive(false);
        }
        if (RightHandObject != null)
        {
            int handItemCount = RightHandObject.GetComponent<ItemBehavior>().ItemCount;
            if (handItemCount > 0)
            {
                RightHandUIImage.transform.GetChild(5).GetComponent<Text>().text = handItemCount + "";
                RightHandUIImage.transform.GetChild(4).GetComponent<Image>().sprite = RightHandObject.GetComponent<SpriteRenderer>().sprite;
                RightHandUIImage.SetActive(true);
            }
            else
            {
                RightHandUIImage.SetActive(false);
            }
        }
        else
        {
            RightHandUIImage.SetActive(false);
        }
    }

    private void moveHandObject(int leftLayer, int RightLayer)
    {
        if (LeftHandObject != null)
        {
            LeftHandObject.GetComponent<Rigidbody2D>().MovePosition(Destination + new Vector2(-HandOffset.x, HandOffset.y));
            LeftHandObject.GetComponent<SpriteRenderer>().sortingOrder = leftLayer;
        }
        if (RightHandObject != null)
        {
            RightHandObject.GetComponent<Rigidbody2D>().MovePosition(Destination + new Vector2(HandOffset.x, HandOffset.y));
            RightHandObject.GetComponent<SpriteRenderer>().sortingOrder = RightLayer;
        }
    }

    public void addPurchase(string purchase)
    {
        Purchases.Add(purchase);
    }

    public void Shoes()
    {
        MovementSpeed += .5f;
    }

    public int xpToLevels(float xp)
    {
        if(xp < LevelMilestones[0])
        {
            return 0;
        }
        else if(xp < LevelMilestones[1])
        {
            return 1;
        }
        else if(xp < LevelMilestones[2])
        {
            return 2;
        }
        else if(xp < LevelMilestones[3])
        {
            return 3;
        }
        else if(xp < LevelMilestones[4])
        {
            return 4;
        }
        else if(xp < LevelMilestones[5])
        {
            return 5;
        }
        else if(xp < LevelMilestones[6])
        {
            return 6;
        }
        else if(xp < LevelMilestones[7])
        {
            return 7;
        }
        else if ( xp < LevelMilestones[8])
        {
            return 8;
        }
        else if (xp < LevelMilestones[9])
        {
            return 9;
        }
        else if (xp < LevelMilestones[10])
        {
            return 10;
        }
        else if (xp < LevelMilestones[11])
        {
            return 11;
        }
        else if (xp < LevelMilestones[12])
        {
            return 12;
        }
        else if (xp < LevelMilestones[13])
        {
            return 13;
        }
        else if (xp < LevelMilestones[14])
        {
            return 14;
        }
        else if (xp < LevelMilestones[15])
        {
            return 15;
        }
        else if (xp < LevelMilestones[16])
        {
            return 16;
        }
        else if (xp < LevelMilestones[17])
        {
            return 17;
        }
        else if (xp < LevelMilestones[18])
        {
            return 18;
        }
        else if (xp <= LevelMilestones[19])
        {
            return 19;
        }
        else
        {
            return 20;
        }
    }

    public void stopDashRecharge()
    {
        StopAllCoroutines();
    }

    IEnumerator DashRecharge()
    {
        yield return new WaitForSeconds(15f - .7f * Level);
        canDash = true;
        Debug.Log("dash true");
        this.GetComponent<GameManager>().DashIndicator.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
