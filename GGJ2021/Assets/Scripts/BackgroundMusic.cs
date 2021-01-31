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
        // Place the current song in the player and play it
        source.clip = songs[currentSong];
        source.Play();
        // Wait for the song to play out the full duration
        yield return new WaitForSeconds(songs[currentSong].length);
        // Increment to the next song, and loop it back to the first song if there isnt a song at that index
        currentSong++;
        if(currentSong >= songs.Count)
        {
            currentSong = 0;
        }
        // Start the new song
        StartCoroutine(backgroundMusic());
    }
}
