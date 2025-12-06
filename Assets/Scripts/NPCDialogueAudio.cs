using UnityEngine;

public class NPCDialogueAudio : MonoBehaviour
{
    [Header("Audios de diálogo")]
    public AudioClip preguntarDNI;
    public AudioClip entrada;
    public AudioClip cacheo;
    public AudioClip vestimenta;
    public AudioClip comportamiento;
    public AudioClip dejarPasar;
    public AudioClip denegarAcceso;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayPreguntarDNI() { Play(preguntarDNI); }
    public void PlayEntrada() { Play(entrada); }
    public void PlayCacheo() { Play(cacheo); }
    public void PlayVestimenta() { Play(vestimenta); }
    public void PlayComportamiento() { Play(comportamiento); }
    public void PlayDejarPasar() { Play(dejarPasar); }
    public void PlayDenegarAcceso() { Play(denegarAcceso); }

    private void Play(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("NPCDialogueAudio: no hay AudioClip asignado.");
            return;
        }

        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }
}
