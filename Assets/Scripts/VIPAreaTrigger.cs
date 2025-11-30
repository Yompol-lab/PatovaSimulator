using UnityEngine;

public class VIPAreaTrigger : MonoBehaviour
{
   
    public bool playerHasVIP = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Persona"))
        {
            
            if (!playerHasVIP)
            {
                Debug.Log("NO tenés acceso al VIP.");
                other.transform.position -= other.transform.forward * 2f;
                return;
            }

            Debug.Log("Bienvenido al VIP!");
        }
    }
}