using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    // Ταχύτητα μετακίνησης του background
    [SerializeField] private float speed = 5f;

    // Πλάτος ενός κομματιού background
    [SerializeField] private float width = 112.8f;

    // Αρχική θέση στον άξονα Χ
    private float startX;

    // Αναφορά στον παίκτη για έλεγχο game over
    private PlayerController player;

    void Start()
    {
        // Αποθήκευση της αρχικής θέσης
        startX = transform.position.x;

        // Εύρεση του PlayerController
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        // Το background κινείται μόνο όσο το παιχνίδι συνεχίζεται
        if (player != null && !player.gameOver)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        // Όταν βγει εκτός ορίου, μεταφέρεται μπροστά για αδιάκοπο scrolling
        if (transform.position.x <= startX - width)
        {
            transform.position += Vector3.right * width * 3f;
        }
    }
}