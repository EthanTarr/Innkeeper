using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpBarBehavior : MonoBehaviour
{
    private float maxWidth = 375;
    private float originalPercentage = 0;
    private float goalPercentage = 0;

    private float AnimationProgress = 0;
    public float AnimationTime = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetXpBar()
    {
        originalPercentage = 0;
        goalPercentage = 0;
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
        rt.localPosition = new Vector2(-maxWidth / 2, rt.localPosition.y);
    }

    public void setSizeByPercentage(float Percentage, float AnimationTime)
    {
        goalPercentage = Percentage;
        this.AnimationTime = AnimationTime;
        StartCoroutine(IncreaseXpBar());
    }

    IEnumerator IncreaseXpBar()
    {
        bool animate = true;
        while (animate)
        {
            AnimationProgress += (1 / 60f) / AnimationTime;
            if (AnimationProgress >= 1)
            {
                originalPercentage = goalPercentage;
                AnimationProgress = 0;
                animate = false;
            }
            RectTransform rt = this.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x + (maxWidth * (goalPercentage - originalPercentage) * ((1 / 60f) / AnimationTime)), rt.sizeDelta.y);
            rt.localPosition = new Vector2(rt.localPosition.x + ((maxWidth * (goalPercentage - originalPercentage) * ((1 / 60f) / AnimationTime)) / 2), rt.localPosition.y);
            yield return new WaitForSeconds((1 / 60f) / AnimationTime);
        }
    }
}
