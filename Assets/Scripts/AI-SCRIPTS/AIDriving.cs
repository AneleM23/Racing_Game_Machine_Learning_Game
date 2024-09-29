using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriving : MonoBehaviour
{
    // This script controls the behavior of AI vehicles.
    // It includes pathfinding, speed adjustments, collision detection, evasive actions, and lane switching logic.

    private Transform[] currentLaneWaypoints;
    private int currentWaypointIndex = 0;

    private Colour_Gizmos colourGizmos;

    public LapSystem lapSystem;

    // Vehicle movement parameters
    public float speed;
    public float rotationSpeed = 50f;
    public float acceleration;
    public float maxSpeed;
    public float detectionRange = 5f;
    public LayerMask aiLayer;
    public LayerMask playerLayer;
    public float cornerDecelerationFactor = 0.3f;

    private Rigidbody rb;

    // Variables to simulate gear changes.
    private int currentGear = 1;
    private float gearChangeSpeedFactor = 0.8f;
    private float gearChangeDuration = 0.5f;

    // Store initial position and rotation
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        initialPosition = transform.position; // Store initial position
        initialRotation = transform.rotation; // Store initial rotation

        colourGizmos = GetComponent<Colour_Gizmos>();
        if (colourGizmos != null)
        {
            ChooseRandomLane();
        }
        else
        {
            Debug.LogError("Colour_Gizmos script not found on this GameObject!");
        }

        // This will randomly set the acceleration, maxSpeed, speed, and detection range for each AI vehicle.
        acceleration = Random.Range(8f, 12f);
        maxSpeed = Random.Range(100f, 150f);
        speed = Random.Range(5f, 10f);
        detectionRange = Random.Range(10f, 20f);

        lapSystem = FindObjectOfType<LapSystem>();
    }

    private void FixedUpdate()
    {
        // Check if the AI vehicle has completed all laps
        if (lapSystem.HasCompletedAllLaps())
        {
            // Reset the AI vehicle after completing all laps
            ResetToInitialPosition();
        }
        else
        {
            DriveTowardsNextWaypoint();
            DetectAndAvoidOtherVehicles();
        }
    }

    private void DriveTowardsNextWaypoint()
    {
        // Calculate the direction to the next waypoint and ignore Y-axis movement to keep the AI vehicle on the track.
        Vector3 direction = currentLaneWaypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0;

        // Rotate the vehicle towards the next waypoint.
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        // If the AI vehicle is not facing the waypoint properly, return.
        if (Vector3.Dot(transform.forward, direction.normalized) < 0.95f)
        {
            return;
        }

        // Move the AI vehicle in the direction of the waypoint.
        Vector3 moveDir = direction.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);

        // If the AI vehicle is close to the current waypoint, update the waypoint index.
        if (direction.magnitude < 2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= currentLaneWaypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        AdjustSpeedOnCorners();
        ManageOverallSpeed();
    }

    private void ChooseRandomLane()
    {
        // Choose a random lane for the AI vehicle.
        int randomLane = Random.Range(0, 3);

        switch (randomLane)
        {
            case 0:
                currentLaneWaypoints = colourGizmos.FarRightWaypoints;
                break;
            case 1:
                currentLaneWaypoints = colourGizmos.middleLaneWaypoints;
                break;
            case 2:
                currentLaneWaypoints = colourGizmos.FarLeftWaypoints;
                break;
        }
    }

    private void AdjustSpeedOnCorners()
    {
        if (currentWaypointIndex < currentLaneWaypoints.Length && currentLaneWaypoints[currentWaypointIndex].CompareTag("Corner"))
        {
            speed = Mathf.Max(speed - (acceleration * cornerDecelerationFactor * Time.fixedDeltaTime), 0.8f * maxSpeed);
        }
        else
        {
            speed = Mathf.Min(speed + (acceleration * Time.fixedDeltaTime), maxSpeed);
        }

        if (speed > maxSpeed * gearChangeSpeedFactor)
        {
            ChangeGear();
        }
    }

    private void ManageOverallSpeed()
    {
        if (speed > 20f)
        {
            speed = Mathf.Max(speed - (acceleration * 0.1f * Time.fixedDeltaTime), 20f);
        }
    }

    private void ChangeGear()
    {
        // Reduce speed when changing gears and start the gear change coroutine.
        speed *= gearChangeSpeedFactor;
        StartCoroutine(GearChangeCoroutine());
    }

    private IEnumerator GearChangeCoroutine()
    {
        yield return new WaitForSeconds(gearChangeDuration);
        speed = Mathf.Min(speed + (acceleration * 2f * Time.fixedDeltaTime), maxSpeed);
        currentGear++;
        if (currentGear > 6) currentGear = 1;
    }

    private void DetectAndAvoidOtherVehicles()
    {
        // Raycasts in different directions to detect other vehicles around the AI
        bool frontBlocked = Physics.Raycast(transform.position, transform.forward, detectionRange, aiLayer);
        bool rightBlocked = Physics.Raycast(transform.position, transform.right, detectionRange / 2, aiLayer);
        bool leftBlocked = Physics.Raycast(transform.position, -transform.right, detectionRange / 2, aiLayer);
        bool rearBlocked = Physics.Raycast(transform.position, -transform.forward, detectionRange / 2, aiLayer);

        // Evasive action or lane switching based on traffic conditions
        if (frontBlocked)
        {
            if (!rightBlocked) SwitchLane("right");
            else if (!leftBlocked) SwitchLane("left");
        }
        else if (rearBlocked || (rightBlocked && leftBlocked))
        {
            SlowDown(); // Slow down if boxed in or no space to evade
        }
    }

    private void SwitchLane(string direction)
    {
        // Choose lane based on traffic situation and distance to next waypoint
        if (direction == "right")
        {
            if (currentLaneWaypoints != colourGizmos.FarRightWaypoints)
                currentLaneWaypoints = colourGizmos.FarRightWaypoints;
        }
        else if (direction == "left")
        {
            if (currentLaneWaypoints != colourGizmos.FarLeftWaypoints)
                currentLaneWaypoints = colourGizmos.FarLeftWaypoints;
        }
    }

    private void SlowDown()
    {
        speed = Mathf.Max(speed - (acceleration * 0.5f * Time.fixedDeltaTime), 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("AI") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HandleCollision();
        }
    }

    private void HandleCollision()
    {
        // Evasive action on collision: slow down or stop temporarily
        speed = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // After stopping briefly, choose a new lane and start moving again
        StartCoroutine(RecoverFromCollision());
    }

    private IEnumerator RecoverFromCollision()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second after collision

        // Reset the vehicle to its initial position after a collision
        ResetToInitialPosition();
    }

    // Method to reset the AI vehicle to its initial position and rotation
    public void ResetToInitialPosition()
    {
        // Stop the vehicle's movement
        speed = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reset the vehicle's position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}






