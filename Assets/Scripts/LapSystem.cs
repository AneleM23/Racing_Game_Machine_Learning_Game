using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapSystem : MonoBehaviour
{
    public int totalLaps = 5;
    private int currentLap = 0;
    private bool canCountLap = true;

    public Text lapCounterText;
    public GameObject nextRoundUI;

    private void Start()
    {
        UpdateLapCounter();
        nextRoundUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LapTrigger") && canCountLap)
        {
            currentLap++;
            UpdateLapCounter();

            if (currentLap >= totalLaps)
            {
                ShowNextRoundUI();
            }

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
