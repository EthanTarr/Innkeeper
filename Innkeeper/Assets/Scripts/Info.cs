using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Info : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Description = "Blank";
    public float width = 100;
    public float height = 50;
    public Vector2 Offset = new Vector2(0, 3);


    public Transform ToolTip;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable)
        {
            ToolTip.GetComponent<ToolTipBehavior>().Offset = this.Offset;
            ToolTip.GetComponent<ToolTipBehavior>().HoverObject = this.transform;
            ToolTip.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            ToolTip.GetChild(0).GetComponent<Text>().text = Description;
            ToolTip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject ExitObject = eventData.pointerCurrentRaycast.gameObject;
        if (ToolTip.GetChild(0).GetComponent<Text>().text == Description && ExitObject != null && !ExitObject.name.Equals("Tool Tip"))
        {
            ToolTip.gameObject.SetActive(false);
        }
    }
}
