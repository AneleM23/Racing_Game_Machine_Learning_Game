using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapSystem : MonoBehaviour
{
    // This script handles the lap system in a racing game, tracking laps, checkpoints, and displaying the lap counter. 
    // It also manages the UI for proceeding to the next round after completing all laps

    public int totalLaps = 5;
    public int currentLap = 0;
    public bool canCountLap = true; // Changed to public
    private int currentCheckpointIndex = 0; // Track which checkpoint is next

    public Text lapCounterText;
    public GameObject nextRoundUI;
    public List<Transform> checkpoints; // Add all your checkpoints here
    public HashSet<int> passedCheckpoints; // Changed to public

    private void Start()
    {
        UpdateLapCounter();
        nextRoundUI.SetActive(false);
        passedCheckpoints = new HashSet<int>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle when car enters a checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            HandleCheckpoint(other.GetComponent<Checkpoint>());
        }

        // Handle lap counting when all checkpoints are passed and player crosses finish line
        if (other.CompareTag("LapTrigger") && passedCheckpoints.Count == checkpoints.Count && canCountLap)
        {
            CompleteLap();
        }
    }

    private void HandleCheckpoint(Checkpoint checkpoint)
    { // Check if the player passed the correct checkpoint in the right order
        if (checkpoint.index == currentCheckpointIndex)
        {
            passedCheckpoints.Add(currentCheckpointIndex); // Mark this checkpoint as passed
            currentCheckpointIndex++;

            // Loop back to the first checkpoint after the last
            if (currentCheckpointIndex >= checkpoints.Count)
            {
                currentCheckpointIndex = 0;
            }
        }
    }

    // Ensure the AI vehicle passed the correct checkpoint in order
    public void AIHandleCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex == currentCheckpointIndex)
        {
            passedCheckpoints.Add(currentCheckpointIndex); // Mark this checkpoint as passed
            currentCheckpointIndex++;

            // Loop back to the first checkpoint after the last
            if (currentCheckpointIndex >= checkpoints.Count)
            {
                currentCheckpointIndex = 0;
            }
        }
    }

    public bool HasCompletedAllLaps()
    {
        return currentLap >= totalLaps; // Add this method
    }

    public void CompleteLap() // Change to public
    {
        currentLap++;
        UpdateLapCounter();

        if (currentLap >= totalLaps)
        {
            ShowNextRoundUI();
        }

        // Reset checkpoints after lap is counted
        passedCheckpoints.Clear();
        StartCoroutine(LapCooldown());
    }

    private void UpdateLapCounter()
    {
        lapCounterText.text = currentLap + "/" + totalLaps;
    }

    // Display the next round UI and pause the game when all laps are completed

    private void ShowNextRoundUI()
    {
        nextRoundUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    // Cooldown to prevent multiple triggers from counting the same lap
    private IEnumerator LapCooldown()
    {
        canCountLap = false;
        yield return new WaitForSeconds(1f); // Prevent multiple triggers
        canCountLap = true;
    }
}




