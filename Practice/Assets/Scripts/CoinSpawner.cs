using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Coin prefab to spawn
    public Vector2 fieldMin; // Bottom-left corner of the field
    public Vector2 fieldMax; // Top-right corner of the field

    private GameObject currentCoin; // Reference to the currently spawned coin

    void Start()
    {
        SpawnCoin(); // Spawn the first coin when the game starts
    }

    // Function to spawn a coin at a random position within the field
    void SpawnCoin()
    {
        Vector3 randomPosition = GetRandomPositionWithinField();
        currentCoin = Instantiate(coinPrefab, randomPosition, Quaternion.identity);
    }

    // Function to get a random position within the rectangular field
    Vector3 GetRandomPositionWithinField()
    {
        float randomX = Random.Range(fieldMin.x, fieldMax.x);
        float randomY = Random.Range(fieldMin.y, fieldMax.y);
        return new Vector3(randomX, randomY, 0); // Assuming a 2D field, set z to 0
    }

    // This function will be called when a player collides with the coin
    public void MoveCoin()
    {
        currentCoin.transform.position = GetRandomPositionWithinField();
    }
}