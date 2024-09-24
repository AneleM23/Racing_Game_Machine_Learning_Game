using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrivingDataStorage : MonoBehaviour
{
    // Singleton instance
    public static PlayerDrivingDataStorage Instance { get; private set; }

    // Public properties to store player driving data
    public float speed; // Speed of the player
    public float acceleration; // Acceleration of the player

    private void Awake()
    {
        // Check if there is already an instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
    }

    // Method to reset data if needed
    public void ResetData()
    {
        speed = 0f;
        acceleration = 0f;
    }

    // Optionally, you can add more methods to manipulate or retrieve driving data
}

