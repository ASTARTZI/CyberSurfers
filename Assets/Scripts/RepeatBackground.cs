using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private float repeatWidth;

    void Start()
    {
        repeatWidth = GetComponent<BoxCollider>().size.x;
    }

    void Update()
    {
        if (transform.position.x < -repeatWidth)
        {
            transform.position += new Vector3(repeatWidth * 2, 0, 0);
        }
    }
}