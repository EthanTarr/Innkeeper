using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPopupBehavor : MonoBehaviour
{

    public void activatePasta()
    {
        this.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 96);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0);
        this.transform.GetChild(2).localPosition = new Vector3(-58, 0);
        this.transform.GetChild(3).gameObject.SetActive(true);
    }
}
