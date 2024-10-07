using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // The enemy prefab to instantiate
    public Transform ship;            // The player's ship
    public float spawnRadius = 10f;   // Radius of the circle around the ship to spawn enemies
    public float moveSpeed = 2f;      // Speed at which enemies move toward the ship
    public float spawnInterval = 2f;  // Time interval between spawns

    private void Start()
    {
        // Start spawning enemies repeatedly
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Spawn an enemy at a random point on the rim of the circle
            SpawnEnemyAtCircle();

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemyAtCircle()
    {
        // Generate a random angle in radians (0 to 2¥ð)
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // Calculate the spawn position using the angle and the circle's radius
        Vector2 spawnPosition = new Vector2(
            Mathf.Cos(randomAngle) * spawnRadius,
            Mathf.Sin(randomAngle) * spawnRadius
        );

        // Instantiate the enemy prefab at the calculated position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Pass the reference of the ship to the enemy so it can move towards it
        enemy.GetComponent<EnemyBehaviour>().Initialize(ship, moveSpeed);
    }
}