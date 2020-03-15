using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefilBehavior : MonoBehaviour
{
    public Transform Water;
    private Transform Bucket;

    //Timer
    private static Transform myTimer = null;
    public Transform Timer;
    public float TimeDelay = 4f; //length of gathering time in seconds

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Bucket == null) //If water bucket is empty
        {
            if (myTimer == null) //If there isnt a timer running
            {
                myTimer = Instantiate(Timer, transform.position, Timer.rotation); //create timer
                Invoke("endTime", TimeDelay); //call endtime() after TimeDelay time
            }
        }
    }

    //Destroys timer and creates four blue fruits in their spaces
    void endTime()
    { 
        if (myTimer == null) //if timer isnt created
        {
            Debug.LogError(name + " Refil Timer is null on endTime() startup.");
        }
        else
        {
            Destroy(myTimer.gameObject); //destroy timer

            if (Water == null) //If water isnt defined
            {
                Debug.LogError(name + " Water Transform is null after refil timer end.");
            } else {
                Bucket = Instantiate(Water, this.gameObject.transform.position + new Vector3(-15f, 0, 0), Water.rotation); //create water in Bucket
            }
        }
    }
}
