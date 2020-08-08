using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canBuy : MonoBehaviour
{
    private Button myButton;
    private PlayerBehavior Player;

    public Transform Food;
    
    // Start is called before the first frame update
    void Start()
    {
        myButton = this.GetComponent<Button>();
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.name.Equals("Buy Noodle") && GameObject.Find("Player").GetComponent<PlayerBehavior>().Level < 3)
        {
            myButton.interactable = false;
        }
        else if (this.name.Equals("Buy Acid Fly") && GameObject.Find("Player").GetComponent<PlayerBehavior>().Level < 6)
        {
            myButton.interactable = false;
        }
        else
        {
            Transform LeftPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().LeftHandObject;
            Transform RightPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().RightHandObject;
            if (LeftPlayerObject != null && RightPlayerObject != null)
            {
                if ((LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite) &&
                    LeftPlayerObject.GetComponent<ItemBehavior>().ItemCount < LeftPlayerObject.GetComponent<ItemBehavior>().ItemMax) ||
                    (RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite) &&
                    RightPlayerObject.GetComponent<ItemBehavior>().ItemCount < RightPlayerObject.GetComponent<ItemBehavior>().ItemMax))
                {
                    if (Player.money < Food.GetComponent<ItemBehavior>().ItemValue)
                    {
                        myButton.interactable = false;
                    }
                    else
                    {
                        myButton.interactable = true;
                    }
                }
                else
                {
                    myButton.interactable = false;
                }
            }
            else
            {
                if (Player.money < Food.GetComponent<ItemBehavior>().ItemValue)
                {
                    myButton.interactable = false;
                }
                else
                {
                    myButton.interactable = true;
                }
            }
        }
    }
}
