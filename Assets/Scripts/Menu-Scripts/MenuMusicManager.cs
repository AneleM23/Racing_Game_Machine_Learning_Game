using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] menuSongs;

    private static MenuMusicManager instance;
    private static bool musicStarted = false;

   // private void Awake()
   // {
    //    GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
    //    if (musicObj.Length > 1)
     //   {
      //      Destroy(this.gameObject);
      //  }
       // DontDestroyOnLoad(this.gameObject);
   // }

    private void Start()
    {
        if (!musicStarted)
        {
            PlayRandomSong();
            musicStarted = true;
        }
    }

  

    private void PlayRandomSong()
    {
        if (menuSongs.Length > 0)
        {
            audioSource.clip = menuSongs[Random.Range(0, menuSongs.Length)];
            audioSource.Play();
        }
    }

    

    public void StartGame()
    {
        Debug.Log("Game Has Started");
        SceneManager.LoadScene("Level_1");
    }

    public void GoToCredits()
    {
        Debug.Log("Credits");
        SceneManager.LoadScene("Credits");
    }

    public void GoToTest()
    {
        Debug.Log("Test Scene");
        SceneManager.LoadScene("Level_3");
    }
    public void GoToMenu()
    {
        Debug.Log("Menu Scene");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

   
}
