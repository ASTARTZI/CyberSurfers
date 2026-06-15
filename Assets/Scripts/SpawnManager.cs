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

    [SerializeField] private float[] lanePositions = { -4f, 0f, 4f };

    [SerializeField] private float congratulationsDelay = 7f;

    private int spawnedObstacles = 0;
    private bool congratulationsStarted = false;

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

            if (!congratulationsStarted)
            {
                congratulationsStarted = true;
                StartCoroutine(ShowCongratulationsAfterDelay());
            }

            return;
        }

        int rewardLane = Random.Range(0, lanePositions.Length);

        for (int lane = 0; lane < lanePositions.Length; lane++)
        {
            bool spawnAirObstacle =
                randomObstacleOrder
                ? Random.value > 0.5f
                : spawnedObstacles % 2 != 0;

            Vector3 spawnPosition =
                spawnAirObstacle ? airSpawnPosition : groundSpawnPosition;

            Vector3 rewardOffset =
                spawnAirObstacle ? airRewardOffset : groundRewardOffset;

            spawnPosition.z = lanePositions[lane];

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
                Instantiate(
                    selectedObstacle,
                    spawnPosition,
                    selectedObstacle.transform.rotation
                );
            }

            if (lane == rewardLane && rewardPrefab != null)
            {
                Instantiate(
                    rewardPrefab,
                    spawnPosition + rewardOffset,
                    rewardPrefab.transform.rotation
                );
            }
        }

        spawnedObstacles++;
        Debug.Log("Spawned obstacle group: " + spawnedObstacles);
    }

    private System.Collections.IEnumerator ShowCongratulationsAfterDelay()
    {
        yield return new WaitForSeconds(congratulationsDelay);

        if (playerController != null && !playerController.gameOver && gameManager != null)
        {
            gameManager.Congratulations();
        }
    }

    public void ObstaclePassed()
    {

    }
}