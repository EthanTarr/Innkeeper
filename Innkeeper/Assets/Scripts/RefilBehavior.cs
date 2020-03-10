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
        if (Bucket == null)
        {
            if (myTimer == null)
            {
                myTimer = Instantiate(Timer, transform.position, Timer.rotation);
                Invoke("endTime", TimeDelay);
            }
        }
    }

    //Destroys timer and creates four blue fruits in their spaces
    void endTime()
    {
        if (myTimer == null)
        {
            Debug.LogError("Blue Fruit Patch Timer is null");
        }
        else
        {
            Destroy(myTimer.gameObject);

            Bucket = Instantiate(Water, this.gameObject.transform.position + new Vector3(-15f, 0, 0), Water.rotation);
        }
    }
}
