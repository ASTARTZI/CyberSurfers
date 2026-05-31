using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] decorPrefabs;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnDecor), 1f, 2f);
    }

    private void SpawnDecor()
    {
        if (decorPrefabs.Length == 0) return;

        int randomDecor = Random.Range(0, decorPrefabs.Length);

        float side = Random.value > 0.5f ? 12f : -12f;

        Vector3 pos = new Vector3(
            15f,
            0f,
            side
        );

        Instantiate(
            decorPrefabs[randomDecor],
            pos,
            decorPrefabs[randomDecor].transform.rotation
        );
    }
}