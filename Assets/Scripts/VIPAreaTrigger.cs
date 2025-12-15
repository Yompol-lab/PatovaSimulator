using UnityEngine;

public class VIPAreaTrigger : MonoBehaviour
{
    public MusicManager musicManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Persona"))
            return;

        PersonTrigger person = other.GetComponent<PersonTrigger>();
        if (person == null)
        {
            Debug.LogWarning("La persona que entró al VIP no tiene PersonTrigger.");
            return;
        }

        if (!person.playerHasVIP)
        {
            Debug.Log("NO tenés acceso al VIP, " + person.personName);
            other.transform.position -= other.transform.forward * 2f;
            return;
        }

        Debug.Log("Bienvenido al VIP " + person.personName + "! Cambiando música...");
        if (musicManager != null)
        {
            musicManager.PlaySpecialMusic(person.personName);
        }
        else
        {
            Debug.LogWarning("MusicManager no asignado en VIPAreaTrigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Persona"))
            return;

        PersonTrigger person = other.GetComponent<PersonTrigger>();
        if (person == null)
            return;

        
        if (musicManager != null)
        {
            Debug.Log("Saliendo del VIP " + person.personName + ". Volviendo a música normal...");
            musicManager.ResumePlaylist();
        }
    }
}
