using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFruitArea : MonoBehaviour
{
    //four blue fruit spaces starting from upper left and continuing clockwise
    private static Transform FruitSpace1 = null;
    private static Transform FruitSpace2 = null;
    private static Transform FruitSpace3 = null;
    private static Transform FruitSpace4 = null;

    //Timer
    private static Transform myTimer = null;

    public Transform Timer;
    public Transform BlueFruit;
    public float TimeDelay = 4f; //length of gathering time in seconds

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Creates timer and invokes 'endtime' function after TimeDelay seconds
    void OnMouseDown()
    {
        if (myTimer == null)
        {
            myTimer = Instantiate(Timer, transform.position, Timer.rotation);
            Invoke("endTime", TimeDelay);
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

            //creates fruit if none is there
            if (FruitSpace1 == null)
            {
                FruitSpace1 = Instantiate(BlueFruit, (Vector2)transform.position + new Vector2(-3.5f, 3.5f), BlueFruit.rotation);
            }
            if (FruitSpace2 == null)
            {
                FruitSpace2 = Instantiate(BlueFruit, (Vector2)transform.position + new Vector2(3.5f, 3.5f), BlueFruit.rotation);
            }
            if (FruitSpace3 == null)
            {
                FruitSpace3 = Instantiate(BlueFruit, (Vector2)transform.position + new Vector2(3.5f, -3.5f), BlueFruit.rotation);
            }
            if (FruitSpace4 == null)
            {
                FruitSpace4 = Instantiate(BlueFruit, (Vector2)transform.position + new Vector2(-3.5f, -3.5f), BlueFruit.rotation);
            }
        }
    }
}
