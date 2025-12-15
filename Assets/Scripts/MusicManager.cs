using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Playlist Normal")]
    public AudioClip[] playlist;
    private int currentIndex = 0;

   
    private AudioClip basePlaylistClip;
    private float basePlaylistTime = 0f;

    [Header("Música Especial por Persona")]
    public List<PersonMusicData> specialMusics = new List<PersonMusicData>();

    public float fadeTime = 1.5f;
    public float maxVolume = 0.5f;

    private bool playingSpecial = false;
    private bool waitingTransition = false;
    private int vipInsideCount = 0;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.volume = maxVolume;
        PlayNextSong();
    }

    void Update()
    {
        
        if (!playingSpecial && audioSource.isPlaying)
        {
            basePlaylistTime = audioSource.time;
        }

        
        if (!audioSource.isPlaying && !waitingTransition && playingSpecial)
        {
            ResumePlaylist();
            return;
        }

        
        if (!audioSource.isPlaying && !waitingTransition && !playingSpecial)
        {
            PlayNextSong();
        }
    }

    

    void PlayNextSong()
    {
        if (playlist == null || playlist.Length == 0) return;

        AudioClip nextClip = playlist[currentIndex];
        currentIndex = (currentIndex + 1) % playlist.Length;

        basePlaylistClip = nextClip;
        basePlaylistTime = 0f;

        waitingTransition = true;
        StartCoroutine(FadeTo(nextClip, false, 0f));
    }

    

    public void PlaySpecialMusic(string personName)
    {
        
        if (!playingSpecial)
        {
            basePlaylistClip = audioSource.clip;
            basePlaylistTime = audioSource.time;
        }

        foreach (var data in specialMusics)
        {
            if (data.personName == personName && data.musicClips.Length > 0)
            {
                AudioClip randomClip =
                    data.musicClips[Random.Range(0, data.musicClips.Length)];

                playingSpecial = true;
                waitingTransition = true;

                StartCoroutine(FadeTo(randomClip, true, 0f));
                return;
            }
        }

        Debug.LogWarning("No se encontró música VIP para: " + personName);
    }

    public void ResumePlaylist()
    {
        if (basePlaylistClip == null)
            return;

        playingSpecial = false;
        waitingTransition = true;

        
        StartCoroutine(FadeTo(basePlaylistClip, false, basePlaylistTime));
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

        playingSpecial = special;

       
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = maxVolume;
        waitingTransition = false;
    }

    

    public void NotifyVipEntered(string personName)
    {
        vipInsideCount++;
        PlaySpecialMusic(personName);
    }

    public void NotifyVipExited()
    {
        vipInsideCount = Mathf.Max(0, vipInsideCount - 1);

        if (vipInsideCount == 0)
            ResumePlaylist();
    }
}

[System.Serializable]
public class PersonMusicData
{
    public string personName;
    public AudioClip[] musicClips;
}
