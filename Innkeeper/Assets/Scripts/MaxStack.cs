using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxStack : MonoBehaviour
{
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.gameObject.name.Equals("LeftHandUI")) {
            if (int.Parse(this.GetComponent<Text>().text) == player.GetComponent<PlayerBehavior>().LeftHandObject.GetComponent<ItemBehavior>().ItemMax)
            {
                this.GetComponent<Text>().color = new Color(255, 0, 0, 255);
            }
            else
            {
                this.GetComponent<Text>().color = new Color(0, 0, 0, 255);
            }
        }
        else
        {
            if (int.Parse(this.GetComponent<Text>().text) == player.GetComponent<PlayerBehavior>().RightHandObject.GetComponent<ItemBehavior>().ItemMax)
            {
                this.GetComponent<Text>().color = new Color(255, 0, 0, 255);
            }
            else
            {
                this.GetComponent<Text>().color = new Color(0, 0, 0, 255);
            }
        }
    }
}
