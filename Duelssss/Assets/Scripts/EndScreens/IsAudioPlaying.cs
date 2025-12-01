using UnityEngine;
using UnityEngine.SceneManagement;

public class IsAudioPlaying : MonoBehaviour
{
    AudioSource audioSource; // this should be the gameplay song audio source - David
    bool hasStarted = false; // tracks if gameplay audio has started - David
    bool hasFinished = false; // tracks if we've already processed end of song - David

    void Start()
    {
        // get the audio source we're monitoring (gameplay song only, not dialogue) - David
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // leave me alone if we're in dialogue - David
        if (DialogueManager.InDialogue)
            return;

        // mark that gameplay audio actually started - David
        if (!hasStarted && audioSource.isPlaying)
        {
            hasStarted = true;
        }

        // gameplay audio stopped and we haven't already finished - David
        if (hasStarted && !audioSource.isPlaying && !hasFinished)
        {
            hasFinished = true; // mark finished so this doesn't run again - David

            // check score and go to appropriate screen - David
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
