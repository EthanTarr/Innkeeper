using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseInfo : MonoBehaviour
{
    private PlayerBehavior Player;

    public int Cost = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Cost += int.Parse(this.transform.GetChild(1).GetComponent<Text>().text) * 200;
        Cost += int.Parse(this.transform.GetChild(3).GetComponent<Text>().text) * 10;
        Cost += int.Parse(this.transform.GetChild(5).GetComponent<Text>().text);
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    }

    public void Purchase()
    {
        Player.money -= Cost;
        GameObject.Find("UI Out").GetComponent<AudioSource>().Play();
        Destroy(this.transform.parent.gameObject);
    }
}
