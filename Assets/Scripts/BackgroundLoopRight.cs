using UnityEngine;

public class BackgroundLoopRight : MonoBehaviour
{
    // Ταχύτητα μετακίνησης του background
    [SerializeField] private float speed = 5f;

    // Πλάτος κάθε τμήματος background
    [SerializeField] private float width = 112.8f;

    // Κρατά τη δεξιότερη θέση για σωστή επανατοποθέτηση
    private static float rightMostX;

    // Αναφορά στον παίκτη
    private Transform player;

    private void Start()
    {
        // Εύρεση του παίκτη στη σκηνή
        player = GameObject.Find("Player").transform;

        // Ενημέρωση της δεξιότερης θέσης background
        if (transform.position.x > rightMostX)
        {
            rightMostX = transform.position.x;
        }
    }

    private void Update()
    {
        // Συνεχής κίνηση του background προς τα αριστερά
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Όταν το background περάσει πίσω από τον παίκτη,
        // μεταφέρεται μπροστά για να δημιουργείται ατελείωτο περιβάλλον
        if (transform.position.x < player.position.x - width)
        {
            rightMostX += width;
            transform.position = new Vector3(rightMostX, transform.position.y, transform.position.z);
        }
    }
}