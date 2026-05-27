using UnityEngine;

[System.Serializable]
public class ObstacleSpawnData
{
    public GameObject obstaclePrefab;
    public float spawnPositionY;
}

public class ObstacleSpawner : MonoBehaviour
{
    public ObstacleSpawnData[] obstacles;

    public float spawnDelay = 2f;

    public bool useRandomDelay = false;
    public float minDelay = 1f;
    public float maxDelay = 3f;

    private void Start()
    {
        ScheduleNextSpawn();
    }

    private void SpawnObstacle()
    {
        ObstacleSpawnData selectedObstacleData = GetRandomObstacleData();

        if (selectedObstacleData == null || selectedObstacleData.obstaclePrefab == null)
        {
            Debug.LogWarning("No hay obstáculo asignado en el Spawner.");
            ScheduleNextSpawn();
            return;
        }

        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            selectedObstacleData.spawnPositionY,
            transform.position.z
        );

        Instantiate(
            selectedObstacleData.obstaclePrefab,
            spawnPosition,
            Quaternion.identity
        );

        ScheduleNextSpawn();
    }

    private ObstacleSpawnData GetRandomObstacleData()
    {
        if (obstacles == null || obstacles.Length == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, obstacles.Length);

        return obstacles[randomIndex];
    }

    private void ScheduleNextSpawn()
    {
        float delay = spawnDelay;

        if (useRandomDelay)
        {
            delay = Random.Range(minDelay, maxDelay);
        }

        Invoke(nameof(SpawnObstacle), delay);
    }
}