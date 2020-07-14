using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public int ItemCount = 1;
    public int ItemMax = 3;
    public float ItemWeight = .1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(ItemCount < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
