using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    // Διακοσμητικά αντικείμενα που μπορούν να εμφανιστούν
    [SerializeField] private GameObject[] decorPrefabs;

    private void Start()
    {
        // Δημιουργία διακοσμητικών ανά τακτά χρονικά διαστήματα
        InvokeRepeating(nameof(SpawnDecor), 1f, 2f);
    }

    private void SpawnDecor()
    {
        // Έλεγχος ότι υπάρχουν διαθέσιμα prefabs
        if (decorPrefabs.Length == 0) return;

        // Επιλογή τυχαίου διακοσμητικού
        int randomDecor = Random.Range(0, decorPrefabs.Length);

        // Τυχαία επιλογή αριστερής ή δεξιάς πλευράς
        float side = Random.value > 0.5f ? 12f : -12f;

        Vector3 pos = new Vector3(
            15f,
            0f,
            side
        );

        // Δημιουργία του διακοσμητικού στη σκηνή
        Instantiate(
            decorPrefabs[randomDecor],
            pos,
            decorPrefabs[randomDecor].transform.rotation
        );
    }
}