using System.Collections;
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
    //public Transform HandObject;

    private GameObject StorageObject;
    private GameObject LeftHandUIImage;
    private GameObject RightHandUIImage;

    private Vector3 HandOffset = new Vector3(3, 0, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        Destination = transform.position; //find destination position
        LeftHandUIImage = GameObject.Find("LeftHandImage");
        if (LeftHandUIImage == null)
        {
            Debug.LogError(name + " could not find Left Hand Image on Startup.");
        }
        RightHandUIImage = GameObject.Find("RightHandImage");
        if (RightHandUIImage == null)
        {
            Debug.LogError(name + " could not find Right Hand Image on Startup.");
        }
        checkHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && controlMovement) //check for left mouse click
        {
            int layermask1 = 1 << LayerMask.NameToLayer("UI");
            int layermask2 = 1 << LayerMask.NameToLayer("Interactable");
            int finalmask = layermask1 | layermask2;
            Collider2D worldClick = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), finalmask); //get all colliders in the world where the mouse is positioned
            Collider2D ScreenClick = Physics2D.OverlapPoint(Input.mousePosition, finalmask); //get all colliders in the screen where the mouse is positioned
            if (worldClick == null && ScreenClick == null) //if the colliding objects are not Interactables in the world or UI elements on the screen
            {
                Destination = Camera.main.ScreenToWorldPoint(Input.mousePosition); //change destination to mouse cursor location
            }
        }
        if (Mathf.Abs((transform.position - (Vector3)Destination).magnitude) > .1f) //if player is farther than .1 from destination (Optimize)
        {
            Vector2 move = Vector2.MoveTowards(transform.position, Destination, MovementSpeed * Time.deltaTime);
            transform.position = move; //move player towards destination
            if (LeftHandObject != null)
            {
                LeftHandObject.transform.position = move - (Vector2)HandOffset;
            }
            if (RightHandObject != null)
            {
                RightHandObject.transform.position = move + (Vector2)HandOffset;
            }
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
                            MovementSpeed += LeftHandObject.GetComponent<ItemBehavior>().ItemWeight * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                            LeftHandObject = null;
                            checkHand();
                        }
                        else
                        {
                            RightHandObject.transform.localScale = new Vector2(5, 5);
                            StorageObject.GetComponent<StorageBehaviour>().PlaceObject(RightHandObject);
                            MovementSpeed += RightHandObject.GetComponent<ItemBehavior>().ItemWeight * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                            RightHandObject = null;
                            checkHand();
                        }
                    }
                    else if (LeftHandObject != null && LeftHandObject.name.Equals(tableObject.name))
                    {
                        tableObject.GetComponent<ItemBehavior>().ItemCount += LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                        MovementSpeed += LeftHandObject.GetComponent<ItemBehavior>().ItemWeight * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                        Destroy(LeftHandObject.gameObject);
                        LeftHandObject = null;
                        checkHand();
                    }
                    else if (RightHandObject != null && RightHandObject.name.Equals(tableObject.name))
                    {
                        tableObject.GetComponent<ItemBehavior>().ItemCount += RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                        MovementSpeed += RightHandObject.GetComponent<ItemBehavior>().ItemWeight * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
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
                            MovementSpeed += -LeftHandObject.GetComponent<ItemBehavior>().ItemWeight * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
                            checkHand();
                        }
                        else if (RightHandObject == null)
                        {
                            RightHandObject = tableObject;
                            StorageObject.GetComponent<StorageBehaviour>().RemoveObject();
                            RightHandObject.transform.position = this.transform.position + HandOffset;
                            RightHandObject.transform.localScale = new Vector2(3, 3);
                            MovementSpeed += -RightHandObject.GetComponent<ItemBehavior>().ItemWeight * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
                            checkHand();
                        }
                    }
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Storage"))
        {
            StorageObject = null;
        }
    }

    public void GiveObject(Transform ItemForPlayer)
    {
        if (LeftHandObject == null)
        {
            LeftHandObject = ItemForPlayer;
            LeftHandObject.transform.position = this.transform.position - HandOffset;
            MovementSpeed += -LeftHandObject.GetComponent<ItemBehavior>().ItemWeight * LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
        }
        else if (LeftHandObject.name.Equals(ItemForPlayer.name))
        {
            LeftHandObject.GetComponent<ItemBehavior>().ItemCount += ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            MovementSpeed += -ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight * ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
        }
        else if (RightHandObject == null)
        {
            RightHandObject = ItemForPlayer;
            RightHandObject.transform.position = this.transform.position + HandOffset;
            MovementSpeed += -RightHandObject.GetComponent<ItemBehavior>().ItemWeight * RightHandObject.GetComponent<ItemBehavior>().ItemCount;
        }
        else if (RightHandObject.name.Equals(ItemForPlayer.name))
        {
            RightHandObject.GetComponent<ItemBehavior>().ItemCount += ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
            MovementSpeed += -ItemForPlayer.GetComponent<ItemBehavior>().ItemWeight * ItemForPlayer.GetComponent<ItemBehavior>().ItemCount;
        }
        else
        {
            Debug.LogError(name + " had a full incompatible hand when trying to give object to player.");
        }
    }

    public void checkHand()
    {
        if (LeftHandObject != null)
        {
            int handItemCount = LeftHandObject.GetComponent<ItemBehavior>().ItemCount;
            if (handItemCount > 0)
            {
                LeftHandUIImage.transform.GetChild(3).GetComponent<Text>().text = handItemCount + "";
                LeftHandUIImage.GetComponent<Image>().sprite = LeftHandObject.GetComponent<SpriteRenderer>().sprite;
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
                RightHandUIImage.transform.GetChild(3).GetComponent<Text>().text = handItemCount + "";
                RightHandUIImage.GetComponent<Image>().sprite = RightHandObject.GetComponent<SpriteRenderer>().sprite;
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
}
