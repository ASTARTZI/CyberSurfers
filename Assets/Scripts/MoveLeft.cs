using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float leftBoundary = -15f;

    private PlayerController playerController;
    private SpawnManager spawnManager;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    private void Update()
    {
        if (playerController != null && !playerController.gameOver)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (transform.position.x < leftBoundary)
        {
            if (gameObject.CompareTag("Obstacle") && spawnManager != null)
            {
                spawnManager.ObstaclePassed();
            }

            Destroy(gameObject);
        }
    }
}