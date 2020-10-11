using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> Music;

    private int track = 0;
    private List<int> previousTracks = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        track = Random.Range(0, Music.Count);
        playSong();
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.GetComponent<AudioSource>().isPlaying)
        {
            playSong();
        }
    }

    public void playSong()
    {
        StopAllCoroutines();
        StartCoroutine(PlaySong());
    }

    IEnumerator PlaySong()
    {
        while(true) {
            
            while (previousTracks.Contains(track))
            {
                track = Random.Range(0, Music.Count);
            }
            this.GetComponent<AudioSource>().clip = Music[track];
            previousTracks.Add(track);
            if (previousTracks.Count > 3)
            {
                previousTracks.RemoveAt(0);
            }
            this.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(this.GetComponent<AudioSource>().clip.length);
        }
    }
}
