using UnityEngine;

public class PersonTrigger : MonoBehaviour
{
    public MusicManager musicManager;
    public string personName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Persona"))
        {
            musicManager.PlaySpecialMusic(personName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Persona"))
        {
            musicManager.ResumePlaylist();
        }
    }
}
