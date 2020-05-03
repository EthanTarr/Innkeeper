using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpObjectBehavior : MonoBehaviour
{
    public Transform Popup; //Popup UI Transform
    public float InitialTurnOffTimer = 5f; //Timer for when popup is initially deactivated

    private Transform Player; //Player Transform

    // Start is called before the first frame update
    void Start()
    {
        if(Popup == null) //check for Popup UI object
        {
            Popup = GameObject.Find(name + " Popup").transform; //Attempt to find Popup UI object
            if (Popup == null) //check again for Popup UI object
            {
                Debug.LogError(name + " could not find Popup Transform on startup.");
            }
        }
        Popup.gameObject.SetActive(true); //Initially turn the popup on
        Invoke("TurnOff", InitialTurnOffTimer); //Invoke TurnOff function after InitialTurnOffTimer time

        Player = GameObject.Find("Player").transform; //Attempt to find Player object
        if(Player == null) //check for Player object
        {
            Debug.LogError(name + " could not find Player Transform on startup.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnTriggerEnter2D is called when two colliders initially interact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player == null) //check for Player object
        {
            Debug.LogError(name + " could not find Player Tranform on collision enter.");
        }
        else if (collision.transform.Equals(Player)) //check if collision was by Player object
        {
            if (Popup == null) //check for Popup UI object
            {
                Debug.LogError(name + " could not find Popup on collision enter");
            }
            Popup.gameObject.SetActive(true); //turn on popup UI object
            CancelInvoke(); //cancel all Invoke calls
        }
    }

    // OnTriggerExit2D is called when two colliders exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Player == null) //check for player
        {
            Debug.Log(name + " could not find Player Tranform on collision exit.");
        }
        else if (collision.transform.Equals(Player)) //check if collision was by Player object
        {
            if (Popup == null) //check for popup UI object
            {
                Debug.LogError(name + " could not find Popup on collision exit");
            }
            Popup.gameObject.SetActive(false); //Turn off Popup UI object 
            GameObject Tooltip = GameObject.Find("Tool Tip");
            if (Tooltip != null)
            {
                Tooltip.SetActive(false);
            }
        }
    }

    // TurnOff sets the popup object to off
    private void TurnOff()
    {
        if (Popup != null)
        {
            Popup.gameObject.SetActive(false); //Turn off Popup UI object
        }
    }
}
