using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform carTransform;  // Reference to the car's transform
    public Vector3 offset;          // Offset from the car
    public float smoothSpeed = 0.125f; // Smoothness factor
    public float rotationSpeed = 5f;   // Speed of the camera rotation

    private void LateUpdate()
    {
        // Desired position of the camera
        Vector3 desiredPosition = carTransform.position + offset;

        // Smooth transition to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the smoothed position to the camera
        transform.position = smoothedPosition;

        // Rotate the camera to follow the car's rotation
        Quaternion desiredRotation = Quaternion.LookRotation(carTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

}
