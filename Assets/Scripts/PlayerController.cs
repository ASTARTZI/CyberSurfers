using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Βασικά components του παίκτη
    private Rigidbody playerRb;
    private BoxCollider playerCollider;
    private GameManager gameManager;
    private Animator playerAnim;
    private AudioSource playerAudio;

    // Ρυθμίσεις άλματος και σκυψίματος
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravityModifier = 2f;
    [SerializeField] private float duckDuration = 1.6f;

    // Ρυθμίσεις αλλαγής λωρίδας
    [SerializeField] private float laneDistance = 4f;
    [SerializeField] private float laneChangeSpeed = 10f;

    // Ήχοι του παίκτη
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip duckSound;

    // Εφέ σύγκρουσης με εμπόδιο
    [SerializeField] private ParticleSystem explosionParticle;

    private bool isOnGround = true;
    private bool isDucking = false;
    public bool gameOver = false;

    private int currentLane = 1; // 0 = αριστερά, 1 = κέντρο, 2 = δεξιά

    // Κανονικές τιμές collider και μεγέθους
    private Vector3 normalColliderCenter;
    private Vector3 normalColliderSize;

    // Τιμές collider όταν ο παίκτης σκύβει
    private Vector3 duckColliderCenter;
    private Vector3 duckColliderSize;

    private Vector3 normalScale;
    private Vector3 duckScale;

    private void Start()
    {
        // Ανάκτηση των components που χρειάζεται ο παίκτης
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = FindFirstObjectByType<GameManager>();

        // Ρύθμιση βαρύτητας για πιο έντονη πτώση
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

        normalColliderCenter = playerCollider.center;
        normalColliderSize = playerCollider.size;

        // Μικρότερος collider για την κίνηση duck
        duckColliderCenter = new Vector3(
            normalColliderCenter.x,
            normalColliderCenter.y * 0.5f,
            normalColliderCenter.z
        );

        duckColliderSize = new Vector3(
            normalColliderSize.x,
            normalColliderSize.y * 0.5f,
            normalColliderSize.z
        );

        normalScale = transform.localScale;
        duckScale = new Vector3(normalScale.x, normalScale.y * 0.5f, normalScale.z);

        // Εκκίνηση animation τρεξίματος
        if (playerAnim != null)
        {
            playerAnim.SetFloat("Speed_f", 1f);
        }
    }

    private void Update()
    {
        // Επιτρέπεται αλλαγή λωρίδας μόνο όσο παίζεται το παιχνίδι
        if (!gameOver)
        {
            HandleLaneMovement();
        }

        // Άλμα με πάνω βέλος ή Space
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isOnGround && !gameOver && !isDucking)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

            if (playerAnim != null)
            {
                playerAnim.SetTrigger("Jump_trig");
            }

            if (playerAudio != null && jumpSound != null)
            {
                playerAudio.PlayOneShot(jumpSound, 1f);
            }
        }

        // Σκύψιμο με κάτω βέλος
        if (Input.GetKeyDown(KeyCode.DownArrow) && isOnGround && !gameOver && !isDucking)
        {
            StartCoroutine(Duck());
        }
    }

    private void HandleLaneMovement()
    {
        // Αλλαγή προς αριστερή λωρίδα
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
        }

        // Αλλαγή προς δεξιά λωρίδα
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
        }

        Vector3 targetPosition = transform.position;
        targetPosition.z = (1 - currentLane) * laneDistance;

        // Ομαλή μετακίνηση προς τη νέα λωρίδα
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            laneChangeSpeed * Time.deltaTime
        );
    }

    private System.Collections.IEnumerator Duck()
    {
        isDucking = true;

        if (playerAudio != null && duckSound != null)
        {
            playerAudio.PlayOneShot(duckSound, 0.8f);
        }

        // Μείωση collider και κλίμακας όσο ο παίκτης σκύβει
        playerCollider.center = duckColliderCenter;
        playerCollider.size = duckColliderSize;
        transform.localScale = duckScale;

        yield return new WaitForSeconds(duckDuration);

        // Επαναφορά κανονικού collider και μεγέθους
        transform.localScale = normalScale;
        playerCollider.center = normalColliderCenter;
        playerCollider.size = normalColliderSize;

        isDucking = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        // Έλεγχος επαφής με το έδαφος
        if (other.gameObject.name == "Ground")
        {
            isOnGround = true;
        }

        // Σύγκρουση με εμπόδιο και τερματισμός παιχνιδιού
        if (other.gameObject.CompareTag("Obstacle") && !gameOver)
        {
            gameOver = true;

            if (playerAudio != null && crashSound != null)
            {
                playerAudio.PlayOneShot(crashSound, 1f);
            }

            if (playerAnim != null)
            {
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
            }

            if (explosionParticle != null)
            {
                explosionParticle.Play();
            }

            StartCoroutine(ShowGameOverAfterDelay());
        }
    }

    private System.Collections.IEnumerator ShowGameOverAfterDelay()
    {
        // Μικρή καθυστέρηση ώστε να φανεί πρώτα το animation θανάτου
        yield return new WaitForSeconds(0.5f);
        gameManager.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Συλλογή reward και ενημέρωση του σκορ
        if (other.gameObject.CompareTag("Reward"))
        {
            gameManager.AddReward();
            Destroy(other.gameObject);
        }
    }
}