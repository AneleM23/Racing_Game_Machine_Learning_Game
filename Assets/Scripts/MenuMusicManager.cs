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
        // Singleton pattern to prevent duplicate MenuMusicManager instances
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    private void Start()
    {
        if (!musicStarted)
        {
            PlayRandomSong();
            musicStarted = true;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        string sceneName = SceneManager.GetSceneByBuildIndex(level).name;

        if (sceneName != "Menu" && sceneName != "Credits")
        {
            Destroy(gameObject); // Destroy the music manager when leaving menu or credits
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

    public void StopMusic()
    {
        audioSource.Stop();
        musicStarted = false; // Reset the flag so music can restart if needed
    }

    // Function to start the game and load Level1
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Function to load the Credits scene
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    // Function to quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }



}
