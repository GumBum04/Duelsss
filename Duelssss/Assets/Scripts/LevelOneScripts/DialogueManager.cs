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
}

public class DialogueManager : MonoBehaviour
{
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
    public AudioSource dialogueAudio;
    public AudioSource advanceSound;

    [Header("Dialogue Content")]
    public DialogueLine[] lines;
    public float typingSpeed = 0.03f;

    private int index;
    private bool isTyping;

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
        Time.timeScale = 0;
        SongManager.PauseGame();

        dimBackground.SetActive(true);

        index = 0;
        StartCoroutine(TypeLine());

        if (dialogueAudio != null)
            dialogueAudio.Play();

        if (backgroundMusic != null)
            backgroundMusic.Pause();

    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        leftBox.SetActive(false);
        rightBox.SetActive(false);

        DialogueLine current = lines[index];

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
        if (!dimBackground.activeSelf) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (advanceSound != null)
                advanceSound.Play();

            if (isTyping)
            {
                StopAllCoroutines();
                if (lines[index].speaker == Speaker.Left)
                    dialogueText_Left.text = lines[index].text;
                else
                    dialogueText_Right.text = lines[index].text;

                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        Time.timeScale = 1;
        SongManager.ResumeGame();

        leftBox.SetActive(false);
        rightBox.SetActive(false);
        dimBackground.SetActive(false);
    
        if (dialogueAudio != null)
            dialogueAudio.Stop();

        if (backgroundMusic != null)
            backgroundMusic.UnPause();

        SongManager.Instance.StartSong();
    }
}
