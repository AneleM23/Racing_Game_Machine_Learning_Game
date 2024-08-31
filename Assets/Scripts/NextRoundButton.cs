using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoundButton : MonoBehaviour
{
   

    public void OnYesButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("AI_LEVEL");
    }

    public void OnNoButtonClick()
    {
        // Unpause the game and hide the UI
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
