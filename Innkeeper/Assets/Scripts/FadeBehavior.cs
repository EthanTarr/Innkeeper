using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehavior : MonoBehaviour
{
    private SoundManager GameMusic;
    private float tempVolume;
    private float currentVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        GameMusic = GameObject.Find("Game Music").GetComponent<SoundManager>();
        ToggleFade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleFade()
    {
        if(this.gameObject.activeSelf)
        {
            tempVolume = GameMusic.RetrieveDesiredVolume();
            currentVolume = 0;
            GameMusic.AudioControl(0);
            StartCoroutine(FadeOut());
        }
        else
        {
            this.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        while (this.GetComponent<Image>().color.a <= 1)
        {
            this.GetComponent<Image>().color += new Color(0, 0, 0, .01f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOut()
    {
        while (this.GetComponent<Image>().color.a >= 0)
        {
            this.GetComponent<Image>().color += new Color(0, 0, 0, -.01f);
            currentVolume += tempVolume / 100f;
            GameMusic.AudioControl(currentVolume);
            yield return new WaitForEndOfFrame();
        }
        GameMusic.AudioControl(tempVolume);
        this.gameObject.SetActive(false);
    }
}
