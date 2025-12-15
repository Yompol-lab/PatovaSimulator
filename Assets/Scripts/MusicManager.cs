using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] playlist;
    private int currentIndex = 0;

    private AudioClip resumeClip;
    private float resumeTime;

    public List<PersonMusicData> specialMusics;

    public float fadeTime = 1.2f;
    public float maxVolume = 0.5f;

    private bool playingVIP = false;
    private int vipInsideCount = 0;

    private Coroutine transitionRoutine;
    private Coroutine waitEndRoutine;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.volume = maxVolume;
        PlayNormal();
    }

    public void NotifyVipEntered(string vipName)
    {
        vipInsideCount++;

        if (!playingVIP)
        {
            resumeClip = audioSource.clip;
            resumeTime = audioSource.time;
        }

        AudioClip vipClip = GetVipClip(vipName);
        if (vipClip != null)
            StartTransition(vipClip, true, 0f);
    }

    public void NotifyVipExited()
    {
        vipInsideCount = Mathf.Max(0, vipInsideCount - 1);
    }

    void StartTransition(AudioClip clip, bool vip, float startTime)
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        if (waitEndRoutine != null)
            StopCoroutine(waitEndRoutine);

        transitionRoutine = StartCoroutine(Transition(clip, vip, startTime));
    }

    IEnumerator Transition(AudioClip clip, bool vip, float startTime)
    {
        float startVol = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.time = startTime;
        audioSource.Play();

        playingVIP = vip;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = maxVolume;
        transitionRoutine = null;

        waitEndRoutine = StartCoroutine(WaitForClipEnd(vip));
    }

    IEnumerator WaitForClipEnd(bool wasVIP)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        if (wasVIP)
            PlayNormalFromResume();
        else
            PlayNextNormal();
    }

    void PlayNormal()
    {
        if (playlist == null || playlist.Length == 0) return;

        currentIndex %= playlist.Length;
        StartTransition(playlist[currentIndex], false, 0f);
    }

    void PlayNextNormal()
    {
        currentIndex = (currentIndex + 1) % playlist.Length;
        PlayNormal();
    }

    void PlayNormalFromResume()
    {
        playingVIP = false;

        if (resumeClip != null)
            StartTransition(resumeClip, false, resumeTime);
        else
            PlayNormal();
    }

    AudioClip GetVipClip(string name)
    {
        foreach (var p in specialMusics)
        {
            if (p.personName == name && p.musicClips != null && p.musicClips.Length > 0)
                return p.musicClips[Random.Range(0, p.musicClips.Length)];
        }

        return null;
    }
}

[System.Serializable]
public class PersonMusicData
{
    public string personName;
    public AudioClip[] musicClips;
}
