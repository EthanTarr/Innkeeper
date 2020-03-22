using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    private Transform Player;
    private Vector3 Destination;
    private Vector3 Velocity = Vector3.zero;

    public float smoothTime = .3f; //time it takes to smooth the motion
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform; //Finds player object
        if(Player == null)
        {
            Debug.LogError(name + " could not find player on startup.");
        }
        Destination = Player.position; //Find the player postion the first time
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null) //check for player
        {
            Debug.LogError(name + " could not find player on update.");
        }
        else
        {
            Destination = Player.TransformPoint(new Vector3(0, 0, -10)); //Find player postion with offset
            transform.position = Vector3.SmoothDamp(transform.position, Destination, ref Velocity, smoothTime); //change current position towards the player position
        }
    }
}
