using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBehaviour : MonoBehaviour
{
    private Vector2 Destination;
    private Vector3 Velocity = Vector3.zero;

    public float smoothTime = .3f; //time it takes to smooth the motion
    public Transform PopupObject;

    public float PopupOffsetX = 0f; //popup offset from object in x
    public float PopupOffsetY = 0f; //popup offset from object in y

    // Start is called before the first frame update
    void Start()
    {
        Destination = transform.position; //Initialize Destination
        if(PopupObject == null) //check for PopupObject
        {
            string ObjectName = name.Split(' ')[0]; //grab object name
            PopupObject = GameObject.Find(ObjectName).transform; //attempt to find object from name
            if (PopupObject == null) //check again for PopupObject
            {
                Debug.Log(name + " could not find PopupObject on startup.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PopupObject == null) //check for player
        {
            Debug.LogError(name + " could not find PopupObject on update.");
        }
        else
        {
            Destination = Camera.main.WorldToScreenPoint(PopupObject.TransformPoint(new Vector2(PopupOffsetX, PopupOffsetY))); //Find PopupObject postion with offset coordinates and convert to screen coordinates
            transform.position = Vector3.SmoothDamp(transform.position, Destination, ref Velocity, smoothTime); //change current position towards the player position
        }
    }
}
