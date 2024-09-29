using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoundLevel3 : MonoBehaviour
{
    public void OnYesButtonClick()
    {
        // Unpause the game and hide the UI
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }

    public void OnNoButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
