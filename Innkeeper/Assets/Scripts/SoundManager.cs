using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject MasterSlider;

    public float MaxVolume = 1f;

    private float previousDesiredVolume = 1;

    public void AudioControl(float desiredVolume)
    {
        this.GetComponent<AudioSource>().volume = desiredVolume * MasterSlider.GetComponent<Slider>().value * MaxVolume;
        previousDesiredVolume = desiredVolume;
    }

    public void AudioControl()
    {
        this.GetComponent<AudioSource>().volume = previousDesiredVolume * MasterSlider.GetComponent<Slider>().value * MaxVolume;
    }
}
