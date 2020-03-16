using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGatheringActionBehavior : MonoBehaviour
{
    public Transform BlueFruitArea;

    // Start is called before the first frame update
    void Start()
    {
        if (BlueFruitArea == null)
        {
            BlueFruitArea = GameObject.Find("Blue Fruit Area").transform;
            if (BlueFruitArea == null)
            {
                Debug.LogError(name + " could not find Blue Fruit Area transform on startup.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // GatherWater() places timer on Blue Fruit Area and calls function to increase Water
    public void GatherWater()
    {
        if (BlueFruitArea.GetComponent<BlueFruitArea>().myTimer == null) //Check for if timer isnt running
        {
            BlueFruitArea.GetComponent<BlueFruitArea>().myTimer = Instantiate(BlueFruitArea.GetComponent<BlueFruitArea>().Timer, BlueFruitArea.transform.position, BlueFruitArea.GetComponent<BlueFruitArea>().Timer.rotation); //create timer
            Invoke("endTime", BlueFruitArea.GetComponent<BlueFruitArea>().TimeDelay); //run function endTime() after TimerDelay time
        }
    }

    // endTime() calls Blue Fruit Area script's endTime()
    private void endTime()
    {
        BlueFruitArea.GetComponent<BlueFruitArea>().endWaterGather();
    }
}
