﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [HideInInspector] public bool controlMovement = true;

    public float MovementSpeed = 15f; //movement speed of the player character
    private Vector2 Destination;

    public Transform LeftHandObject;
    public Transform RightHandObject;

    public Sprite FrontErin;
    public Sprite SideErin;
    public Sprite BackErin;

    public float PreviousXp = 0;
    public float xp = 0;
    public int Level
    {
        get { return xpToLevels(xp); }
    }
    [HideInInspector] public int[] LevelMilestones;

    public float strength = 0;

    public List<string> PlayerSkills;

    public Vector3 HandOffset = new Vector3(3, -1, 0);

    private GameObject Cauldron;
    private GameObject StorageObject;
    private GameObject LeftHandUIImage;
    private GameObject RightHandUIImage;

    public Transform LevelChoices;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelMilestones = new int[10];
        LevelMilestones[0] = 150;
        LevelMilestones[1] = 500;
        LevelMilestones[2] = 1000;
        LevelMilestones[3] = 2200;
        LevelMilestones[4] = 4800;
        LevelMilestones[5] = 10000;
        LevelMilestones[6] = 15000;
        LevelMilestones[7] = 21000;
        LevelMilestones[8] = 28500;
        LevelMilestones[9] = 37000;


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
            Destination = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                Destination += new Vector2(0, MovementSpeed);
                this.GetComponent<SpriteRenderer>().sprite = BackErin;
                this.GetComponent<SpriteRenderer>().flipX = false;
                HandOffset.x = 3;
                moveHandObject(99, 99);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Destination += new Vector2(-MovementSpeed, 0);
                this.GetComponent<SpriteRenderer>().sprite = SideErin;
                this.GetComponent<SpriteRenderer>().flipX = true;
                HandOffset.x = 2;
                moveHandObject(99, 105);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Destination += new Vector2(0, -MovementSpeed);
                this.GetComponent<SpriteRenderer>().sprite = FrontErin;
                this.GetComponent<SpriteRenderer>().flipX = false;
                HandOffset.x = 3;
                moveHandObject(105, 105);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Destination += new Vector2(MovementSpeed, 0);
                this.GetComponent<SpriteRenderer>().sprite = SideErin;
                this.GetComponent<SpriteRenderer>().flipX = false;
                HandOffset.x = 2;
                moveHandObject(105, 99);
            }

            //Vector2 move = Vector2.MoveTowards(transform.position, Destination, 100000 * Time.deltaTime);
            this.GetComponent<Rigidbody2D>().MovePosition(Destination);
            //transform.position = move; //move player towards destination
            //moveHandObject(105, 105);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.T))
        {
            LevelChoices.gameObject.SetActive(true);
        }



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
                    else if (LeftHandObject != null && LeftHandObject.name.Equals(tableObject.name))
                    {
                        tableObject.GetComponent<ItemBehavior>().ItemCount += LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                        MovementSpeed += Mathf.Max(LeftHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                        Destroy(LeftHandObject.gameObject);
                        LeftHandObject = null;
                        checkHand();
                    }
                    else if (RightHandObject != null && RightHandObject.name.Equals(tableObject.name))
                    {
                        tableObject.GetComponent<ItemBehavior>().ItemCount += RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                        MovementSpeed += Mathf.Max(RightHandObject.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                        Destroy(RightHandObject.gameObject);
                        RightHandObject = null;
                        checkHand();
                    }
                    else
                    {
                        if (LeftHandObject == null)
                        {
                            LeftHandObject = tableObject;
                            StorageObject.GetComponent<StorageBehaviour>().RemoveObject();
                            LeftHandObject.transform.position = this.transform.position - HandOffset;
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
            else if(Cauldron != null)
            {
                if(Cauldron.GetComponent<CauldronBehavior>().isEmpty)
                {
                    if(LeftHandObject != null && LeftHandObject.name.Equals("Water")) 
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
                else if(Cauldron.GetComponent<CauldronBehavior>().isCookedPasta && (LeftHandObject == null || RightHandObject == null))
                {
                    Cauldron.GetComponent<CauldronBehavior>().grabPastaBowl();
                }
            }
            else if (LeftHandObject != null && RightHandObject == null)
            {
                RightHandObject = Instantiate(LeftHandObject, this.transform.position + HandOffset, LeftHandObject.rotation);
                RightHandObject.name = LeftHandObject.name;
                RightHandObject.GetComponent<ItemBehavior>().ItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount / 2;
                LeftHandObject.GetComponent<ItemBehavior>().ItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount / 2 + LeftHandObject.GetComponent<ItemBehavior>().ItemCount % 2;
                RightHandObject.transform.localScale = new Vector2(3, 3);
                checkHand();
            } 
            else if (LeftHandObject == null && RightHandObject != null)
            {
                LeftHandObject = Instantiate(RightHandObject, this.transform.position - HandOffset, RightHandObject.rotation);
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
    }

    public bool GiveObject(Transform ItemForPlayer)
    {
        if (LeftHandObject == null)
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
        else if (LeftHandObject.name.Equals(ItemForPlayer.name))
        {
            LeftHandObject.GetComponent<ItemBehavior>().ItemCount += ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            MovementSpeed += -Mathf.Max(ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
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
                LeftHandObject.GetComponent<SpriteRenderer>().sortingOrder = 99;
            }
            else
            {
                LeftHandObject.GetComponent<SpriteRenderer>().sortingOrder = 105;
            }
            return true;
        }
        else if (RightHandObject.name.Equals(ItemForPlayer.name))
        {
            RightHandObject.GetComponent<ItemBehavior>().ItemCount += ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            MovementSpeed += -Mathf.Max(ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight - strength, 0) * ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            Destroy(ItemForPlayer.gameObject);
            return true;
        }
        else
        {
            //Debug.LogError(name + " had a full incompatible hand when trying to give object to player.");
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
            //LeftHandObject.transform.position = Vector3.Lerp(LeftHandObject.transform.position, Destination + new Vector2(-HandOffset.x, HandOffset.y), .9f);
            LeftHandObject.GetComponent<Rigidbody2D>().MovePosition(Destination + new Vector2(-HandOffset.x, HandOffset.y));
            LeftHandObject.GetComponent<SpriteRenderer>().sortingOrder = leftLayer;
        }
        if (RightHandObject != null)
        {
            RightHandObject.GetComponent<Rigidbody2D>().MovePosition(Destination + new Vector2(HandOffset.x, HandOffset.y));
            RightHandObject.GetComponent<SpriteRenderer>().sortingOrder = RightLayer;
        }
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
        else
        {
            return 10;
        }
    }
}
