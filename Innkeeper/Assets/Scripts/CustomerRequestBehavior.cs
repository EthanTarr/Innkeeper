using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomerRequestBehavior : MonoBehaviour
{
    public GameObject Popup; // parent Popup object

    private Transform Customer; // customer Transform
    private GameObject ResourceCounter;
    private ArrayList Items = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        Items.Add("Blue Fruit");
        Items.Add("Water");
        Items.Add("Blue Fruit Juice");
        string RequestedItem = (string) Items[UnityEngine.Random.Range(0, Items.Count)];
        ResourceCounter = GameObject.Find(RequestedItem + " UI Counter");
        if (ResourceCounter == null)
        {
            Debug.LogError(name + " could not find ResourceCounter on startup");
        }
        GameObject Image = GameObject.Find(RequestedItem + " UI Image");
        if (Image == null)
        {
            Debug.LogError(name + " could not find Image on startup");
        }
        else
        {
            this.GetComponent<Image>().sprite = Image.GetComponent<Image>().sprite;
        }

        if(Popup == null)
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
        if (ResourceCounter == null) //Check for Resource UI Counter Object
        {
            Debug.LogError(name + " Resource UI Counter could not be found after FullfilRequest() startup.");
        }
        else
        {
            int counter = -1; //Initialize Counter
            try
            {
                counter = int.Parse(ResourceCounter.GetComponent<Text>().text); //get current resource count from UI
            }
            catch (Exception e)
            {
                Debug.LogError(name + " Resource Counter is not an int. " + e);
            }
            if (counter > 0) //check for any collected resources
            {
                ResourceCounter.GetComponent<Text>().text = counter + -1 + ""; //remove one save new resource count
                ResourceCounter.GetComponent<CounterBehaviour>().onChange(); //signify that the value has been changed
                if(Customer == null)
                {
                    Debug.LogError(name + " could not find Customer Transform after decrementing the counter.");
                } else
                {
                    if (this.transform.parent.transform.childCount <= 1)
                    {
                        Destroy(Customer.gameObject); //Destroy Customer
                        Destroy(Popup); //Destroy Parent Popup
                    }
                    else if(this.transform.parent.transform.childCount == 2)
                    {
                        ChangeToSingle();
                    } else
                    {
                        bool foundSelf = false;
                        foreach (Transform child in this.transform.parent)
                        {
                            if(!foundSelf)
                            {
                                if(child == this.transform)
                                {
                                    foundSelf = true;
                                }
                                child.GetComponent<RectTransform>().position += new Vector3(25, 0, 0);
                            }
                            else
                            {
                                child.GetComponent<RectTransform>().position += new Vector3(-25, 0, 0);
                            }
                        }
                        this.transform.parent.GetComponent<RectTransform>().sizeDelta += new Vector2(-50, 0);
                    }

                    Destroy(this.gameObject); //Destroy this gameobject
                }
            }
        }
    }

    public void ChangeToMultiple()
    {
        Popup.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 132);
        Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(132, Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y);
        this.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position + new Vector3(-(this.GetComponent<RectTransform>().sizeDelta.x/2), 0, 0);
    }

    public void ChangeToSingle()
    {
        Popup.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 66);
        Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(66, Popup.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y);
        this.transform.parent.GetComponent<RectTransform>().right = Vector3.zero;
        this.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, this.transform.parent.GetComponent<RectTransform>().sizeDelta.y);
        foreach (Transform child in this.transform.parent)
        {
            if (child != this.transform)
            {
                child.GetComponent<RectTransform>().localPosition = new Vector2(33, child.GetComponent<RectTransform>().localPosition.y);
            }
        }
    }
}
