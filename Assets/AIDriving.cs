using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriving : MonoBehaviour
{
    private Transform[] currentLaneWaypoints;
    private int currentWaypointIndex = 0;

    private Colour_Gizmos colourGizmos;

    public float speed;
    public float rotationSpeed = 50f;
    public float acceleration;
    public float maxSpeed;  // Adjusted max speed for race cars
    public float detectionRange = 5f;  // Range to detect other vehicles ahead
    public LayerMask aiLayer;  // Layer mask for detecting other AI vehicles
    public float cornerDecelerationFactor = 0.3f;  // Reduced factor for less deceleration at corners

    private Rigidbody rb;

    // Gear parameters
    private int currentGear = 1;
    private float gearChangeSpeedFactor = 0.8f; // Factor to reduce speed during gear change
    private float gearChangeDuration = 0.5f; // Duration of gear change effect

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Set interpolation for smooth movement

        colourGizmos = GetComponent<Colour_Gizmos>();
        if (colourGizmos != null)
        {
            ChooseRandomLane();  // Each AI chooses a lane
        }
        else
        {
            Debug.LogError("Colour_Gizmos script not found on this GameObject!");
        }

        // Randomize vehicle behavior for more variability
        acceleration = Random.Range(8f, 12f);  // Different acceleration ranges
        maxSpeed = Random.Range(100f, 150f);   // Different max speed ranges
        speed = Random.Range(5f, 10f); // Starting speed range
        detectionRange = Random.Range(10f, 20f);  // Random detection range
    }

    private void FixedUpdate() // Changed to FixedUpdate for physics-based calculations
    {
        DriveTowardsNextWaypoint();
        DetectAndAvoidOtherVehicles();  // Check for nearby AI vehicles and avoid them
    }

    private void DriveTowardsNextWaypoint()
    {
        // Get the current waypoint in the chosen lane
        Vector3 direction = currentLaneWaypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0;  // Ensure the AI doesn't move vertically

        // Rotate towards the waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        // Ensure the car's local forward direction is correct
        if (Vector3.Dot(transform.forward, direction.normalized) < 0.95f)
        {
            // If the vehicle is off by a large angle, stop it from moving
            return;
        }

        // Move the vehicle forward in the direction it is facing
        Vector3 moveDir = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);

        // Check if we've reached the waypoint
        if (direction.magnitude < 2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= currentLaneWaypoints.Length)
            {
                currentWaypointIndex = 0;  // Loop back to the start
            }
        }

        // Manage acceleration and deceleration
        AdjustSpeedOnCorners();
        ManageOverallSpeed();  // New method for overall speed control
    }

    // Randomly select a lane for each AI vehicle
    private void ChooseRandomLane()
    {
        int randomLane = Random.Range(0, 3);  // 0: inner, 1: middle, 2: outer

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
        // Check if the next waypoint is tagged as a corner
        if (currentWaypointIndex < currentLaneWaypoints.Length && currentLaneWaypoints[currentWaypointIndex].CompareTag("Corner"))
        {
            // Slightly decelerate for corners
            speed = Mathf.Max(speed - (acceleration * cornerDecelerationFactor * Time.fixedDeltaTime), 0.8f * maxSpeed);
        }
        else
        {
            // Speed up to max speed after passing the corner
            speed = Mathf.Min(speed + (acceleration * Time.fixedDeltaTime), maxSpeed);
        }

        // Simulate gear change
        if (speed > maxSpeed * gearChangeSpeedFactor)
        {
            ChangeGear();
        }
    }

    private void ManageOverallSpeed()
    {
        // Gradually reduce speed when not accelerating
        if (speed > 20f)  // Prevent it from going below a certain minimum speed
        {
            speed = Mathf.Max(speed - (acceleration * 0.1f * Time.fixedDeltaTime), 20f);  // Gradually reduce speed
        }
    }

    private void ChangeGear()
    {
        // Reduce speed temporarily during gear change
        speed *= gearChangeSpeedFactor;

        // Wait for gear change duration before accelerating again
        StartCoroutine(GearChangeCoroutine());
    }

    private IEnumerator GearChangeCoroutine()
    {
        yield return new WaitForSeconds(gearChangeDuration);

        // After the delay, increase speed back towards max
        speed = Mathf.Min(speed + (acceleration * 2f * Time.fixedDeltaTime), maxSpeed); // Double the acceleration to simulate the post-gear change speed up
        currentGear++;
        if (currentGear > 6) currentGear = 1; // Reset gear if it exceeds the max
    }

    private void DetectAndAvoidOtherVehicles()
    {
        // Check for any AI vehicles ahead within the detection range
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, aiLayer))
        {
            // If an AI vehicle is detected ahead, switch lanes
            SwitchLane();
        }
    }

    private void SwitchLane()
    {
        // Check if the next lane is clear and switch to a different lane
        int nextLane = Random.Range(0, 3);
        switch (nextLane)
        {
            case 0:
                if (currentLaneWaypoints != colourGizmos.FarRightWaypoints)  // Ensure we're not already in this lane
                    currentLaneWaypoints = colourGizmos.FarRightWaypoints;
                break;
            case 1:
                if (currentLaneWaypoints != colourGizmos.middleLaneWaypoints)
                    currentLaneWaypoints = colourGizmos.middleLaneWaypoints;
                break;
            case 2:
                if (currentLaneWaypoints != colourGizmos.FarLeftWaypoints)
                    currentLaneWaypoints = colourGizmos.FarLeftWaypoints;
                break;
        }
    }
}




