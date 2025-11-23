using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner2D : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Enemy Prefabs (isi 3 prefab di sini)")]
    public List<GameObject> enemies;   
    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public float minSpawnDistance = 6f;
    public float maxSpawnDistance = 14f;

    [Header("Difficulty Scaling")]
    public int dangerLevel = 1;
    public float difficultyIncreaseInterval = 10f;

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;

    void Update()
    {
        if (player == null) return; // jaga-jaga

        spawnTimer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        // Spawn wave
        if (spawnTimer >= spawnInterval)
        {
            SpawnWave();
            spawnTimer = 0f;
        }

        // Naikkan difficulty tiap beberapa detik
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            dangerLevel++;
            difficultyTimer = 0f;

            // spawn makin sering
            spawnInterval = Mathf.Max(0.8f, spawnInterval - 0.1f);
            Debug.Log("Danger Level: " + dangerLevel + " | SpawnInterval: " + spawnInterval);
        }
    }

    void SpawnWave()
    {
        
        int spawnCount = Mathf.RoundToInt(1 + dangerLevel * 0.7f);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            GameObject enemyPrefab = GetRandomEnemy(); 
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        // Untuk 2D top-down
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        return (Vector2)player.position + randomDir * distance;

    }

    GameObject GetRandomEnemy()
    {
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogWarning("EnemySpawner2D: List musuh kosong!");
            return null;
        }

        // random dari 3 prefab
        int index = Random.Range(0, enemies.Count);
        return enemies[index];
    }
}
