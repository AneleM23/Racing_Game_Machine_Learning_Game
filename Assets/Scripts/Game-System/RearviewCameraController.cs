using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearviewCameraController : MonoBehaviour
{
    public Transform target; // The player's car
    public float offset = 5f; // The distance between the camera and the car
    public RenderTexture rearViewTexture; // The RenderTexture for the rear view camera

    private Vector3 initialOffset;

    void Start()
    {
        initialOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        transform.position = target.position + initialOffset;
        transform.LookAt(target.position);
    }

}
