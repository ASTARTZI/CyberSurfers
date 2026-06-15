using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Αν το αντικείμενο που άγγιξε το coin είναι ο παίκτης
        if (other.CompareTag("Player"))
        {
            // Καταστροφή του coin μετά τη συλλογή του
            Destroy(gameObject);
        }
    }
}