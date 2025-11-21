using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dimBackground;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public AudioSource dialogueAudio;
    public AudioSource advanceSound;

    [TextArea(3, 10)]
    public string[] lines; 

    public float typingSpeed = 0.03f;

    private int index;
    private bool isTyping;

    void Start()
    {
        dimBackground.SetActive(false);
        dialogueBox.SetActive(false);

        StartDialogue(); 
    }

    public void StartDialogue()
    {
        Time.timeScale = 0;

        dimBackground.SetActive(true);
        dialogueBox.SetActive(true);

        index = 0;
        StartCoroutine(TypeLine());

        if (dialogueAudio != null)
            dialogueAudio.Play();

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        if (!dialogueBox.activeSelf) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (advanceSound != null)
                advanceSound.Play();

            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
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
        dimBackground.SetActive(false);
        dialogueBox.SetActive(false);

        Time.timeScale = 1;

        if (dialogueAudio != null)
            dialogueAudio.Stop();
    }
}
