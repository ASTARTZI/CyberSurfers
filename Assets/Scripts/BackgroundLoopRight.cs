using UnityEngine;

public class BackgroundLoopRight : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float width = 112.8f;

    private static float rightMostX;
    private Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        if (transform.position.x > rightMostX)
        {
            rightMostX = transform.position.x;
        }
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < player.position.x - width)
        {
            rightMostX += width;
            transform.position = new Vector3(rightMostX, transform.position.y, transform.position.z);
        }
    }
}