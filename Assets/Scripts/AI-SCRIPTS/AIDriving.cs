using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriving : MonoBehaviour
{
    private Transform[] currentLaneWaypoints;
    private int currentWaypointIndex = 0;

    private Colour_Gizmos colourGizmos;

    private LapSystem lapSystem;

    public float speed;
    public float rotationSpeed = 50f;
    public float acceleration;
    public float maxSpeed;
    public float detectionRange = 5f;
    public LayerMask aiLayer;
    public float cornerDecelerationFactor = 0.3f;

    private Rigidbody rb;

    private int currentGear = 1;
    private float gearChangeSpeedFactor = 0.8f;
    private float gearChangeDuration = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        colourGizmos = GetComponent<Colour_Gizmos>();
        if (colourGizmos != null)
        {
            ChooseRandomLane();
        }
        else
        {
            Debug.LogError("Colour_Gizmos script not found on this GameObject!");
        }

        acceleration = Random.Range(8f, 12f);
        maxSpeed = Random.Range(100f, 150f);
        speed = Random.Range(5f, 10f);
        detectionRange = Random.Range(10f, 20f);

        lapSystem = FindObjectOfType<LapSystem>();
    }

    private void FixedUpdate()
    {
        // Check if the AI has completed all laps
        if (lapSystem != null && lapSystem.currentLap >= lapSystem.totalLaps)
        {
            speed = 0;  // Stop the AI vehicle
            return;  // Skip the rest of the update
        }

        DriveTowardsNextWaypoint();
        DetectAndAvoidOtherVehicles();
    }

    private void DriveTowardsNextWaypoint()
    {
        Vector3 direction = currentLaneWaypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        if (Vector3.Dot(transform.forward, direction.normalized) < 0.95f)
        {
            return;
        }

        Vector3 moveDir = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);

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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, aiLayer))
        {
            SwitchLane();
        }
    }

    private void SwitchLane()
    {
        int nextLane = Random.Range(0, 3);
        switch (nextLane)
        {
            case 0:
                if (currentLaneWaypoints != colourGizmos.FarRightWaypoints)
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




