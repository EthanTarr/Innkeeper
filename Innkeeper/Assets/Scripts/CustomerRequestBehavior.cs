using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomerRequestBehavior : MonoBehaviour
{
    public GameObject Popup; // parent Popup object

    private Transform Customer; // customer Transform
    public List<Transform> Items;

    // Start is called before the first frame update
    void Start()
    {
        Transform RequestedItem = (Transform) Items[UnityEngine.Random.Range(0, Items.Count)];
        this.GetComponent<Image>().sprite = RequestedItem.GetComponent<SpriteRenderer>().sprite;
        if (Popup == null)
        {
            Debug.LogError(name + " could not find Popup parent object on startup");
        } else
        {
            Customer = Popup.GetComponent<PopupBehaviour>().PopupObject; //set Customer Transform to be parent Popup's Customer Tranform
        }
    }

    // Checks to see if Blue Fruit Counter can be legally decremented. If it can, then the customer and this parent object are deleted
    public void FullfilRequest()
    {
        Transform PlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().HandObject;
        if (PlayerObject != null)
        {
            if(PlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite))
            {
                if (Customer == null)
                {
                    Debug.LogError(name + " could not find Customer Transform after decrementing the counter.");
                }
                else
                {
                    if (this.transform.parent.transform.childCount <= 1) //if this is the last request
                    {
                        Destroy(Customer.gameObject); //Destroy Customer
                        Destroy(Popup); //Destroy Parent Popup
                    }
                    else if (this.transform.parent.transform.childCount == 2) //if there will be only one more request
                    {
                        ChangeToSingle(); //change UI to single request look
                    }
                    else
                    {
                        bool foundSelf = false; //bool for if self child is found
                        foreach (Transform child in this.transform.parent) //search through this parents children
                        {
                            if (!foundSelf) //if not found self
                            {
                                if (child == this.transform)
                                {
                                    foundSelf = true; //found self
                                }
                                child.GetComponent<RectTransform>().position += new Vector3(25, 0, 0); //move request to the right
                            }
                            else
                            {
                                child.GetComponent<RectTransform>().position += new Vector3(-25, 0, 0); //move reqeust to the left
                            }
                        }
                        this.transform.parent.GetComponent<RectTransform>().sizeDelta += new Vector2(-50, 0); //decrease content size
                    }
                    Destroy(PlayerObject.gameObject);
                    Destroy(this.gameObject); //Destroy this gameobject
                }
            }
        }
    }

    public void ChangeToMultiple()
    {
        Popup.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 132); //expand UI scrollview to have two or more requests
        Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(132, Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y); //expand box collider to new size
        this.GetComponent<RectTransform>().position += new Vector3(-(this.GetComponent<RectTransform>().sizeDelta.x/2), 0, 0); //move this request to the left half my size
    }

    public void ChangeToSingle()
    {
        Popup.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 66); //reduce UI scrollview to one request
        Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(66, Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y); //reduce box colldier to new size
        this.transform.parent.GetComponent<RectTransform>().right = Vector3.zero; //reduce content to one reqeust
        this.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, this.transform.parent.GetComponent<RectTransform>().sizeDelta.y); //reduce it more
        foreach (Transform child in this.transform.parent) //search for brothers
        {
            if (child != this.transform) //find brother
            {
                child.GetComponent<RectTransform>().localPosition = new Vector2(33, child.GetComponent<RectTransform>().localPosition.y); //move brother to single request spot
            }
        }
    }
}
