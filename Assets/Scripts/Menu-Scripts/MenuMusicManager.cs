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
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
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

    // Use SceneManager.sceneLoaded instead of OnLevelWasLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Check if we are in a menu or credits scene
        if (sceneName == "Menu" || sceneName == "Credits")
        {
            // Do nothing, keep music playing
        }
        else
        {
            // If it's not the menu or credits, stop the music manager
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

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from sceneLoaded event
    }




}
