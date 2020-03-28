using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [HideInInspector] public bool controlMovement = true;

    public float MovementSpeed = 15f; //movement speed of the player character
    private Vector2 Destination;

    public Transform HandObject;

    private GameObject StorageObject;
    private GameObject HandUIImage;
    
    // Start is called before the first frame update
    void Start()
    {
        Destination = transform.position; //find destination position
        HandUIImage = GameObject.Find("HandImage");
        if (HandUIImage == null)
        {
            Debug.LogError(name + " could not find Hand Image on Startup.");
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
            if (HandObject != null)
            {
                HandObject.transform.position = move;
            }
        }

        if (Input.GetMouseButtonDown(1) && StorageObject != null)
        {
            if (HandObject == null)
            {
                HandObject = StorageObject.GetComponent<StorageBehaviour>().GatherObject();
                if (HandObject != null)
                {
                    HandObject.transform.position = this.transform.position;
                    HandObject.transform.localScale = new Vector2(3, 3);
                    checkHand();
                }
            }
            else
            {
                HandObject.transform.localScale = new Vector2(5, 5);
                StorageObject.GetComponent<StorageBehaviour>().PlaceObject(HandObject);
                HandObject = null;
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

    public void checkHand()
    {
        if (HandObject != null)
        {
            int handItemCount = HandObject.GetComponent<ItemBehavior>().ItemCount;
            if (handItemCount > 0)
            {
                HandUIImage.transform.GetChild(3).GetComponent<Text>().text = handItemCount + "";
                HandUIImage.GetComponent<Image>().sprite = HandObject.GetComponent<SpriteRenderer>().sprite;
                HandUIImage.SetActive(true);
            }
            else
            {
                HandUIImage.SetActive(false);
            }
        }
        else
        {
            HandUIImage.SetActive(false);
        }
    }
}
