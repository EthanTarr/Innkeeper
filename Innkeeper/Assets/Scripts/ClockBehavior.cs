using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockBehavior : MonoBehaviour
{
    public Sprite Deactivated;
    public Sprite Morning;
    public Sprite Noon;
    public Sprite Dark;

    private GameManager Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTime();
    }

    private void checkTime()
    {
        int Time = Player.TimelineCount;

        if(Time > 0 && Time < Player.DayTimeLimit)
        {
            if(Time < Player.DayTimeLimit / 3)
            {
                this.GetComponent<Image>().sprite = Morning;
            }
            else if (Time < 2 * Player.DayTimeLimit / 3)
            {
                this.GetComponent<Image>().sprite = Noon;
            }
            else
            {
                this.GetComponent<Image>().sprite = Dark;
            }
        }
        else
        {
            this.GetComponent<Image>().sprite = Deactivated;
        }
    }
}
