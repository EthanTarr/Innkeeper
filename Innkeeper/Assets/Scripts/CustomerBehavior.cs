using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    public List<Sprite> Customers;
    
    // Start is called before the first frame update
    void Start()
    {
        int thisCustomer = Random.Range(0, Customers.Count);
        this.GetComponent<SpriteRenderer>().sprite = Customers[thisCustomer];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
