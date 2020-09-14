using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomerRequestBehavior : MonoBehaviour
{
    public GameObject Popup; // parent Popup object

    public Transform BronzeCoin;

    public float xpGain = 50;

    public int maxTip = 3;

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
                (RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)) || 
                (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Any Meal Will Do") && !Player.GetComponent<GameManager>().canAnyMeal && 
                (LeftPlayerObject != null || RightPlayerObject != null)) || (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn, My Hand") && 
                Player.GetComponent<GameManager>().StorageTableContains(this.GetComponent<Image>().sprite)))
            {
                if (Customer == null)
                {
                    Debug.LogError(name + " could not find Customer Transform after decrementing the counter.");
                }
                else
                {
                    int otherItemIndex = 3;
                    int originalOtherRequestCount = 1;
                    int otherRequestCount = 1;
                    if (this.transform.parent.transform.childCount == 5)
                    {
                        originalOtherRequestCount = int.Parse(this.transform.parent.transform.GetChild(otherItemIndex).GetChild(0).GetComponent<Text>().text);
                        otherRequestCount = originalOtherRequestCount;
                        if (this.transform.parent.transform.GetChild(3).Equals(this.transform))
                        {
                            otherItemIndex = 4;
                        }
                    }

                    int RequestCount = int.Parse(this.transform.GetChild(0).GetComponent<Text>().text);
                    if ((LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)) || 
                        (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Any Meal Will Do") && !Player.GetComponent<GameManager>().canAnyMeal && LeftPlayerObject != null))
                    {
                        RequestCount = satisfyRequest(LeftPlayerObject, this.transform, RequestCount);
                    }
                    else if (LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.transform.parent.transform.GetChild(otherItemIndex).GetComponent<Image>().sprite))
                    {
                        otherRequestCount = satisfyRequest(LeftPlayerObject, this.transform.parent.transform.GetChild(otherItemIndex), otherRequestCount);
                    }

                    if ((RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)) ||
                        (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Any Meal Will Do") && !Player.GetComponent<GameManager>().canAnyMeal && RightPlayerObject != null && RequestCount > 0))
                    {
                        RequestCount = satisfyRequest(RightPlayerObject, this.transform, RequestCount);
                    }
                    else if ((RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.transform.parent.transform.GetChild(otherItemIndex).GetComponent<Image>().sprite)) ||
                        (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Any Meal Will Do") && !Player.GetComponent<GameManager>().canAnyMeal && RightPlayerObject != null && otherRequestCount > 0))
                    {
                        otherRequestCount = satisfyRequest(RightPlayerObject, this.transform.parent.transform.GetChild(otherItemIndex), otherRequestCount);
                    }

                    if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn, My Hand") && Player.GetComponent<GameManager>().StorageTableContains(this.GetComponent<Image>().sprite))
                    {
                        RequestCount = satisfyRequest(Player.GetComponent<GameManager>().StorageTableRetrieve(this.GetComponent<Image>().sprite), this.transform, RequestCount);
                    }
                    else if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn, My Hand") && Player.GetComponent<GameManager>().StorageTableContains(this.transform.parent.transform.GetChild(otherItemIndex).GetComponent<Image>().sprite))
                    {
                        otherRequestCount = satisfyRequest(Player.GetComponent<GameManager>().StorageTableRetrieve(this.transform.parent.transform.GetChild(otherItemIndex).GetComponent<Image>().sprite), this.transform.parent.transform.GetChild(otherItemIndex), otherRequestCount);
                    }

                    if (otherRequestCount <= 0)
                    {

                        ReduceSize(); //change UI to fit less items
                        Destroy(this.transform.parent.transform.GetChild(otherItemIndex).gameObject); //Destroy this gameobject
                    }

                    if (RequestCount <= 0)
                    {

                        if (this.transform.parent.childCount <= 5 /*|| otherRequestCount <= 0*/) //if this is the last request
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

                            Transform indicator = Instantiate(Customer.GetComponent<CustomerBehavior>().MoodIndicator, Customer.transform.position + new Vector3(0, 15, 0), 
                                Customer.GetComponent<CustomerBehavior>().MoodIndicator.transform.rotation);
                            indicator.parent = Customer;
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
    }

    private int satisfyRequest(Transform PlayerObject, Transform Request, int RequestCount)
    {
        int HandCount = PlayerObject.gameObject.GetComponent<ItemBehavior>().ItemCount;
        int CoinCount = HandCount;
        if (RequestCount - HandCount > 0)
        {
            Request.transform.GetChild(0).GetComponent<Text>().text = (int.Parse(Request.transform.GetChild(0).GetComponent<Text>().text) - HandCount) + "";
            PlayerObject.GetComponent<ItemBehavior>().ItemCount = 0;
            Player.GetComponent<PlayerBehavior>().MovementSpeed += Math.Max((PlayerObject.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength) * HandCount, 0);
            Player.GetComponent<PlayerBehavior>().xp += xpGain * HandCount;
            Player.GetComponent<PlayerBehavior>().money += HandCount * PlayerObject.gameObject.GetComponent<ItemBehavior>().ItemValue;
        }
        else
        {
            PlayerObject.GetComponent<ItemBehavior>().ItemCount -= RequestCount;
            Player.GetComponent<PlayerBehavior>().MovementSpeed += Math.Max((PlayerObject.GetComponent<ItemBehavior>().ItemWeight - Player.GetComponent<PlayerBehavior>().strength) * RequestCount, 0);
            Player.GetComponent<PlayerBehavior>().xp += xpGain * RequestCount;
            if (Customer.GetComponent<CustomerBehavior>().myTimer != null)
            {
                Player.GetComponent<PlayerBehavior>().money += (RequestCount * PlayerObject.gameObject.GetComponent<ItemBehavior>().ItemValue) +
                    (((maxTip * 2 + 1) - Customer.GetComponent<CustomerBehavior>().myTimer.GetComponent<TimerBehavior>().count) / 2);
            }
            else
            {
                Player.GetComponent<PlayerBehavior>().money += (RequestCount * PlayerObject.gameObject.GetComponent<ItemBehavior>().ItemValue) + maxTip;
            }
            if(PlayerObject.GetComponent<ItemBehavior>().ItemValue > 1)
            {
                Player.GetComponent<GameManager>().ExpensiveFood++;
            }
            Player.GetComponent<GameManager>().mealsServed++;
            CoinCount = RequestCount;
        }
        for(int i = 0; i < CoinCount; i++)
        {
            Instantiate(BronzeCoin, PlayerObject.position + new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(0, 1), 0), BronzeCoin.rotation);
        }
        Player.GetComponent<PlayerBehavior>().checkHand(); //tell player script to check hand for UI
        RequestCount -= HandCount;
        return RequestCount;
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
