using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Ταχύτητα μετακίνησης του αντικειμένου
    [SerializeField] private float speed = 5f;

    // Όριο εξόδου από τη σκηνή
    [SerializeField] private float leftBoundary = -15f;

    private PlayerController playerController;
    private SpawnManager spawnManager;

    private void Start()
    {
        // Εύρεση των βασικών scripts του παιχνιδιού
        playerController = FindFirstObjectByType<PlayerController>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    private void Update()
    {
        // Κίνηση μόνο όσο το παιχνίδι δεν έχει τελειώσει
        if (playerController != null && !playerController.gameOver)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Έλεγχος αν το αντικείμενο βγήκε εκτός ορίων
        if (transform.position.x < leftBoundary)
        {
            // Αν είναι εμπόδιο, ενημερώνεται ο SpawnManager ότι προσπεράστηκε
            if (gameObject.CompareTag("Obstacle") && spawnManager != null)
            {
                spawnManager.ObstaclePassed();
            }

            // Καταστροφή του αντικειμένου
            Destroy(gameObject);
        }
    }
}