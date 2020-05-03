using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipBehavior : MonoBehaviour, IPointerEnterHandler
{
    private Vector2 Destination;
    private Vector3 Velocity = Vector3.zero;

    public float smoothTime = .3f;
    public Transform HoverObject;
    public Vector2 Offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destination = HoverObject.TransformPoint(new Vector2(Offset.x, Offset.y)); //Find PopupObject postion with offset coordinates and convert to screen coordinates
        transform.position = Vector3.SmoothDamp(transform.position, Destination, ref Velocity, smoothTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
}
