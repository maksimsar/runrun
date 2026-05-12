using System.Collections;
using UnityEngine;

public sealed class ObstacleSpawner : MonoBehaviour
{
    private float[] spawnIntervals = { 1.5f, 2.5f };

    private const float minAllowedInterval = 0.9f;
    private const float maxAllowedInterval = 1.6f;

    private bool spawnObstacles = true;
    private Coroutine spawnRoutine;

    private GameObject[] groundObstacles;
    private GameObject flyingObstacle;

    private Transform flyingSpawnPoint;
    private Transform groundSpawnPoint;

    public void RestartSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        spawnObstacles = true;
        spawnRoutine = StartCoroutine(SpawnObstacles());
    }

    public void ScaleSpawner()
    {
        const float decreaseValue = 0.075f;

        spawnIntervals[0] = Mathf.Max(minAllowedInterval, spawnIntervals[0] - decreaseValue);
        spawnIntervals[1] = Mathf.Max(maxAllowedInterval, spawnIntervals[1] - decreaseValue);

        if (spawnIntervals[1] <= spawnIntervals[0])
        {
            spawnIntervals[1] = spawnIntervals[0] + 0.25f;
        }
    }

    private void Awake()
    {
        groundObstacles = new GameObject[3] {
            Resources.Load("Prefabs/Obstacle [Spike]") as GameObject,
            Resources.Load("Prefabs/Obstacle [Spider]") as GameObject,
            Resources.Load("Prefabs/Obstacle [Fighter]") as GameObject
        };

        flyingObstacle = Resources.Load("Prefabs/Obstacle [Bat]") as GameObject;

        groundSpawnPoint = GameObject.Find("Ground Enemy Point").transform;
        flyingSpawnPoint = GameObject.Find("Flying Enemy Point").transform;
    }

    private void Start()
    {
        spawnRoutine = StartCoroutine(SpawnObstacles());
    }

    private float GetRandomSpawnInterval()
    {
        return Random.Range(spawnIntervals[0], spawnIntervals[1]);
    }

    private GameObject GetRandomObstacle()
    {
        int chance = Random.Range(1, 3);

        if (chance == 1)
        {
            return flyingObstacle;
        }

        return groundObstacles[Random.Range(0, groundObstacles.Length)];
    }

    private IEnumerator SpawnObstacles()
    {
        const float spawnDelay = 3.5f;
        yield return new WaitForSeconds(spawnDelay);

        while (spawnObstacles)
        {
            float spawnInterval = GetRandomSpawnInterval();

            GameObject obstacle = GetRandomObstacle();

            Transform spawnPoint = obstacle == flyingObstacle
                ? flyingSpawnPoint
                : groundSpawnPoint;

            Instantiate(obstacle, spawnPoint, false);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}