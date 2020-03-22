using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [HideInInspector] public bool controlMovement = true;

    public float MovementSpeed = 15f; //movement speed of the player character
    private Vector2 Destination;
    
    // Start is called before the first frame update
    void Start()
    {
        Destination = transform.position; //find destination position
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && controlMovement) //check for left mouse click
        {
            Collider2D worldClick = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //get all colliders in the world where the mouse is positioned
            Collider2D ScreenClick = Physics2D.OverlapPoint(Input.mousePosition); //get all colliders in the screen where the mouse is positioned
            if (!worldClick.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Interactable")) && !ScreenClick.transform.gameObject.layer.Equals(LayerMask.NameToLayer("UI"))) //if the colliding objects are not Interactables in the world or UI elements on the screen
            {
                Destination = Camera.main.ScreenToWorldPoint(Input.mousePosition); //change destination to mouse cursor location
            }
        }
        if (Mathf.Abs((transform.position - (Vector3)Destination).magnitude) > .1f) //if player is farther than .1 from detination (Optimize)
        {
            transform.position = Vector2.MoveTowards(transform.position, Destination, MovementSpeed * Time.deltaTime); //move player towards destination
        }
    }


}
