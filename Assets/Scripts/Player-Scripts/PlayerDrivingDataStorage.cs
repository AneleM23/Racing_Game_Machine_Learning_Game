using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrivingDataStorage : MonoBehaviour
{
    // Singleton instance
    public static PlayerDrivingDataStorage Instance { get; private set; }

    // Public properties to store player driving data
    public float speed; 
    public float acceleration; 

    private void Awake()
    {
        // Check if there is already an instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    // Method to reset data if needed
    public void ResetData()
    {
        speed = 0f;
        acceleration = 0f;
    }

    
}

