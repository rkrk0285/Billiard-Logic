using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float rotationSpeed = 100f;   // Speed of rotation (degrees per second)
    public Transform ship;               // Reference to the ship to rotate
    public Transform player;             // Reference to the player object

    private bool isInTrigger = false;    // Flag to track whether the player is in the trigger zone
    public bool isControllingShip = false; // Flag to track if the player is controlling the ship

    private Transform originalParent;    // Store the player's original parent (to restore hierarchy)
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        // Store the player's original parent (or null if it doesn't have a parent)
        originalParent = player.parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if the player is in the trigger zone and presses Space to toggle control
        if (isInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            isControllingShip = !isControllingShip;  // Toggle ship control mode

            if (isControllingShip)
            {
                // Make the player a child of the ship
                player.SetParent(ship);
            }
        }

        // If the player is controlling the ship, allow rotation
        if (isControllingShip)
        {
            // Rotate left when A is pressed
            if (Input.GetKey(KeyCode.A))
            {
                RotateLeft();
            }

            // Rotate right when D is pressed
            if (Input.GetKey(KeyCode.D))
            {
                RotateRight();
            }
        }
    }

    private void RotateLeft()
    {
        // Rotate the ship to the left (counterclockwise, negative z-axis rotation)
        ship.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void RotateRight()
    {
        // Rotate the ship to the right (clockwise, positive z-axis rotation)
        ship.transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the trigger collider, allow the possibility of controlling the ship
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            spriteRenderer.color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player exits the trigger collider, disable ship control mode and reset flags
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            isControllingShip = false; // Automatically exit ship control mode when leaving the trigger
            spriteRenderer.color = Color.red;
            // Restore the player to the original hierarchy (in case it was still parented to the ship)
            player.SetParent(null);
        }
    }
}