using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayAwhileButtonBehavior : MonoBehaviour
{
    public GameObject Popup; // parent Popup object

    private Transform Customer; // customer Transform
    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        if (Popup == null)
        {
            Debug.LogError(name + " could not find Popup parent object on startup");
        }
        else
        {
            Customer = Popup.GetComponent<PopupBehaviour>().PopupObject; //set Customer Transform to be parent Popup's Customer Tranform
        }
        Player = GameObject.Find("Player").transform;
        if (Player == null)
        {
            Debug.LogError(name + " could not find player on statup");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetTimer()
    {
        Customer.GetComponent<CustomerBehavior>().myTimer.GetComponent<TimerBehavior>().reset();
    }

    public void ResetSkill()
    {
        foreach (Transform customer in Player.GetComponent<GameManager>().Customers)
        {
            customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).GetComponent<Button>().interactable = false;
        }
        Player.GetComponent<GameManager>().Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).GetComponent<Button>().interactable = false;
        StartCoroutine("Reactivate");
    }

    IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(60 - 3 * Player.GetComponent<PlayerBehavior>().Level);
        foreach (Transform customer in Player.GetComponent<GameManager>().Customers)
        {
            customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).GetComponent<Button>().interactable = true;
        }
        Player.GetComponent<GameManager>().Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).GetComponent<Button>().interactable = true;
    }
}
