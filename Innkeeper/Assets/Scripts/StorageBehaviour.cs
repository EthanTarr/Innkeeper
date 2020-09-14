using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBehaviour : MonoBehaviour
{
    public Transform LeftObject;
    public Transform CenterObject;
    public Transform RightObject;
    public Transform Highlight;

    private Transform Player;
    
    // Start is called before the first frame update
    void Start()
    {
        if(Highlight == null)
        {
            Debug.LogError(name + " could not find Highlight on startup");
        }
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceObject(Transform Object)
    {
        if (Highlight.gameObject.activeSelf)
        {
            if (Highlight.position.x < this.transform.position.x - this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
            {
                LeftObject = Object;
                LeftObject.position = new Vector2(this.transform.position.x - this.GetComponent<SpriteRenderer>().bounds.size.x / 3, Highlight.transform.position.y);
            }
            else if (Highlight.position.x < this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
            {
                CenterObject = Object;
                CenterObject.position = new Vector2(this.transform.position.x, Highlight.transform.position.y);
            }
            else
            {
                RightObject = Object;
                RightObject.position = new Vector2(this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x / 3, Highlight.transform.position.y);
            }
            
        }
        else
        {
            Debug.LogError(name + " could not find Highlight.");
        }
    }

    public Transform GatherObject()
    {
        if (Highlight.gameObject.activeSelf)
        {
            if (Highlight.position.x < this.transform.position.x - this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
            {
                return LeftObject;
            }
            else if (Highlight.position.x < this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
            {
                return CenterObject;
            }
            else
            {
                return RightObject;
            }
        }
        else
        {
            Debug.LogError(name + " could not find Highlight.");
            return null;
        }
    }

    public void RemoveObject()
    {
        if (Highlight.gameObject.activeSelf)
        {
            if (Highlight.position.x < this.transform.position.x - this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
            {
                LeftObject = null;
            }
            else if (Highlight.position.x < this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
            {
                CenterObject = null;
            }
            else
            {
                RightObject = null;
            }
        }
        else
        {
            Debug.LogError(name + " could not find Highlight.");
        }
    }

    public bool isFull()
    {
        return LeftObject != null && CenterObject != null && RightObject != null;
    }

    public bool contains(Sprite Object)
    {
        return LeftObject.GetComponent<SpriteRenderer>().sprite.Equals(Object) || CenterObject.GetComponent<SpriteRenderer>().sprite.Equals(Object) || RightObject.GetComponent<SpriteRenderer>().sprite.Equals(Object);
    }

    public Transform retrieve(Sprite Object)
    {
        if(LeftObject.GetComponent<SpriteRenderer>().sprite.Equals(Object))
        {
            return LeftObject;
        }
        else if (CenterObject.GetComponent<SpriteRenderer>().sprite.Equals(Object))
        {
            return CenterObject;
        }
        else if (RightObject.GetComponent<SpriteRenderer>().sprite.Equals(Object))
        {
            return RightObject;
        }
        return null;
    }

    public void Decode(int[] foods)
    {
        LeftObject = Player.GetComponent<ResourceManager>().Foods[foods[0]];
        CenterObject = Player.GetComponent<ResourceManager>().Foods[foods[1]];
        RightObject = Player.GetComponent<ResourceManager>().Foods[foods[2]];
    }

    public int[] Encode()
    {
        int[] temp = new int[3];
        int count = 0;
        foreach(Transform food in Player.GetComponent<ResourceManager>().Foods)
        {
            if(LeftObject != null && LeftObject.name.Equals(food.name))
            {
                temp[0] = count;
            }
            if (CenterObject != null && CenterObject.name.Equals(food.name))
            {
                temp[1] = count;
            }
            if (RightObject != null && RightObject.name.Equals(food.name))
            {
                temp[2] = count;
            }
        }
        return temp;
    }

    private void moveHighlight(Collider2D collision)
    {
        Vector2 PlayerPosition = collision.transform.position;
        if (PlayerPosition.x < this.transform.position.x - this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
        {
            Highlight.transform.position = new Vector2(this.transform.position.x - this.GetComponent<SpriteRenderer>().bounds.size.x / 3, Highlight.transform.position.y);
        }
        else if (PlayerPosition.x < this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x / 6)
        {
            Highlight.transform.position = new Vector2(this.transform.position.x, Highlight.transform.position.y);
        }
        else
        {
            Highlight.transform.position = new Vector2(this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x / 3, Highlight.transform.position.y);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        moveHighlight(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Highlight.gameObject.SetActive(true);
        moveHighlight(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Highlight.gameObject.SetActive(false);
    }
}
