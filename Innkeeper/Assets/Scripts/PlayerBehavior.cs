using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float MovementSpeed = 15f;
    private Vector2 Destination;
    
    // Start is called before the first frame update
    void Start()
    {
        Destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        transform.position = Vector2.MoveTowards(transform.position, Destination, MovementSpeed * Time.deltaTime);
    }


}
