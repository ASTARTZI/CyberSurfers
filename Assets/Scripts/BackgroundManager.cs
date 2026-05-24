using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform[] leftBackgrounds;
    [SerializeField] private Transform[] rightBackgrounds;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float overlap = 0.25f;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        Arrange(leftBackgrounds);
        Arrange(rightBackgrounds);
    }

    private void Update()
    {
        if (playerController != null && playerController.gameOver)
            return;

        MoveAndLoop(leftBackgrounds);
        MoveAndLoop(rightBackgrounds);
    }

    private void Arrange(Transform[] backgrounds)
    {
        if (backgrounds.Length == 0) return;

        float width = GetWidth(backgrounds[0]) - overlap;
        float startX = backgrounds[0].position.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position = new Vector3(
                startX + i * width,
                backgrounds[i].position.y,
                backgrounds[i].position.z
            );
        }
    }

    private void MoveAndLoop(Transform[] backgrounds)
    {
        if (backgrounds.Length == 0) return;

        float width = GetWidth(backgrounds[0]) - overlap;

        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * speed * Time.deltaTime;
        }

        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x < -80f)
            {
                float rightMostX = GetRightMostX(backgrounds);
                bg.position = new Vector3(
                    rightMostX + width,
                    bg.position.y,
                    bg.position.z
                );
            }
        }
    }

    private float GetRightMostX(Transform[] backgrounds)
    {
        float maxX = backgrounds[0].position.x;

        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }

        return maxX;
    }

    private float GetWidth(Transform bg)
    {
        return bg.GetComponent<SpriteRenderer>().bounds.size.x;
    }
}