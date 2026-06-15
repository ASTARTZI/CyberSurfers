using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    // Backgrounds αριστερής και δεξιάς πλευράς
    [SerializeField] private Transform[] leftBackgrounds;
    [SerializeField] private Transform[] rightBackgrounds;

    // Ταχύτητα κίνησης των backgrounds
    [SerializeField] private float speed = 5f;

    // Μικρή επικάλυψη ώστε να μην εμφανίζονται κενά
    [SerializeField] private float overlap = 0.25f;

    private PlayerController playerController;

    private void Start()
    {
        // Εύρεση του PlayerController
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Αρχική τοποθέτηση των backgrounds στη σωστή σειρά
        Arrange(leftBackgrounds);
        Arrange(rightBackgrounds);
    }

    private void Update()
    {
        // Σταματά η κίνηση όταν τελειώσει το παιχνίδι
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

        // Τοποθέτηση των backgrounds το ένα δίπλα στο άλλο
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

        // Μετακίνηση όλων των backgrounds προς τα αριστερά
        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * speed * Time.deltaTime;
        }

        // Επανατοποθέτηση background που βγήκε εκτός οθόνης
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

        // Εύρεση του δεξιότερου background
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }

        return maxX;
    }

    private float GetWidth(Transform bg)
    {
        // Επιστρέφει το πραγματικό πλάτος του sprite
        return bg.GetComponent<SpriteRenderer>().bounds.size.x;
    }
}