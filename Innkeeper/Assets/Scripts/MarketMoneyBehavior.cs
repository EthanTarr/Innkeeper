using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketMoneyBehavior : MonoBehaviour
{
    private int currentValue = 0;
    private PlayerBehavior Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.money != currentValue)
        {
            moneyToCurrency(Player.money);
            currentValue = Player.money;
        }
        Player.GetComponent<GameManager>().MarketTime += Time.deltaTime;
    }

    private void moneyToCurrency(int money)
    {
        this.transform.GetChild(6).GetComponent<Text>().text = (money / 200) + "";
        money = money % 200;
        this.transform.GetChild(4).GetComponent<Text>().text = (money / 10) + "";
        money = money % 10;
        this.transform.GetChild(2).GetComponent<Text>().text = money + "";
    }
}
