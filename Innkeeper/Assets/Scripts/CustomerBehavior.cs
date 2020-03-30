using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    public List<Sprite> Customers;

    public float MovementSpeed = 15f; //movement speed of the customer character
    [HideInInspector] public Vector2 Destination;
    
    // Start is called before the first frame update
    void Start()
    {
        int thisCustomer = Random.Range(0, Customers.Count);
        this.GetComponent<SpriteRenderer>().sprite = Customers[thisCustomer];
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs((transform.position - (Vector3)Destination).magnitude) > .1f) //if customer is farther than .1 from destination (Optimize)
        {
            Vector2 move = Vector2.MoveTowards(transform.position, Destination, MovementSpeed * Time.deltaTime);
            transform.position = move; //move customer towards destination
        }
    }
}
