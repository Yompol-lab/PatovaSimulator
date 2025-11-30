using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Source principal")]
    public AudioSource audioSource;

    [Header("Playlist Normal")]
    public AudioClip[] playlist;
    private int currentIndex = 0;

    [Header("Música Especial por Persona")]
    public List<PersonMusicData> specialMusics = new List<PersonMusicData>();

    [Header("Fade")]
    public float fadeTime = 1.5f;

    
    [Range(0f, 1f)]
    public float maxVolume = 0.5f;

    private bool playingSpecial = false;
    private AudioClip currentSpecialClip;

    [Header("Sincronización de Luces (opcional)")]
    public float currentAudioLevel = 0f;

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
        {
            PlayNextSong();
        }

        
        currentAudioLevel = Mathf.Abs(audioSource.volume * Mathf.Sin(Time.time * 10));
    }

    

    void PlayNextSong()
    {
        if (playlist == null || playlist.Length == 0) return;

        AudioClip nextClip = playlist[currentIndex];
        currentIndex = (currentIndex + 1) % playlist.Length;

        StartCoroutine(FadeTo(nextClip, false));
    }

   

    public void PlaySpecialMusic(string personName)
    {
        foreach (var data in specialMusics)
        {
            if (data.personName == personName)
            {
                if (data.musicClips == null || data.musicClips.Length == 0)
                {
                    Debug.LogWarning("La persona " + personName + " no tiene músicas asignadas.");
                    return;
                }

                
                AudioClip randomClip = data.musicClips[Random.Range(0, data.musicClips.Length)];

                currentSpecialClip = randomClip;
                playingSpecial = true;
                StartCoroutine(FadeTo(currentSpecialClip, true));
                return;
            }
        }

        Debug.LogWarning("No se encontró música especial para: " + personName);
    }

    
    public void ResumePlaylist()
    {
        playingSpecial = false;
        PlayNextSong();
    }

    

    private IEnumerator FadeTo(AudioClip newClip, bool special)
    {
        float startVolume = audioSource.volume;

        
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        
        playingSpecial = special;

        
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, maxVolume, t / fadeTime);
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
