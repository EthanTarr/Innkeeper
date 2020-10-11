using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canFill : MonoBehaviour
{
    public float resetTime = 60;

    private PlayerBehavior Player;
    public bool reseting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!reseting)
        {
            if(this.GetComponent<Button>().interactable && !this.transform.parent.GetComponent<PopupBehaviour>().PopupObject.GetComponent<CauldronBehavior>().isEmpty)
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (!this.GetComponent<Button>().interactable && this.transform.parent.GetComponent<PopupBehaviour>().PopupObject.GetComponent<CauldronBehavior>().isEmpty)
            {
                this.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void reset()
    {
        reseting = true;
        StartCoroutine("Reinteract");
    }

    IEnumerator Reinteract()
    {
        resetTime = 60 - (Player.Level * 1.5f);
        yield return new WaitForSeconds(resetTime);
        foreach(Transform popup in Player.GetComponent<GameManager>().CauldronPopups)
        {
            popup.GetChild(5).GetComponent<Button>().interactable = true;
        }
        reseting = false;
    }
}
