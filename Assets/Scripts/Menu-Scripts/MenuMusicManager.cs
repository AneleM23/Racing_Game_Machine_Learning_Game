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

    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

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
        SceneManager.LoadScene("Level_1");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
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
