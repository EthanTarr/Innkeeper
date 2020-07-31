using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodBehavior : MonoBehaviour
{
    public Sprite UnhappyIdicator;
    
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animation>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(this.GetComponent<Animation>()["Indication"].normalizedTime >= 1)
        {
            Destroy(this.transform.parent.gameObject);
        }*/
        if(!this.GetComponent<Animation>().isPlaying)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
