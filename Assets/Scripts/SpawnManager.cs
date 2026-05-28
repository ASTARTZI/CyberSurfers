using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] groundObstaclePrefabs;
    [SerializeField] private GameObject[] airObstaclePrefabs;
    [SerializeField] private GameObject rewardPrefab;

    [SerializeField] private int maxObstacles = 20;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float repeatRate = 3f;

    [SerializeField] private Vector3 groundSpawnPosition = new Vector3(12f, 0f, 0f);
    [SerializeField] private Vector3 airSpawnPosition = new Vector3(12f, 1.4f, 0f);

    [SerializeField] private Vector3 groundRewardOffset = new Vector3(-5f, 1.2f, 0f);
    [SerializeField] private Vector3 airRewardOffset = new Vector3(-7f, 0.8f, 0f);

    [SerializeField] private bool randomObstacleOrder = true;

    private int spawnedObstacles = 0;
    private int passedObstacles = 0;

    private PlayerController playerController;
    private GameManager gameManager;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        gameManager = FindFirstObjectByType<GameManager>();
        InvokeRepeating(nameof(SpawnObstacleAndReward), startDelay, repeatRate);
    }

    private void SpawnObstacleAndReward()
    {
        if (playerController != null && playerController.gameOver)
        {
            CancelInvoke(nameof(SpawnObstacleAndReward));
            return;
        }

        if (spawnedObstacles >= maxObstacles)
        {
            CancelInvoke(nameof(SpawnObstacleAndReward));
            return;
        }

        bool spawnAirObstacle = randomObstacleOrder ? Random.value > 0.5f : spawnedObstacles % 2 != 0;

        Vector3 spawnPosition = spawnAirObstacle ? airSpawnPosition : groundSpawnPosition;
        Vector3 rewardOffset = spawnAirObstacle ? airRewardOffset : groundRewardOffset;

        GameObject selectedObstacle = null;

        if (spawnAirObstacle)
        {
            if (airObstaclePrefabs != null && airObstaclePrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, airObstaclePrefabs.Length);
                selectedObstacle = airObstaclePrefabs[randomIndex];
            }
        }
        else
        {
            if (groundObstaclePrefabs != null && groundObstaclePrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, groundObstaclePrefabs.Length);
                selectedObstacle = groundObstaclePrefabs[randomIndex];
            }
        }

        if (selectedObstacle != null)
        {
            Instantiate(selectedObstacle, spawnPosition, selectedObstacle.transform.rotation);
        }

        if (rewardPrefab != null)
        {
            Instantiate(rewardPrefab, spawnPosition + rewardOffset, rewardPrefab.transform.rotation);
        }

        spawnedObstacles++;
    }

    public void ObstaclePassed()
    {
        passedObstacles++;

        if (passedObstacles >= maxObstacles && gameManager != null)
        {
            gameManager.Congratulations();
        }
    }
}