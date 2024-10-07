using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;              // Player movement speed
    public ShipController shipController;     // Reference to the ShipController script

    private Vector2 movement;

    void Update()
    {
        // Only allow movement if the player is not controlling the ship
        if (!shipController.isControllingShip)
        {
            // Get input from WASD and arrow keys
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the player when not controlling the ship
        if (!shipController.isControllingShip)
        {
            transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}