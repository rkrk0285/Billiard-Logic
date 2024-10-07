using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform ship;          // The player's ship
    private float moveSpeed;         // Speed at which this enemy moves toward the ship

    // This method will be called when the enemy is instantiated to initialize its movement
    public void Initialize(Transform ship, float moveSpeed)
    {
        this.ship = ship;            // Store the reference to the ship's transform
        this.moveSpeed = moveSpeed;  // Set the enemy's movement speed
    }

    private void Update()
    {
        // Move the enemy towards the ship
        if (ship != null)
        {
            // Calculate the direction towards the ship
            Vector2 direction = (ship.position - transform.position).normalized;

            // Move the enemy towards the ship using MoveTowards
            transform.position = Vector2.MoveTowards(transform.position, ship.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the enemy collides with the ship
        if (collision.CompareTag("Ship"))
        {
            // Destroy the enemy on collision
            Destroy(gameObject);
        }
    }
}