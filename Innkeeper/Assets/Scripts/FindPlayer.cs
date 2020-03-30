using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    private Transform Player;
    private Vector3 Destination;
    private Vector3 Velocity = Vector3.zero;

    public float smoothTime = .3f; //time it takes to smooth the motion
    public float minYValue = -1; //min y value the camera is allowed to move
    public float maxYValue = 1; //max y value the camera is allowed to move
    public float minXValue = -1; //min x value the camera is allowed to move
    public float maxXValue = 1; //max x value the camera is allowed to move

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform; //Finds player object
        if(Player == null)
        {
            Debug.LogError(name + " could not find player on startup.");
        }
        Destination = transform.position; //Find the player postion the first time
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
            Vector3 temp = Destination; //temp save old destination
            Destination = Player.TransformPoint(new Vector3(0, 0, -10)); //Find player postion with offset
            if (Destination.y < minYValue) //if camera is going to move below minYValue
            {
                Destination = new Vector3(Destination.x, temp.y, Destination.z); //new destination will have old y value
            }
            if (Destination.y > maxYValue) //if camera is going to move above maxYValue
            {
                Destination = new Vector3(Destination.x, temp.y, Destination.z); //new destination will have old y value
            }
            if (Destination.x < minXValue) //if camera is going to move below minXValue
            {
                Destination = new Vector3(temp.x, Destination.y, Destination.z); //new destination will have old x value
            }
            if (Destination.x > maxXValue) //if camera is going to move above maxXValue
            {
                Destination = new Vector3(temp.x, Destination.y, Destination.z); //new destination will have old x value
            }
            transform.position = Vector3.SmoothDamp(transform.position, Destination, ref Velocity, smoothTime); //change current position towards the player position
        }
    }
}
