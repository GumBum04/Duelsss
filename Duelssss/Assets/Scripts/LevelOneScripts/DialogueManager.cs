using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Speaker
{
    Left,
    Right
}

[System.Serializable]
public class DialogueLine
{
    public Speaker speaker;
    public string speakerName;
    [TextArea(3, 10)]
    public string text;
    public AudioClip voiceClip; // dialogue audio clip - David
}

public class DialogueManager : MonoBehaviour
{
    public static bool InDialogue = false; // lock lose-screen logic while in dialogue - David
    public GameObject dimBackground;

    [Header("Left Dialogue UI")]
    public GameObject leftBox;
    public TextMeshProUGUI nameText_Left;
    public TextMeshProUGUI dialogueText_Left;

    [Header("Right Dialogue UI")]
    public GameObject rightBox;
    public TextMeshProUGUI nameText_Right;
    public TextMeshProUGUI dialogueText_Right;

    [Header("Audio")]
    public AudioSource dialogueAudio; // THIS IS NOW ONLY FOR DIALOGUE (assign DialogueAudio) - David
    public AudioSource advanceSound;

    [Header("Dialogue Content")]
    public DialogueLine[] lines;
    public float typingSpeed = 0.03f;

    private int index;
    private bool isTyping;
    private bool lockInput = false; // prevents clicks after last line - David

    public AudioSource backgroundMusic;

    void Start()
    {
        dimBackground.SetActive(false);
        leftBox.SetActive(false);
        rightBox.SetActive(false);

        StartDialogue();
    }

    public void StartDialogue()
    {
        InDialogue = true; // lock lose-screen logic - David
        lockInput = false; // reset input lock - David
        Time.timeScale = 0;
        SongManager.PauseGame(); // pause gameplay song - David

        dimBackground.SetActive(true);

        index = 0;
        StartCoroutine(TypeLine());

        if (backgroundMusic != null)
            backgroundMusic.Pause(); // pause background music during dialogue - David
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        leftBox.SetActive(false);
        rightBox.SetActive(false);

        DialogueLine current = lines[index];

        // play dialogue audio clip on dialogue-only audio source - David
        if (dialogueAudio != null)
        {
            dialogueAudio.Stop();
            if (current.voiceClip != null)
            {
                dialogueAudio.clip = current.voiceClip;
                dialogueAudio.Play();
            }
        }

        // type out text for the correct speaker - David
        if (current.speaker == Speaker.Left)
        {
            leftBox.SetActive(true);
            nameText_Left.text = current.speakerName;
            dialogueText_Left.text = "";

            foreach (char c in current.text)
            {
                dialogueText_Left.text += c;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }
        else
        {
            rightBox.SetActive(true);
            nameText_Right.text = current.speakerName;
            dialogueText_Right.text = "";

            foreach (char c in current.text)
            {
                dialogueText_Right.text += c;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }

        isTyping = false;
    }

    void Update()
    {
        if (!dimBackground.activeSelf || lockInput)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (advanceSound != null)
                advanceSound.Play();

            // finish typing instantly if mid-type - David
            if (isTyping)
            {
                StopAllCoroutines();
                if (dialogueAudio.isPlaying)
                    dialogueAudio.Stop(); // stop current line audio - David

                if (lines[index].speaker == Speaker.Left)
                    dialogueText_Left.text = lines[index].text;
                else
                    dialogueText_Right.text = lines[index].text;

                isTyping = false;
            }
            else
            {
                // last line? lock input and immediately end dialogue - David
                if (index >= lines.Length - 1)
                {
                    lockInput = true; // prevent further clicks - David
                    if (dialogueAudio.isPlaying)
                        dialogueAudio.Stop(); // kill last dialogue audio - David
                    EndDialogueImmediate(); // immediately start song - David
                }
                else
                {
                    // stop current dialogue audio before next line - David
                    if (dialogueAudio.isPlaying)
                        dialogueAudio.Stop();
                    NextLine();
                }
            }
        }
    }

    void NextLine()
    {
        if (lockInput) return;

        index++;
        if (index < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            // safety: stop audio and immediately end dialogue - David
            if (dialogueAudio.isPlaying)
                dialogueAudio.Stop();
            EndDialogueImmediate();
        }
    }

    void EndDialogueImmediate()
    {
        InDialogue = false; // unlock lose-screen logic - David

        Time.timeScale = 1;
        SongManager.ResumeGame(); // unpause SongManager - David

        leftBox.SetActive(false);
        rightBox.SetActive(false);
        dimBackground.SetActive(false);

        if (backgroundMusic != null)
            backgroundMusic.UnPause();

        // IMMEDIATELY start gameplay song without waiting for dialogue audio - David
        SongManager.Instance.StartSong();
    }
}
