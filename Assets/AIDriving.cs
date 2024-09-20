using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriving : MonoBehaviour
{
    private Transform[] currentLaneWaypoints;
    private int currentWaypointIndex = 0;

    private Colour_Gizmos colourGizmos;

    public float speed = 10f;
    public float rotationSpeed = 50f;
    public float acceleration = 10f;
    public float maxSpeed = 200f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get the Colour_Gizmos script from the same GameObject
        colourGizmos = GetComponent<Colour_Gizmos>();

        if (colourGizmos != null)
        {
            ChooseRandomLane();  // Each AI chooses a lane
        }
        else
        {
            Debug.LogError("Colour_Gizmos script not found on this GameObject!");
        }
    }

    private void Update()
    {
        DriveTowardsNextWaypoint();
    }

    private void DriveTowardsNextWaypoint()
    {
        // Get the current waypoint in the chosen lane
        Vector3 direction = currentLaneWaypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0;  // Ensure the AI doesn't move vertically

        // Rotate towards the waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));

        // Ensure the car's local forward direction is correct
        if (Vector3.Dot(transform.forward, direction.normalized) < 0.95f)
        {
            // If the vehicle is off by a large angle, stop it from moving
            return;
        }

        // Move the vehicle forward in the direction it is facing
        Vector3 moveDir = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(transform.position + moveDir);

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
        // Slow down the vehicle based on waypoint conditions (corner sharpness)
        if (currentWaypointIndex > 0)
        {
            Transform previousWaypoint = currentLaneWaypoints[currentWaypointIndex - 1];
            Vector3 toNextWaypoint = currentLaneWaypoints[currentWaypointIndex].position - previousWaypoint.position;

            float cornerAngle = Vector3.Angle(transform.forward, toNextWaypoint.normalized);

            // For sharper corners, reduce speed; otherwise, increase it
            if (cornerAngle > 45f)  // Sharper turns
            {
                speed = Mathf.Max(speed - acceleration * Time.deltaTime, 20f);  // Decelerate for corners
            }
            else
            {
                speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);  // Speed up
            }
        }
        else
        {
            speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);  // Gradually speed up
        }
    }
}

