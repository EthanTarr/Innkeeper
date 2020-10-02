using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseInfo : MonoBehaviour
{
    private PlayerBehavior Player;

    public int Cost = 0;
    public int StartingCost = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        updateCost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCost()
    {
        this.transform.GetChild(1).GetComponent<Text>().text = (Cost / 200) + "";
        this.transform.GetChild(3).GetComponent<Text>().text = ((Cost % 200) / 10) + "";
        this.transform.GetChild(5).GetComponent<Text>().text = (Cost % 10) + "";
    }

    public void Purchase()
    {
        if(Player == null)
        {
            Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        }
        Player.money -= Cost;
        GameObject.Find("UI Out").GetComponent<AudioSource>().Play();
        this.transform.parent.gameObject.SetActive(false);
    }
}
