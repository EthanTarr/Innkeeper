﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    public List<Transform> Arrows;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startCounting(float time)
    {
        InvokeRepeating("nextStep", time / 8f, time / 8f);
    }

    private void nextStep()
    {
        count++;
        Transform arrow = this.transform.GetChild(count);
        switch (count)
        {
            case 1:
                arrow.localPosition = new Vector2(.4f, .32f);
                arrow.localRotation = Quaternion.Euler(0, 0, 0);
                arrow.GetComponent<SpriteRenderer>().flipX = false;
                arrow.GetComponent<SpriteRenderer>().flipY = true;
                break;
            case 2:
                arrow.localPosition = new Vector2(.4f, -.32f);
                arrow.localRotation = Quaternion.Euler(0, 0, 0);
                arrow.GetComponent<SpriteRenderer>().flipX = false;
                arrow.GetComponent<SpriteRenderer>().flipY = false;
                break;
            case 3:
                arrow.localPosition = new Vector2(.32f, -.4f);
                arrow.localRotation = Quaternion.Euler(0, 0, -90);
                arrow.GetComponent<SpriteRenderer>().flipX = false;
                arrow.GetComponent<SpriteRenderer>().flipY = true;
                break;
            case 4:
                arrow.localPosition = new Vector2(-.32f, -.4f);
                arrow.localRotation = Quaternion.Euler(0, 0, -90);
                arrow.GetComponent<SpriteRenderer>().flipX = false;
                arrow.GetComponent<SpriteRenderer>().flipY = false;
                break;
            case 5:
                arrow.localPosition = new Vector2(-.4f, -.32f);
                arrow.localRotation = Quaternion.Euler(0, 0, 0);
                arrow.GetComponent<SpriteRenderer>().flipX = true;
                arrow.GetComponent<SpriteRenderer>().flipY = false;
                break;
            case 6:
                arrow.localPosition = new Vector2(-.4f, .32f);
                arrow.localRotation = Quaternion.Euler(0, 0, 180);
                arrow.GetComponent<SpriteRenderer>().flipX = false;
                arrow.GetComponent<SpriteRenderer>().flipY = false;
                break;
            case 7:
                arrow.localPosition = new Vector2(-.32f, .4f);
                arrow.localRotation = Quaternion.Euler(0, 0, -90);
                arrow.GetComponent<SpriteRenderer>().flipX = true;
                arrow.GetComponent<SpriteRenderer>().flipY = false;
                break;
        }

        if(count == 8)
        {
            Destroy(this.gameObject);
        }
    }
}