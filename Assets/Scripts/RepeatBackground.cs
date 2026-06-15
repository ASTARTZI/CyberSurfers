using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    // Πλάτος του background για τον υπολογισμό της επανάληψης
    private float repeatWidth;

    void Start()
    {
        // Ανάκτηση του πλάτους από το BoxCollider
        repeatWidth = GetComponent<BoxCollider>().size.x;
    }

    void Update()
    {
        // Όταν το background βγει εκτός οθόνης, μεταφέρεται μπροστά
        if (transform.position.x < -repeatWidth)
        {
            transform.position += new Vector3(repeatWidth * 2, 0, 0);
        }
    }
}