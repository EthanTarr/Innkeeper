using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasePopupBehavior : MonoBehaviour
{

    public void activatePasta()
    {
        this.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.transform.GetChild(0).GetComponent<RectTransform>().rect.width + 50);
        this.transform.GetChild(0).localPosition = new Vector3(this.transform.GetChild(0).localPosition.x + 25, 0);
        this.transform.GetChild(1).localPosition = new Vector3(this.transform.GetChild(1).localPosition.x + 50, 0);
        this.transform.GetChild(5).gameObject.SetActive(true);
    }

    public void activateFlys()
    {
        this.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.transform.GetChild(0).GetComponent<RectTransform>().rect.width + 50);
        this.transform.GetChild(0).localPosition = new Vector3(this.transform.GetChild(0).localPosition.x + 25, 0);
        this.transform.GetChild(1).localPosition = new Vector3(this.transform.GetChild(1).localPosition.x + 50, 0);
        this.transform.GetChild(6).gameObject.SetActive(true);
    }
}
