using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehavior : MonoBehaviour
{
    public Transform Customer;
    public GameObject CustomerPopup;
    public Transform Door;
    public Vector3 Offset = new Vector3(0, -2, 0);
    public Vector3 Offset1 = new Vector3(2, 0, 0);
    public Vector3 Offset2 = new Vector3(-2, 0, 0);
    public bool isStool = false;
    //public List<Transform> Path;

    [HideInInspector] public Transform CurrentCustomer;
    [HideInInspector] public Transform CurrentCustomer1;
    [HideInInspector] public Transform CurrentCustomer2;

    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        if (Customer == null)
        {
            Debug.LogError(name + " could not find Customer on startup");
        }
        if (CustomerPopup == null)
        {
            Debug.LogError(name + " could not find Customer Popup on startup");
        }
        if (CustomerPopup == null)
        {
            Debug.LogError(name + " could not find Customer Popup on startup");
        }
        Player = GameObject.Find("Player").transform;
    }

    public bool SpawnCustomer()
    {
        if ((!isStool && (CurrentCustomer == null || CurrentCustomer1 == null || CurrentCustomer2 == null)) || (isStool && CurrentCustomer == null))
        {
            Transform customer = Instantiate(Customer, Door.transform.position, Customer.rotation); //create customer object
            int PathChoice = 0;
            int choices = 0;
            if (!isStool)
            {
                if (CurrentCustomer == null)
                {
                    choices++;
                }
                if (CurrentCustomer1 == null)
                {
                    choices++;
                }
                if (CurrentCustomer2 == null)
                {
                    choices++;
                }
            }
            else
            {
                choices = 1;
            }
            int spot = UnityEngine.Random.Range(1, choices);

            if (CurrentCustomer == null)
            {
                if (spot == 1)
                {
                    CurrentCustomer = customer;
                    PathChoice = 0;
                }
                spot--;
            }

            if (CurrentCustomer1 == null && spot > 0)
            {
                if (spot == 1)
                {
                    CurrentCustomer1 = customer;
                    PathChoice = 1;
                }
                spot--;
            }
            
            if (CurrentCustomer2 == null && spot > 0)
            {
                CurrentCustomer2 = customer;
                PathChoice = 2;
            }
            customer.GetComponent<CustomerBehavior>().Table = this.transform;
            List<Vector2> customerPath = new List<Vector2>();
            customerPath.Add(Door.transform.position);
            for (int i = 0; i < this.transform.GetChild(PathChoice).childCount; i++)
            {
                customerPath.Add(this.transform.GetChild(PathChoice).GetChild(i).position);
            }
            if (PathChoice == 0)
            {
                customerPath.Add(this.transform.position + Offset);
            }
            else if (PathChoice == 1)
            {
                customerPath.Add(this.transform.position + Offset1);
            }
            else
            {
                customerPath.Add(this.transform.position + Offset2);
            }
            customer.GetComponent<CustomerBehavior>().setPath(customerPath);

            GameObject popup = Instantiate(CustomerPopup, Camera.main.WorldToScreenPoint(customer.transform.position), CustomerPopup.transform.rotation); //create popup object
            popup.transform.SetParent(GameObject.Find("Canvas").transform, false); //place popup in canvas
            popup.transform.SetAsFirstSibling();

            int RequestAmount;
            float rand = Random.Range(0f, 100f);
            if (rand < (70 - (Player.GetComponent<GameManager>().TimelineCount * Player.GetComponent<GameManager>().SpawnTimerIncreaseAmount * .1f)))
            {
                RequestAmount = 0;
            }
            else if (rand < 110 - (Player.GetComponent<GameManager>().TimelineCount * Player.GetComponent<GameManager>().SpawnTimerIncreaseAmount * .05f))
            {
                RequestAmount = 1;
            }
            else
            {
                RequestAmount = 2;
            }
            if (RequestAmount > 0)
            {
                popup.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<CustomerRequestBehavior>().ChangeToMultiple(); //set UI to have multiple requests
            }
            for (int i = 0; i < RequestAmount; i++)
            {
                GameObject popupChild = popup.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; //grab request object
                GameObject newRequest = Instantiate(popupChild, popup.transform.position +
                    new Vector3((popupChild.GetComponent<RectTransform>().sizeDelta.x * (i + 1)) - (popupChild.GetComponent<RectTransform>().sizeDelta.x * .5f), 0, 0), popupChild.transform.rotation); //create new reqeust
                newRequest.transform.SetParent(popupChild.transform.parent);
                if (i > 0)
                {
                    popupChild.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(popupChild.transform.parent.GetComponent<RectTransform>().sizeDelta.x +
                        popupChild.GetComponent<RectTransform>().sizeDelta.x, popupChild.transform.parent.GetComponent<RectTransform>().sizeDelta.y); //increase UI content size to hold more requests
                    foreach (Transform child in popupChild.transform.parent)
                    {
                        child.GetComponent<RectTransform>().position = child.GetComponent<RectTransform>().position + new Vector3(-popupChild.transform.parent.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0); //move the requests to fit in UI
                    }
                }
            }

            customer.GetComponent<PopUpObjectBehavior>().Popup = popup.transform; //set customer to have popup
            popup.GetComponent<PopupBehaviour>().PopupObject = customer; //set popup to have customer
            customer.gameObject.SetActive(true); //turn on customer object
            popup.gameObject.SetActive(true); //turn on popup object
            return true;
        }
        else
        {
            return false;
        }
    }
}
