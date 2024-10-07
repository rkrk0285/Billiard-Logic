using UnityEngine;

public class RoomCameraSwitch : MonoBehaviour
{
    public Camera mainCamera;     // Reference to the main camera
    private bool isInTrigger = false;  // To track if the player is inside the trigger area

    public enum E_RoomType
    {
        up, down, left, right
    }

    public E_RoomType currentRoomType; // The current room type for this trigger

    void Update()
    {
        // Check if the player is in the trigger zone and presses space
        if (isInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            if (mainCamera != null)
            {
                Vector3 newCameraPosition = mainCamera.transform.position;

                // Move the camera based on the room type
                if (currentRoomType == E_RoomType.left)
                {
                    newCameraPosition.x += 20f; // Move camera +20 on the x-axis
                    Debug.Log("Camera moved +20 along x-axis for left room");
                    
                }
                else if (currentRoomType == E_RoomType.right)
                {
                    newCameraPosition.x += 40f; // Move camera +40 on the x-axis
                    Debug.Log("Camera moved +40 along x-axis for right room");
                }

                // Apply the new camera position
                mainCamera.transform.position = newCameraPosition;
            }
            else
            {
                Debug.LogError("Main camera not assigned!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the trigger, allow camera switch
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("Player entered trigger zone.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player exits the trigger, reset the trigger flag
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            mainCamera.transform.position = new Vector3(0,0,-10);
        }
    }
}
