using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedometerUI : MonoBehaviour
{
    public PlayerCarController carController; 
    public TextMeshProUGUI speedText; 

    private void Start()
    {
        if (carController == null)
        {
            // Automatically find the car controller if not set
            carController = FindObjectOfType<PlayerCarController>(); 
        }

        if (speedText == null)
        {
            // Automatically find the TextMeshProUGUI if not set
            speedText = GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        // Get the absolute value of the car's current speed
        float speed = Mathf.Abs(carController.currentSpeed);
        // Update the UI text with the speed
        speedText.text = Mathf.RoundToInt(speed).ToString(); 
    }

}
