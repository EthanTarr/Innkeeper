using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Customer;
    public GameObject CustomerPopup;

    public float SpawnTime = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        if(Customer == null)
        {
            Debug.LogError(name + " could not find Customer on startup");
        }
        if(CustomerPopup == null)
        {
            Debug.LogError(name + " could not find Customer Popup on startup");
        }
        StartCoroutine(SpawnCustomer()); //This allows the funtion to run on a timer
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCustomer()
    {
        Vector2 position;
        while (true)
        {
            position = new Vector2(Random.Range(-100, 100) * 1f, Random.Range(-100,100) * .05f); //create random location
            Transform customer = Instantiate(Customer, position, Customer.rotation); //create customer object

            GameObject popup = Instantiate(CustomerPopup, Camera.main.WorldToScreenPoint(customer.transform.position), CustomerPopup.transform.rotation); //create popup object
            popup.transform.SetParent(GameObject.Find("Canvas").transform, false); //place popup in canvas

            int RequestAmount = Random.Range(0, 3); //determine a random amount of more requests
            if(RequestAmount > 0)
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
                    foreach(Transform child in popupChild.transform.parent)
                    {
                        child.GetComponent<RectTransform>().position = child.GetComponent<RectTransform>().position + new Vector3(-popupChild.transform.parent.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0); //move the requests to fit in UI
                    }
                }
            }

            customer.GetComponent<PopUpObjectBehavior>().Popup = popup.transform; //set customer to have popup
            popup.GetComponent<PopupBehaviour>().PopupObject = customer; //set popup to have customer
            customer.gameObject.SetActive(true); //turn on customer object
            popup.gameObject.SetActive(true); //turn on popup object
            yield return new WaitForSeconds(SpawnTime); //wait for spawntime
        }
    }
}
