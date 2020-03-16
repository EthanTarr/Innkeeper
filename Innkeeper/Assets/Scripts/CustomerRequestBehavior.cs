using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomerRequestBehavior : MonoBehaviour
{
    public GameObject Popup; // parent Popup object

    private Transform Customer; // customer Transform

    // Checks to see if Blue Fruit Counter can be legally decremented. If it can, then the customer and this parent object are deleted
    public void FullfilRequest()
    {
        GameObject BlueFruitCounter = GameObject.Find("Blue Fruit UI Counter"); //Grab Blue Fruti UI Counter Object
        if (BlueFruitCounter == null) //Check for Blue Fruit UI Counter Object
        {
            Debug.LogError(name + " Blue Fruit UI Counter could not be found after FullfilRequest() startup.");
        }
        else
        {
            int counter = -1; //Initialize Counter
            try
            {
                counter = int.Parse(BlueFruitCounter.GetComponent<Text>().text); //get current blue fruit count from UI
            }
            catch (Exception e)
            {
                Debug.LogError(name + " Blue Fruit Counter is not an int. " + e);
            }
            if (counter > 0) //check for any collected blue fruits
            {
                BlueFruitCounter.GetComponent<Text>().text = counter + -1 + ""; //remove one save new blue fruit count
                if(Customer == null)
                {
                    Customer = GameObject.Find(Popup.name.Split(' ')[0]).transform;
                    if(Customer == null)
                    {
                        Debug.LogError(name + " could not find Customer Transform after decrementing the counter.");
                    } else
                    {
                        Destroy(Customer.gameObject); //Destroy Customer
                        Destroy(Popup); //Destroy Parent Popup
                    }
                }
            }
        }
    }
}
