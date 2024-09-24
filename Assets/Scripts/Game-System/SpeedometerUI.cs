using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedometerUI : MonoBehaviour
{
    public PlayerCarController carController; // Reference to your car controller script
    public TextMeshProUGUI speedText; // Reference to the TextMeshProUGUI component

    private void Start()
    {
        if (carController == null)
        {
            carController = FindObjectOfType<PlayerCarController>(); // Automatically find the car controller if not set
        }

        if (speedText == null)
        {
            speedText = GetComponent<TextMeshProUGUI>(); // Automatically find the TextMeshProUGUI if not set
        }
    }

    private void Update()
    {
        float speed = Mathf.Abs(carController.currentSpeed); // Get the absolute value of the car's current speed
        speedText.text = Mathf.RoundToInt(speed).ToString(); // Update the UI text with the speed
    }

}
