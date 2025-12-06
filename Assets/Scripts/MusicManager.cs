using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Playlist Normal")]
    public AudioClip[] playlist;
    private int currentIndex = 0;
    private float resumeTime = 0f;   
    private AudioClip resumeClip;    

    [Header("Música Especial por Persona")]
    public List<PersonMusicData> specialMusics = new List<PersonMusicData>();

    public float fadeTime = 1.5f;
    public float maxVolume = 0.5f;

    private bool playingSpecial = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.volume = maxVolume;
        PlayNextSong();
    }

    void Update()
    {
        if (!audioSource.isPlaying && !playingSpecial)
            PlayNextSong();
    }

    void PlayNextSong()
    {
        if (playlist == null || playlist.Length == 0) return;

        AudioClip nextClip = playlist[currentIndex];
        currentIndex = (currentIndex + 1) % playlist.Length;

        StartCoroutine(FadeTo(nextClip, false, 0f));
    }

    public void PlaySpecialMusic(string personName)
    {
        
        resumeClip = audioSource.clip;
        resumeTime = audioSource.time;

        foreach (var data in specialMusics)
        {
            if (data.personName == personName && data.musicClips.Length > 0)
            {
                AudioClip randomClip = data.musicClips[Random.Range(0, data.musicClips.Length)];
                playingSpecial = true;

                StartCoroutine(FadeTo(randomClip, true, 0f));
                return;
            }
        }
    }

    public void ResumePlaylist()
    {
        playingSpecial = false;

        
        StartCoroutine(FadeTo(resumeClip, false, resumeTime));
    }

    private IEnumerator FadeTo(AudioClip newClip, bool special, float startTime)
    {
        float startVol = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.clip = newClip;
        audioSource.time = startTime;
        audioSource.Play();

        // Set flag
        playingSpecial = special;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }
}

[System.Serializable]
public class PersonMusicData
{
    public string personName;
    public AudioClip[] musicClips;
}
