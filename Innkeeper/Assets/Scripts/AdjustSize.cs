using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            this.GetComponent<RectTransform>().sizeDelta = 
                new Vector2(this.transform.parent.GetComponent<RectTransform>().rect.size.x * .9f, this.transform.parent.GetComponent<RectTransform>().rect.size.y * .9f);
        }
    }
}
