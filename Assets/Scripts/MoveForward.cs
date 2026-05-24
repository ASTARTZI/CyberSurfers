using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float destroyX = -15f;

    void Update()
    {
        transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}