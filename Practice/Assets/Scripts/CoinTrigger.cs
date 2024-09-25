using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private CoinSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<CoinSpawner>(); // Find the spawner script in the scene
    }

    private void OnTriggerEnter2D(Collider2D other) // Assuming a 2D game, use OnTriggerEnter for 3D
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the player
        {
            spawner.MoveCoin(); // Move the coin to a new position
        }
    }
}