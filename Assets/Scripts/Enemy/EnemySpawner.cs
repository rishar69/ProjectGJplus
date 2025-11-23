using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner2D : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Enemy Prefabs (isi 3 prefab di sini)")]
    public List<GameObject> enemies;

    [Header("Spawn Points (titik tetap di map)")]
    public List<Transform> spawnPoints;   // isi di Inspector

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;          // jeda awal antar wave
    public float minDistanceFromPlayer = 4f;  // minimal jarak spawn point ke player
    public int baseSpawnCount = 2;            // jumlah musuh awal per wave

    [Header("Difficulty Scaling")]
    public int dangerLevel = 1;
    public float difficultyIncreaseInterval = 10f; // naik level tiap 10 detik
    public float spawnIntervalMin = 0.8f;          // batas bawah interval

    private bool isSpawning = true;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("EnemySpawner2D: Player belum di-assign!");
            isSpawning = false;
            return;
        }

        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogError("EnemySpawner2D: List enemies kosong!");
            isSpawning = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("EnemySpawner2D: SpawnPoints kosong! Tambahkan titik spawn di Inspector.");
            isSpawning = false;
            return;
        }

        // Mulai 2 coroutine terpisah: spawn & difficulty
        StartCoroutine(SpawnLoop());
        StartCoroutine(DifficultyLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            SpawnWave();
            // tunggu sesuai spawnInterval (yang bisa berubah seiring waktu)
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator DifficultyLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);

            dangerLevel++;

            // makin tinggi level, jeda antar wave makin pendek
            spawnInterval = Mathf.Max(spawnIntervalMin, spawnInterval - 0.1f);

            Debug.Log($"[Spawner] Danger Level: {dangerLevel}, SpawnInterval: {spawnInterval}");
        }
    }

    void SpawnWave()
    {
        // hitung jumlah musuh per wave
        int spawnCount = Mathf.RoundToInt(baseSpawnCount + dangerLevel * 0.7f);
        if (spawnCount < 1) spawnCount = 1;

        // kumpulkan spawn point yang cukup jauh dari player
        List<Transform> validPoints = GetValidSpawnPoints();

        if (validPoints.Count == 0)
        {
            // kalau semua terlalu dekat, pakai semua spawn point sebagai fallback
            validPoints = spawnPoints;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = validPoints[Random.Range(0, validPoints.Count)];
            GameObject enemyPrefab = GetRandomEnemy();
            if (enemyPrefab == null) continue;

            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    List<Transform> GetValidSpawnPoints()
    {
        List<Transform> result = new List<Transform>();

        foreach (var sp in spawnPoints)
        {
            if (sp == null) continue;

            float dist = Vector2.Distance(player.position, sp.position);
            if (dist >= minDistanceFromPlayer)
            {
                result.Add(sp);
            }
        }

        return result;
    }

    GameObject GetRandomEnemy()
    {
        if (enemies == null || enemies.Count == 0) return null;
        int index = Random.Range(0, enemies.Count);
        return enemies[index];
    }
}
