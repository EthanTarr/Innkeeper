using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public List<AudioClip> Voices;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stopSounds()
    {
        StopAllCoroutines();
    }

    public void startHumming()
    {
        StartCoroutine("Humming");
    }

    IEnumerator Humming()
    {
        while (true)
        {
            if (!this.GetComponent<AudioSource>().isPlaying)
            {
                int rand = Random.Range(0, 5);
                this.GetComponent<AudioSource>().clip = Voices[rand];
                this.GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(Random.Range(20, 30));
        }
    }

    public void StartDay()
    {
        int rand = Random.Range(5, 8);
        this.GetComponent<AudioSource>().clip = Voices[rand];
        this.GetComponent<AudioSource>().Play();
    }

    public void EndDay()
    {
        int rand = Random.Range(8, 11);
        this.GetComponent<AudioSource>().clip = Voices[rand];
        this.GetComponent<AudioSource>().Play();
    }

    public void Order()
    {
        int rand = Random.Range(11, 13);
        this.GetComponent<AudioSource>().clip = Voices[rand];
        this.GetComponent<AudioSource>().Play();
    }

    public void Lift()
    {
        this.GetComponent<AudioSource>().clip = Voices[13];
        this.GetComponent<AudioSource>().Play();
    }

    public void Goblins()
    {
        this.GetComponent<AudioSource>().clip = Voices[14];
        this.GetComponent<AudioSource>().Play();
    }
}
