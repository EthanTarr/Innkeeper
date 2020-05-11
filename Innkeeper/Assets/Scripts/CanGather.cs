using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanGather : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.parent.gameObject.activeSelf)
        {
            if (this.name.Equals("Noodle Gather Action") && GameObject.Find("Player").GetComponent<PlayerBehavior>().Level < 3)
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (this.name.Equals("Acid Fly Gather Action") && GameObject.Find("Player").GetComponent<PlayerBehavior>().Level < 6)
            {
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                Transform LeftPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().LeftHandObject;
                Transform RightPlayerObject = GameObject.Find("Player").GetComponent<PlayerBehavior>().RightHandObject;
                if (LeftPlayerObject != null && RightPlayerObject != null)
                {
                    if ((LeftPlayerObject != null && LeftPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)) ||
                        (RightPlayerObject != null && RightPlayerObject.GetComponent<SpriteRenderer>().sprite.Equals(this.GetComponent<Image>().sprite)))
                    {
                        this.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        this.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    this.GetComponent<Button>().interactable = true;
                }
            }


            if(this.name.Equals("Noodle Gather Action") && GameObject.Find("Player").GetComponent<PlayerBehavior>().MovementSpeed < 
                ((.16f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 3))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (this.name.Equals("Acid Fly Gather Action") && GameObject.Find("Player").GetComponent<PlayerBehavior>().MovementSpeed <
                ((1f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 1))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (this.name.Equals("Water Gather Action") && GameObject.Find("Player").GetComponent<PlayerBehavior>().MovementSpeed <
                ((.4f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 1))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (this.name.Equals("Blue Fruit Gather Action") && GameObject.Find("Player").GetComponent<PlayerBehavior>().MovementSpeed <
                ((.2f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 3))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.GetComponent<Button>().interactable = true;
            }
        }
    }
}
