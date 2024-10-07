using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile to instantiate
    public Transform firePoint;          // The point from where the projectile will be fired
    public float fireRate = 1f;          // Time between each shot
    public Transform ship;               // Reference to the ship

    private float nextFireTime = 0f;     // Track when the next shot can be fired

    void Update()
    {
        // Check for firing input (for example, space bar) and if it's time to fire again
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate; // Set the next fire time
        }
    }

    void Fire()
    {
        // Instantiate the projectile at the fire point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Pass the ship reference to the projectile
        projectile.GetComponent<Projectile>().Initialize(ship);
    }
}
