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
                    }
                    Destroy(this.gameObject); //Destroy Parent Popup
                }
            }
        }
    }

    public void ChangeToMultiple()
    {
        Popup.GetComponentInChildren<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 132);
        this.GetComponent<RectTransform>().position = new Vector2(-(this.GetComponent<RectTransform>().sizeDelta.x/2), 0);
    }

    private void ChangeToSingle()
    {
        Popup.GetComponentInChildren<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 66);
        foreach (Transform child in this.transform.parent)
        {
            if (child != this.gameObject)
            {
                this.GetComponent<RectTransform>().position = new Vector2(0, 0);
            }
        }
    }
}
