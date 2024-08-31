using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCarController : MonoBehaviour
{
    public float maxSpeed = 100f;
    public float turnSpeed = 50f;
    public float acceleration = 10f;
    public float deceleration = 20f;

    public AudioSource accelerationAudioSource;
    public AudioSource idleAudioSource; // Idle sound
    public GameObject restartButton; // Reference to the Restart button

    private Rigidbody rb;
    public float currentSpeed = 0f;
    private float moveInput = 0f;
    private float turnInput = 0f;
    private bool isCarStopped = false; // Flag to check if the car is stopped

    private Vector3 initialPosition;  // Store initial position for resetting
    private Quaternion initialRotation; // Store initial rotation for resetting

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        accelerationAudioSource.Play(); // Play the sound at the start

        // Save the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Hide the restart button at the start
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isCarStopped)
        {
            moveInput = -Input.GetAxis("Vertical"); // Invert the vertical input
            turnInput = -Input.GetAxis("Horizontal"); // Invert the horizontal input

            HandleAudio();
        }
    }

    private void FixedUpdate()
    {
        if (!isCarStopped)
        {
            HandleMovement();
            HandleTurning();
        }

        CheckIfFlipped();
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

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the car hits something hard enough
        if (collision.relativeVelocity.magnitude > 5f) // Threshold can be adjusted
        {
            StopCar();
        }
    }

    private void CheckIfFlipped()
    {
        // Check if the car is flipped over based on its rotation
        if (Vector3.Dot(transform.up, Vector3.down) > 0.5f) // Adjust the threshold if needed
        {
            StopCar();
        }
    }

    private void StopCar()
    {
        isCarStopped = true;
        currentSpeed = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Show the restart button
        if (restartButton != null)
        {
            restartButton.SetActive(true); // Show the button
        }

        Debug.Log("Car Crashed!"); // Log crash message
    }

    public void RestartGame()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
