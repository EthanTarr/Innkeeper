using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBookBehavior : MonoBehaviour
{
    private bool touching = false;

    private Transform Player;

    public Sprite closedBook;
    public Sprite openBook;

    public GameObject cookBookPopup;
    public GameObject TitleScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(touching && Input.GetMouseButtonDown(1))
        {
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = openBook;
            cookBookPopup.SetActive(true);
            if(!TitleScreen.activeSelf)
            {
                Time.timeScale = 0;
                GameObject.Find("Player").GetComponent<AudioSource>().Stop();
                GameObject.Find("CraftingTable").GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void closingBook()
    {
        if (!TitleScreen.activeSelf)
        {
            Time.timeScale = 1;
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = closedBook;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("Player"))
        {
            touching = true;
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            touching = false;
            this.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
