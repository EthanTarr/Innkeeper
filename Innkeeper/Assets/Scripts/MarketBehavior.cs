using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketBehavior : MonoBehaviour
{
    private bool flourCheck = false;
    private bool jarCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelCheck(int level)
    {
        if(level > 2 && !flourCheck)
        {
            for(int i = 0; i < this.transform.childCount; i++)
            {
                if(this.transform.GetChild(i).name.Equals("Flour"))
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                    flourCheck = true;
                    break;
                }
            }
        }

        if (level > 5 && !jarCheck)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).name.Equals("Jar"))
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                    jarCheck = true;
                    break;
                }
            }
        }
    }
}
