using UnityEngine;

public class VIPAreaTrigger : MonoBehaviour
{
    public MusicManager musicManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Persona")) return;

        var p = other.GetComponent<PersonTrigger>();
        if (p == null || !p.playerHasVIP) return;

        musicManager.NotifyVipEntered(p.personName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Persona")) return;

        var p = other.GetComponent<PersonTrigger>();
        if (p == null || !p.playerHasVIP) return;

        musicManager.NotifyVipExited();
    }

}
