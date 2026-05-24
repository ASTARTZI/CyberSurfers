using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float width = 112.8f;

    private float startX;
    private PlayerController player;

    void Start()
    {
        startX = transform.position.x;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player != null && !player.gameOver)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (transform.position.x <= startX - width)
        {
            transform.position += Vector3.right * width * 3f;
        }
    }
}