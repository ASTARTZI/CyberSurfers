using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Prefabs για εμπόδια εδάφους, αέρα και reward
    [SerializeField] private GameObject[] groundObstaclePrefabs;
    [SerializeField] private GameObject[] airObstaclePrefabs;
    [SerializeField] private GameObject rewardPrefab;

    // Ρυθμίσεις δημιουργίας εμποδίων
    [SerializeField] private int maxObstacles = 20;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float repeatRate = 3f;

    // Θέσεις εμφάνισης εμποδίων
    [SerializeField] private Vector3 groundSpawnPosition = new Vector3(12f, 0f, 0f);
    [SerializeField] private Vector3 airSpawnPosition = new Vector3(12f, 1.4f, 0f);

    // Θέσεις reward σε σχέση με το εμπόδιο
    [SerializeField] private Vector3 groundRewardOffset = new Vector3(-5f, 1.2f, 0f);
    [SerializeField] private Vector3 airRewardOffset = new Vector3(-7f, 0.8f, 0f);

    [SerializeField] private bool randomObstacleOrder = true;

    // Θέσεις των τριών λωρίδων στον άξονα Ζ
    [SerializeField] private float[] lanePositions = { -4f, 0f, 4f };

    // Καθυστέρηση πριν εμφανιστεί το Congratulations
    [SerializeField] private float congratulationsDelay = 7f;

    private int spawnedObstacles = 0;
    private bool congratulationsStarted = false;

    private PlayerController playerController;
    private GameManager gameManager;

    private void Start()
    {
        // Εύρεση των βασικών scripts του παιχνιδιού
        playerController = FindFirstObjectByType<PlayerController>();
        gameManager = FindFirstObjectByType<GameManager>();

        // Επαναλαμβανόμενη δημιουργία ομάδων εμποδίων και rewards
        InvokeRepeating(nameof(SpawnObstacleAndReward), startDelay, repeatRate);
    }

    private void SpawnObstacleAndReward()
    {
        // Σταματά το spawning αν ο παίκτης έχει χάσει
        if (playerController != null && playerController.gameOver)
        {
            CancelInvoke(nameof(SpawnObstacleAndReward));
            return;
        }

        // Όταν δημιουργηθούν όλα τα εμπόδια, ξεκινά η διαδικασία νίκης
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

        // Τυχαία λωρίδα στην οποία θα εμφανιστεί reward
        int rewardLane = Random.Range(0, lanePositions.Length);

        for (int lane = 0; lane < lanePositions.Length; lane++)
        {
            // Επιλογή αν το εμπόδιο θα είναι στον αέρα ή στο έδαφος
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

            // Επιλογή τυχαίου prefab από τη σωστή κατηγορία εμποδίων
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

            // Δημιουργία του επιλεγμένου εμποδίου
            if (selectedObstacle != null)
            {
                Instantiate(
                    selectedObstacle,
                    spawnPosition,
                    selectedObstacle.transform.rotation
                );
            }

            // Δημιουργία reward μόνο σε μία από τις λωρίδες
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
        // Αναμονή ώστε να περάσουν και τα τελευταία εμπόδια
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