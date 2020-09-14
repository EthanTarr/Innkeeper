using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPopupBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateFlys()
    {
        this.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 156);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0);
        this.transform.GetChild(1).localPosition = new Vector3(88, 0);
        this.transform.GetChild(3).gameObject.SetActive(true);
    }
}
