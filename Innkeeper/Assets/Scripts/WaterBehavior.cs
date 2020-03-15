using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    public int Size = 3;
    private int CurrentSize;

    // Start is called before the first frame update
    void Start()
    {
        CurrentSize = Size; //Initialize CurrentSize
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        CurrentSize--; //decrease current size
        this.transform.localScale = new Vector2(this.transform.localScale.x, this.transform.localScale.y * ((float)CurrentSize / Size)); //Squish object
        if(CurrentSize <= 0) //if size is 0 or less
        {
            Destroy(this.gameObject); //Destroy object
        }
    }
}
