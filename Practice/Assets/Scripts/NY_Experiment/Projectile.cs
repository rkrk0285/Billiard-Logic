using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;            // Speed of the projectile
    public float lifespan = 3f;           // Time before the projectile destroys itself if it doesn't hit anything
    private Transform ship;               // Reference to the ship to calculate distance to the circle's rim

    private Vector2 direction;             // Direction in which the projectile will move

    // Method to set the projectile's direction when instantiated
    public void Initialize(Transform ship)
    {
        this.ship = ship;
        CalculateDirection();
    }

    private void CalculateDirection()
    {
        // Calculate the direction from the ship to the projectile's current position
        Vector2 toShip = (Vector2)transform.position - (Vector2)ship.position;

        // Normalize the direction vector and multiply it by the desired distance (circle radius)
        float circleRadius = 5f; // Change this to your desired radius
        direction = toShip.normalized * (circleRadius - toShip.magnitude);
    }

    private void Start()
    {
        // Start moving the projectile in the specified direction
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed; // Use the provided direction

        // Destroy the projectile after a set lifespan
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for collision with enemies
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy the enemy
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
