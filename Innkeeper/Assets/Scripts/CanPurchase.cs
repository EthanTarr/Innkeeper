using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanPurchase : MonoBehaviour
{
    private int currentValue = 0;
    private PlayerBehavior Player;

    // Start is called before the first frame update
    void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.money < this.transform.parent.GetChild(1).GetComponent<PurchaseInfo>().Cost)
        {
            this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.GetComponent<Button>().interactable = true;
        }
    }
}
