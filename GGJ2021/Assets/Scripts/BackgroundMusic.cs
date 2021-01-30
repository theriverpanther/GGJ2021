using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource source;
    [SerializeField]
    List<AudioClip> songs;
    int currentSong = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.loop = true;
        StartCoroutine(backgroundMusic());
    }

    IEnumerator backgroundMusic()
    {
        source.clip = songs[currentSong];
        source.Play();
        yield return new WaitForSeconds(songs[currentSong].length);
        currentSong++;
        if(currentSong >= songs.Count)
        {
            currentSong = 0;
        }
        StartCoroutine(backgroundMusic());
    }
}
