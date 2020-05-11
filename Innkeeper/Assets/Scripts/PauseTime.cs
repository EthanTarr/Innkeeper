using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StopTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTime()
    {
        Time.timeScale = 1;
    }

    public void StopTime ()
    {
        Time.timeScale = 0;
        GameObject.Find("Player").GetComponent<AudioSource>().Stop();
        GameObject.Find("CraftingTable").GetComponent<AudioSource>().Stop();
    }

    public void ToggleScreen()
    {
        if(this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
            StartTime();
        }
        else
        {
            this.gameObject.SetActive(true);
            StopTime();
        }
    }
}
