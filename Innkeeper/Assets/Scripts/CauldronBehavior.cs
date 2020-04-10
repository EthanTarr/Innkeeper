using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronBehavior : MonoBehaviour
{
    public Transform HeldItem;
    public Sprite EmptyCauldron;
    public Sprite PastaCauldron;

    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        if(Player == null)
        {
            Debug.LogError(name + " could not find player on startup");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HeldItem == null)
        {
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = EmptyCauldron;
        }
    }

    public void grabItem()
    {
        bool check = Player.GetComponent<PlayerBehavior>().GiveObject(HeldItem);
        if(check)
        {
            HeldItem = null;
        }
        Player.GetComponent<PlayerBehavior>().checkHand();
    }
}
