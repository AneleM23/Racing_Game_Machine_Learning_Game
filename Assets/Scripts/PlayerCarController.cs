using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    public float maxSpeed = 100f;
    public float turnSpeed = 50f;
    public float acceleration = 10f;
    public float deceleration = 20f;

    public AudioSource accelerationAudioSource;
    public AudioSource idleAudioSource; // Idle sound

    private Rigidbody rb;
    public float currentSpeed = 0f;
    private float moveInput = 0f;
    private float turnInput = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        accelerationAudioSource.Play(); // Play the sound at the start
    }

    private void Update()
    {
        moveInput = -Input.GetAxis("Vertical"); // Invert the vertical input
        turnInput = -Input.GetAxis("Horizontal"); // Invert the horizontal input

        HandleAudio();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleTurning();
    }

    private void HandleMovement()
    {
        if (moveInput != 0)
        {
            currentSpeed += moveInput * acceleration * Time.fixedDeltaTime;
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.fixedDeltaTime;
                if (currentSpeed < 0) currentSpeed = 0;
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += deceleration * Time.fixedDeltaTime;
                if (currentSpeed > 0) currentSpeed = 0;
            }
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        Vector3 move = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void HandleTurning()
    {
        if (currentSpeed != 0)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(currentSpeed);
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    private void HandleAudio()
    {
        float normalizedSpeed = Mathf.Abs(currentSpeed) / maxSpeed; // Normalize speed between 0 and 1

        // Adjust the pitch based on the normalized speed
        accelerationAudioSource.pitch = Mathf.Lerp(1.0f, 2.0f, normalizedSpeed); // Pitch range from 1.0 to 2.0

        // Adjust the volume based on input
        if (Mathf.Abs(currentSpeed) > 0)
        {
            accelerationAudioSource.volume = Mathf.Lerp(accelerationAudioSource.volume, 1.0f, Time.deltaTime * 2.0f); // Smoothly increase volume

            if (!accelerationAudioSource.isPlaying)
            {
                accelerationAudioSource.Play();
            }
            if (idleAudioSource.isPlaying)
            {
                idleAudioSource.Stop();
            }
        }
        else
        {
            accelerationAudioSource.volume = Mathf.Lerp(accelerationAudioSource.volume, 0.5f, Time.deltaTime * 2.0f); // Smoothly reduce volume when not accelerating

            if (!idleAudioSource.isPlaying)
            {
                idleAudioSource.Play();
            }
            accelerationAudioSource.Stop();
        }
    }

}
