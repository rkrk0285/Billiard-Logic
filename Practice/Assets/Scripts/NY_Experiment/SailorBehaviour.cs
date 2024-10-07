using System.Collections;
using UnityEngine;

public class SailorBehaviour : MonoBehaviour
{
    public Transform oarPosition;  // Position where the sailor rows
    public Transform bedPosition;  // Position where the sailor sleeps
    public float movementSpeed = 2f; // Speed at which the sailor moves
    public float sleepDuration = 15f; // Time the sailor spends sleeping
    private bool isWorking = false;   // Flag to check if the sailor is rowing
    public bool isSleeping = false;   // Flag to check if the sailor is sleeping
    private Coroutine sailorRoutine;  // Reference to the coroutine controlling the sailor's behavior

    private void Start()
    {
        // Start the sailor's routine (rowing and sleeping)
        sailorRoutine = StartCoroutine(SailorRoutine());
    }

    private void Update()
    {
        // Check if the player presses "E" to wake up the sailor
        if (isSleeping && Input.GetKeyDown(KeyCode.E))
        {
            WakeUp();  // Call the WakeUp method when "E" is pressed
        }
    }

    private IEnumerator SailorRoutine()
    {
        while (true)
        {
            // Sailor rows for a random amount of time (5 to 10 seconds)
            isSleeping = false;
            yield return MoveToPosition(oarPosition.position);  // Move to the oar position
            float rowingTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(rowingTime);

            // Sailor goes to sleep for 15 seconds
            yield return MoveToPosition(bedPosition.position);  // Move to the bed position
            isSleeping = true;
            yield return new WaitForSeconds(sleepDuration);
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move the sailor to the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Method to wake up the sailor and restart the loop
    public void WakeUp()
    {
        if (isSleeping)
        {
            isSleeping = false;  // Mark the sailor as no longer sleeping
            StopCoroutine(sailorRoutine);  // Stop the current routine
            sailorRoutine = StartCoroutine(SailorRoutine());  // Restart the loop
        }
    }
}
