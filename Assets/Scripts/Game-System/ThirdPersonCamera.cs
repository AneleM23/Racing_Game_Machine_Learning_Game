using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform carTransform;  // Reference to the car's transform
    public Vector3 offset;          // Offset from the car
    public float smoothSpeed = 0.125f; // Smoothness factor
    public float rotationSpeed = 5f;   // Speed of the camera rotation

    private void LateUpdate()
    {
        /// Calculate the desired position with offset
        Vector3 desiredPosition = carTransform.position + carTransform.rotation * offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Ensure the camera is always looking at the car
        transform.LookAt(carTransform);

    }



}
