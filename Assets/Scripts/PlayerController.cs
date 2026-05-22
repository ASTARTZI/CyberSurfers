using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private BoxCollider playerCollider;
    private GameManager gameManager;
    private Animator playerAnim;
    private AudioSource playerAudio;

    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravityModifier = 2f;
    [SerializeField] private float duckDuration = 1.4f;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip duckSound;

    private bool isOnGround = true;
    private bool isDucking = false;
    public bool gameOver = false;

    private Vector3 normalColliderCenter;
    private Vector3 normalColliderSize;

    private Vector3 duckColliderCenter;
    private Vector3 duckColliderSize;

    private Vector3 normalScale;
    private Vector3 duckScale;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = FindFirstObjectByType<GameManager>();

        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

        normalColliderCenter = playerCollider.center;
        normalColliderSize = playerCollider.size;

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

        if (playerAnim != null)
        {
            playerAnim.SetFloat("Speed_f", 1f);
        }
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.DownArrow) && isOnGround && !gameOver && !isDucking)
        {
            StartCoroutine(Duck());
        }
    }

    private System.Collections.IEnumerator Duck()
    {
        isDucking = true;

        if (playerAudio != null && duckSound != null)
        {
            playerAudio.PlayOneShot(duckSound, 0.8f);
        }

        playerCollider.center = duckColliderCenter;
        playerCollider.size = duckColliderSize;
        transform.localScale = duckScale;

        yield return new WaitForSeconds(duckDuration);

        transform.localScale = normalScale;
        playerCollider.center = normalColliderCenter;
        playerCollider.size = normalColliderSize;

        isDucking = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Ground")
        {
            isOnGround = true;
        }

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

            gameManager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reward"))
        {
            gameManager.AddReward();
            Destroy(other.gameObject);
        }
    }
}