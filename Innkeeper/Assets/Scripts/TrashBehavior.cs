using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehavior : MonoBehaviour
{
    public Transform Highlight;

    private PlayerBehavior Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Player.LeftHandObject != null || Player.RightHandObject != null)
        {
            Highlight.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Highlight.gameObject.SetActive(false);
    }
}
