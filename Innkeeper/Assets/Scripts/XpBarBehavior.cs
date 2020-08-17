using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpBarBehavior : MonoBehaviour
{
    public float originalPercentage = 0;
    private float goalPercentage = 0;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goalPercentage <= this.GetComponent<Animation>()["XP Bar"].normalizedTime)
        {
            this.GetComponent<Animation>().Stop();
            if(goalPercentage < 1)
            {
                originalPercentage = goalPercentage;
            }
            else
            {
                originalPercentage = 0;
            }
        }
    }

    public void setSizeByPercentage(float Percentage)
    {
        this.GetComponent<Animation>()["XP Bar"].normalizedSpeed = .5f;
        this.GetComponent<Animation>()["XP Bar"].time = originalPercentage;
        Debug.Log("Orginally at " + originalPercentage + " Percent");
        originalPercentage = 0;
        this.GetComponent<Animation>().Play("XP Bar");
        this.goalPercentage = Percentage;
    }

}
