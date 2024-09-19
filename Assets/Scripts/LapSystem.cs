using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapSystem : MonoBehaviour
{
    public int totalLaps = 5;
    private int currentLap = 0;
    private bool canCountLap = true;
    private int currentCheckpointIndex = 0; // Track which checkpoint is next

    public Text lapCounterText;
    public GameObject nextRoundUI;
    public List<Transform> checkpoints; // Add all your checkpoints here
    private HashSet<int> passedCheckpoints; // Keep track of passed checkpoints

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
            // Get checkpoint index from the checkpoint game object
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
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

        // Handle lap counting when all checkpoints are passed and player crosses finish line
        if (other.CompareTag("LapTrigger") && passedCheckpoints.Count == checkpoints.Count && canCountLap)
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
    }

    private void UpdateLapCounter()
    {
        lapCounterText.text = currentLap + "/" + totalLaps;
    }

    private void ShowNextRoundUI()
    {
        nextRoundUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    private IEnumerator LapCooldown()
    {
        canCountLap = false;
        yield return new WaitForSeconds(1f); // Prevent multiple triggers
        canCountLap = true;
    }
}


