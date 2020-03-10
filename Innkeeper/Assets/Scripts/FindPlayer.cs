using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    private GameObject Player;
    private Vector3 Destination;
    private Vector3 Velocity = Vector3.zero;

    public float smoothTime = .3f;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        if(Player == null)
        {
            Debug.LogError("Could not find player.");
            Destroy(this);
        }
        Destination = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Destination = Player.transform.TransformPoint(new Vector3(0, 0, -10));
        transform.position = Vector3.SmoothDamp(transform.position, Destination, ref Velocity, smoothTime);
    }
}
