using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearviewCameraController : MonoBehaviour
{
    public Transform carTransform;  // Reference to the car's transform
    public Vector3 offset;          // Offset from the car (should be behind the car)
    public float smoothSpeed = 0.125f; // Smoothness factor

    private void LateUpdate()
    {
        // Calculate the desired position behind the car using the car's forward direction
        Vector3 desiredPosition = carTransform.position - carTransform.forward * offset.z + carTransform.up * offset.y;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Ensure the camera is always looking at the back of the car
        transform.LookAt(carTransform.position + carTransform.forward * 2f);
    }


}
