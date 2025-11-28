using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Playlist Normal")]
    public AudioClip[] playlist;
    private int currentIndex = 0;

    [Header("Música Especial por Persona")]
    public List<PersonMusicData> specialMusics = new List<PersonMusicData>();

    [Header("Fade")]
    public float fadeTime = 1.5f;

    private bool playingSpecial = false;
    private AudioClip currentSpecialClip;

    [Header("Sincronización de Luces")]
    public float currentAudioLevel = 0f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        PlayNextSong();
    }

    void Update()
    {
        // Detecta si terminó la canción normal
        if (!audioSource.isPlaying && !playingSpecial)
        {
            PlayNextSong();
        }

        // Para luces sincronizadas
        currentAudioLevel = Mathf.Abs(audioSource.volume * Mathf.Sin(Time.time * 10));
    }

    void PlayNextSong()
    {
        if (playlist.Length == 0) return;

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
                currentSpecialClip = data.musicClip;
                playingSpecial = true;
                StartCoroutine(FadeTo(currentSpecialClip, true));
                return;
            }
        }
    }

    public void ResumePlaylist()
    {
        playingSpecial = false;
        PlayNextSong();
    }

    private IEnumerator FadeTo(AudioClip newClip, bool special)
    {
        float startVolume = audioSource.volume;

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 1;
    }
}


[System.Serializable]
public class PersonMusicData
{
    public string personName;
    public AudioClip musicClip;
}
