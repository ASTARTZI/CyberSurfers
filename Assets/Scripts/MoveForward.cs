using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Ταχύτητα κίνησης του αντικειμένου
    [SerializeField] private float speed = 5f;

    // Όριο πέρα από το οποίο το αντικείμενο καταστρέφεται
    [SerializeField] private float destroyX = -15f;

    void Update()
    {
        // Μετακίνηση προς τα αριστερά
        transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);

        // Διαγραφή του αντικειμένου όταν βγει εκτός ορίων
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}