using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner2D : MonoBehaviour
{
  [Header("Player")]
    public Transform player;

    [Header("Enemy Prefabs (isi 3 prefab di sini)")]
    public List<GameObject> enemies;

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;        // jeda antar wave
    public float minDistanceFromPlayer = 2f; // jarak aman dari player
    public float edgeMargin = 0.5f;         // biar nggak nempel di pinggir layar
    public int baseSpawnCount = 2;          // jumlah musuh awal per wave

    [Header("Difficulty Scaling")]
    public int dangerLevel = 1;
    public float difficultyIncreaseInterval = 10f;
    public float spawnIntervalMin = 0.8f;   // batas minimal interval

    private bool isRunning = true;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("EnemySpawner2D: Player belum di-assign!");
            isRunning = false;
            return;
        }

        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogError("EnemySpawner2D: List enemies kosong!");
            isRunning = false;
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError("EnemySpawner2D: mainCamera belum di-assign dan tidak menemukan Camera.main!");
            isRunning = false;
            return;
        }

        // mulai loop spawn & difficulty
        StartCoroutine(SpawnLoop());
        StartCoroutine(DifficultyLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (isRunning)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator DifficultyLoop()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);

            dangerLevel++;
            spawnInterval = Mathf.Max(spawnIntervalMin, spawnInterval - 0.1f);

            Debug.Log($"[Spawner] Danger Level: {dangerLevel}, SpawnInterval: {spawnInterval}");
        }
    }

    private void SpawnWave()
    {
        int spawnCount = Mathf.RoundToInt(baseSpawnCount + dangerLevel * 0.7f);
        if (spawnCount < 1) spawnCount = 1;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPos = GetSpawnPositionInCamera();
            GameObject enemyPrefab = GetRandomEnemy();
            if (enemyPrefab == null) continue;

            // pastikan Z = 0 (top-down 2D)
            Vector3 finalPos = new Vector3(spawnPos.x, spawnPos.y, 0f);
            Instantiate(enemyPrefab, finalPos, Quaternion.identity);
        }
    }

    private Vector2 GetSpawnPositionInCamera()
    {
        // Info kamera ortho
        Vector3 camPos = mainCamera.transform.position;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        // Coba cari posisi yang:
        // - di dalam area kamera
        // - cukup jauh dari player
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(camPos.x - halfWidth + edgeMargin,
                                   camPos.x + halfWidth - edgeMargin);
            float y = Random.Range(camPos.y - halfHeight + edgeMargin,
                                   camPos.y + halfHeight - edgeMargin);

            Vector2 candidate = new Vector2(x, y);

            if (Vector2.Distance(candidate, player.position) >= minDistanceFromPlayer)
            {
                return candidate;
            }
        }

        // Fallback: kalau 10x gagal, spawn di kanan player lalu di-clamp ke dalam kamera
        Vector2 fallback = (Vector2)player.position + Vector2.right * minDistanceFromPlayer;

        float clampedX = Mathf.Clamp(fallback.x,
                                     camPos.x - halfWidth + edgeMargin,
                                     camPos.x + halfWidth - edgeMargin);
        float clampedY = Mathf.Clamp(fallback.y,
                                     camPos.y - halfHeight + edgeMargin,
                                     camPos.y + halfHeight - edgeMargin);

        return new Vector2(clampedX, clampedY);
    }

    private GameObject GetRandomEnemy()
    {
        if (enemies == null || enemies.Count == 0) return null;
        int index = Random.Range(0, enemies.Count);
        return enemies[index];
    }
}