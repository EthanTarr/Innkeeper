using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehavior : MonoBehaviour
{
    public Transform Popup;

    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        if(Popup == null)
        {
            Debug.LogError(name + " could not find Popup Transform on startup.");
        }
        Popup.gameObject.SetActive(false); //Turn off Popup

        Player = GameObject.Find("Player").transform;
        if(Player == null)
        {
            Debug.LogError(name + " could not find Player Transform on startup.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player == null)
        {
            Debug.Log(name + " could not find Player Tranform on collision enter.");
        }
        else if (collision.transform.Equals(Player))
        {
            if (Popup == null)
            {
                Debug.LogError(name + " could not find Popup on collision enter");
            }
            Popup.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Player == null)
        {
            Debug.Log(name + " could not find Player Tranform on collision exit.");
        }
        else if (collision.transform.Equals(Player))
        {
            if (Popup == null)
            {
                Debug.LogError(name + " could not find Popup on collision exit");
            }
            Popup.gameObject.SetActive(false);
        }
    }
}
