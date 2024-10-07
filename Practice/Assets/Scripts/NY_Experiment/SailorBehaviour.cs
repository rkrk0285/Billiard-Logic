using System.Collections;
using UnityEngine;

public class SailorBehavior : MonoBehaviour
{
    public Transform oarPosition;  // Position where the sailor rows
    public Transform bedPosition;  // Position where the sailor sleeps
    public float movementSpeed = 2f; // Speed at which the sailor moves
    public float sleepDuration = 15f; // Time the sailor spends sleeping
    private bool isWorking = true;  // To track whether the sailor is rowing or sleeping
    private bool isAwake = true;    // To track whether the sailor is awake
    private Vector3 targetPosition;  // Target position for movement

    void Start()
    {
        // Start the sailor's behavior loop
        StartCoroutine(SailorLoop());
    }

    void Update()
    {
        // Move the sailor toward the target position
        MoveToTarget();

        // Check if the player presses "E" to wake the sailor up
        if (Input.GetKeyDown(KeyCode.E) && !isAwake)
        {
            Debug.Log("Sailor woke up!");
            isAwake = true;
            StopCoroutine("Sleep");
            StartCoroutine(SailorLoop()); // Resume the work loop
        }
    }

    IEnumerator SailorLoop()
    {
        while (true)
        {
            // Sailor rows for a random amount of time
            yield return RowBoat();

            // Sailor goes to bed to sleep for a fixed duration
            yield return Sleep();
        }
    }

    IEnumerator RowBoat()
    {
        Debug.Log("Sailor is rowing the boat...");

        // Move to the oar position for rowing
        targetPosition = oarPosition.position;

        // Wait until the sailor reaches the oar position
        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) < 0.1f);

        // Sailor rows for a random time between 5 and 10 seconds
        float rowTime = Random.Range(5f, 10f);
        yield return new WaitForSeconds(rowTime);
    }

    IEnumerator Sleep()
    {
        Debug.Log("Sailor is going to bed...");

        // Move to the bed position for sleeping
        targetPosition = bedPosition.position;

        // Wait until the sailor reaches the bed position
        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) < 0.1f);

        Debug.Log("Sailor is sleeping...");
        isAwake = false;

        // Sailor sleeps for the specified duration (15 seconds)
        yield return new WaitForSeconds(sleepDuration);
    }

    private void MoveToTarget()
    {
        // Move the sailor towards the target position
        if (targetPosition != null && isAwake)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }
}
