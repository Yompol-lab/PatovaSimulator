using UnityEngine;

public class PersonTrigger : MonoBehaviour
{
    public MusicManager musicManager;
    public string personName;

   
    public bool isVIPPerson = false;

    
    public bool playerHasVIP = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Persona"))
        {
           
            if (isVIPPerson && !playerHasVIP)
            {
                Debug.Log("No tenés VIP para hablar con esta persona.");
                return;
            }

            musicManager.PlaySpecialMusic(personName);
        }
    }

   
}
