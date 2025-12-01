// When the audio component has stopped playing, play otherClip.
// Remember to assign an AudioClip in the Inspector.

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IsAudioPlaying : MonoBehaviour
{

    AudioSource audioSource;
    bool hasStarted = false;
    bool hasFinished = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
void Update()
{
    if (!hasStarted && audioSource.isPlaying)
    {
        hasStarted = true; // audio actually started
    }

    if (hasStarted && !audioSource.isPlaying && !hasFinished)
    {
        hasFinished = true;

        if (ScoreManager.comboScore < 100)
        {
            SceneManager.LoadScene("LoseScreen");
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
}