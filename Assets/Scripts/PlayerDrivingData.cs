using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrivingData : MonoBehaviour
{
    private float currentSpeed;
    private float currentAcceleration;
    public float maxSpeed;

    // Call this method from your PlayerCarController to update the speed and acceleration
    public void UpdateDrivingData(float speed, float acceleration)
    {
        currentSpeed = speed;
        currentAcceleration = acceleration;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetCurrentAcceleration()
    {
        return currentAcceleration;
    }



}
