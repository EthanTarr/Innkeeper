﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    public List<Transform> Arrows;

    public Sprite GreenArrow;
    public Sprite OrangeArrow;
    public Sprite RedArrow;

    [HideInInspector] public int count = 0;
    private float time;

    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if(Player == null)
        {
            Debug.LogError(name + " could not find Player");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reset()
    {
        StopAllCoroutines();
        count = 0;
        for (int i = 0; i < this.transform.childCount - 1; i++)
        {
            this.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = GreenArrow;
            this.transform.GetChild(i).localPosition = new Vector2(.32f, .4f);
            this.transform.GetChild(i).localRotation = Quaternion.Euler(0, 0, 90);
            this.transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = false;
            this.transform.GetChild(i).GetComponent<SpriteRenderer>().flipY = false;
        }
        startCounting(this.time);
    }

    public void startCounting(float time)
    {
        this.time = time;
        StartCoroutine(nextStep());
    }

    IEnumerator nextStep()
    {
        while (true)
        {
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
                    for(int i = 0; i < this.transform.childCount - 1; i++)
                    {
                        this.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = OrangeArrow;
                    }
                    break;
                case 7:
                    arrow.localPosition = new Vector2(-.32f, .4f);
                    arrow.localRotation = Quaternion.Euler(0, 0, -90);
                    arrow.GetComponent<SpriteRenderer>().flipX = true;
                    arrow.GetComponent<SpriteRenderer>().flipY = false;
                    for (int i = 0; i < this.transform.childCount - 1; i++)
                    {
                        this.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = RedArrow;
                    }
                    break;
            }

            if (count == 8)
            {
                Player.GetComponent<GameManager>().Timers.Remove(this.transform);
                Destroy(this.gameObject);
            }

            count++;

            yield return new WaitForSeconds(time / 8f);
        }
    }
}
