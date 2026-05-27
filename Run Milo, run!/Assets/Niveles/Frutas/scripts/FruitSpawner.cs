using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs;

    public float minSpawnPositionY = -2f;
    public float maxSpawnPositionY = 2f;

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;

    private void Start()
    {
        ScheduleNextSpawn();
    }

    private void SpawnFruit()
    {
        GameObject selectedFruit = GetRandomFruit();

        if (selectedFruit == null)
        {
            Debug.LogWarning("No hay frutas asignadas en Fruit Prefabs.");
            ScheduleNextSpawn();
            return;
        }

        float randomY = Random.Range(minSpawnPositionY, maxSpawnPositionY);

        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            randomY,
            transform.position.z
        );

        Instantiate(selectedFruit, spawnPosition, Quaternion.identity);

        ScheduleNextSpawn();
    }

    private GameObject GetRandomFruit()
    {
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, fruitPrefabs.Length);

        return fruitPrefabs[randomIndex];
    }

    private void ScheduleNextSpawn()
    {
        float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);

        Invoke(nameof(SpawnFruit), randomDelay);
    }
}