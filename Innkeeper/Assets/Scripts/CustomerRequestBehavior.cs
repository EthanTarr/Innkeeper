using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomerRequestBehavior : MonoBehaviour
{
    public GameObject Popup; // parent Popup object

    public float xpGain = 50;

    private Transform Customer; // customer Transform
    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        if (Popup == null)
        {
            Debug.LogError(name + " could not find Popup parent object on startup");
        } else
        {
            Customer = Popup.GetComponent<PopupBehaviour>().PopupObject; //set Customer Transform to be parent Popup's Customer Tranform
        }
        Player = GameObject.Find("Player").transform;
        if(Player == null)
        {
            Debug.LogError(name + " could not find player on statup");
        }
    }

    private void Update()
    {
        if (Popup.activeSelf)
        {
            Transform LeftPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().LeftHandObject;
            Transform RightPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().RightHandObject;
            if ((LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)) ||
                (RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)))
            {
                this.GetComponent<Button>().interactable = true;
            }
            else
            {
                this.GetComponent<Button>().interactable = false;
            }
        }
    }


    // Checks to see if Blue Fruit Counter can be legally decremented. If it can, then the customer and this parent object are deleted
    public void FullfilRequest()
    {
        Transform LeftPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().LeftHandObject;
        Transform RightPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().RightHandObject;
        if (LeftPlayerObject != null || RightPlayerObject != null)
        {
            if((LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)) || 
                (RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)))
            {
                if (Customer == null)
                {
                    Debug.LogError(name + " could not find Customer Transform after decrementing the counter.");
                }
                else
                {
                    int RequestCount = int.Parse(this.transform.GetChild(0).GetComponent<Text>().text);
                    if (LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite))
                    {
                        int HandCount = LeftPlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount;
                        if(RequestCount - HandCount > 0)
                        {
                            this.transform.GetChild(0).GetComponent<Text>().text = (int.Parse(this.transform.GetChild(0).GetComponent<Text>().text) - HandCount) + "";
                            LeftPlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount = 0;
                            Player.GetComponent<PlayerBehavior>().MovementSpeed += Math.Max((LeftPlayerObject.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength) * HandCount, 0);
                            Player.GetComponent<PlayerBehavior>().xp += xpGain * HandCount;
                            Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand for UI
                            return;
                        }
                        LeftPlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount += -RequestCount;
                        Player.GetComponent<PlayerBehavior>().MovementSpeed += Math.Max((LeftPlayerObject.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength) * RequestCount, 0);
                    }
                    else
                    {
                        int HandCount = RightPlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount;
                        if (RequestCount - HandCount > 0)
                        {
                            this.transform.GetChild(0).GetComponent<Text>().text = (int.Parse(this.transform.GetChild(0).GetComponent<Text>().text) - HandCount) + "";
                            RightPlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount = 0;
                            Player.GetComponent<PlayerBehavior>().MovementSpeed += Math.Max((RightPlayerObject.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength) * HandCount, 0);
                            Player.GetComponent<PlayerBehavior>().xp += xpGain * HandCount;
                            Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand for UI
                            return;
                        }
                        RightPlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount += -RequestCount;
                        Player.GetComponent<PlayerBehavior>().MovementSpeed += Math.Max((RightPlayerObject.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength) * RequestCount, 0);
                    }
                    Player.GetComponent<PlayerBehavior>().xp += xpGain * RequestCount;
                    Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand for UI



                    if (this.transform.parent.transform.childCount <= 4) //if this is the last request
                    {
                        Customer.GetComponent<CustomerBehavior>().SendCustomerAway();
                        Player.GetComponent<GameManager>().numOfSatisfiedCustomers++;
                        if (Customer.GetComponent<CustomerBehavior>().customer.Equals("drake"))
                        {
                            Customer.GetComponent<AudioSource>().clip = Customer.GetComponent<CustomerBehavior>().DrakeSounds[UnityEngine.Random.Range(1, 3)];
                        }
                        else if (Customer.GetComponent<CustomerBehavior>().customer.Equals("antinium"))
                        {
                            Customer.GetComponent<AudioSource>().clip = Customer.GetComponent<CustomerBehavior>().AntiniumSounds[UnityEngine.Random.Range(1, 3)];
                        }
                        else
                        {
                            Customer.GetComponent<AudioSource>().clip = Customer.GetComponent<CustomerBehavior>().GoblinSounds[UnityEngine.Random.Range(1, 3)];
                        }
                        Customer.GetComponent<AudioSource>().Play();
                    }
                    else
                    {
                        ReduceSize(); //change UI to fit less items
                    }
                    Destroy(this.gameObject); //Destroy this gameobject
                }
            }
        }
    }


    public void ReduceSize()
    {
        Popup.transform.GetChild(2).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Popup.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta.x - 
            this.GetComponent<RectTransform>().sizeDelta.x); //reduce UI view
        Popup.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2((Popup.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta.x +
                    Popup.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x) * -.5f, 0);
        Popup.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2((Popup.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta.x +
            Popup.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.x) * .5f, 0);
        foreach (Transform child in this.transform.parent) //search for siblings
        {
            if (!(child.gameObject.name.Equals("Left Background") || child.gameObject.name.Equals("Right Background") || child.gameObject.name.Equals("Center Background") || child.Equals(this.transform))) //find sibling
            {
                if(child.GetComponent<RectTransform>().localPosition.x < this.GetComponent<RectTransform>().localPosition.x)
                {
                    child.GetComponent<RectTransform>().localPosition += new Vector3(child.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0); //move sibling to fit
                }
                else
                {
                    child.GetComponent<RectTransform>().localPosition += new Vector3(child.GetComponent<RectTransform>().sizeDelta.x * -.5f, 0, 0); //move sibling to fit
                }
            }
        }
    }
}
