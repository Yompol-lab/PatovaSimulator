using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IllegalItem"))
        {
            Destroy(other.gameObject);
        }
    }
}
