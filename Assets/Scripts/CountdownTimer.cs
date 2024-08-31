using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountdownTimer : MonoBehaviour
{
    public Text countdownText;  // Reference to the UI Text component for countdown display
    public int countdownTime = 3; // Starting countdown time (from 3)
    public PlayerCarController playerCarController; // Reference to the car controller script

    private void Start()
    {
        // Disable the car movement at the start
        playerCarController.enabled = false;

        // Start the countdown coroutine
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        // Countdown loop
        while (countdownTime > 0)
        {
            // Update the countdown UI text
            countdownText.text = countdownTime.ToString();

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease the countdown time
            countdownTime--;
        }

        // Countdown reached 0, clear the text
        countdownText.text = "";

        // Enable the car movement after the countdown
        playerCarController.enabled = true;
    }

}
