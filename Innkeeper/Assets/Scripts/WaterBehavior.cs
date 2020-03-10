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
        CurrentSize = Size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        CurrentSize--;
        this.transform.localScale = new Vector2(this.transform.localScale.x, this.transform.localScale.y * ((float)CurrentSize / Size));
        if(CurrentSize == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
