using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        // Make sure I exist
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // keep going between scenes we can add an if statement before this for when we have the main to kill it.
        }
        else
        {
            Destroy(gameObject); // kill duplicates
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            Destroy(gameObject);
        }
    }
}
