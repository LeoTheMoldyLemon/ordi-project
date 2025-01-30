using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private DialogueLine[] dialogueLines;
    private DialogueLine lastDialogue;

    private int currentLine = 0;
    public bool triggered = false;
    [SerializeField] private bool pause = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        triggered = true;
        PlayerController.Instance.locked = true;
        TextWriterUI.Instance.textDone.AddListener(NextLine);
        if (pause) Time.timeScale = 0;
        NextLine();
    }

    private void NextLine()
    {
        if (lastDialogue != null) lastDialogue.Deactivate();
        if (currentLine >= dialogueLines.Length)
        {
            PlayerController.Instance.locked = false;
            if (pause) Time.timeScale = 1;
            return;
        }
        DialogueLine dialogue = dialogueLines[currentLine];
        dialogue.Activate();
        currentLine++;
        lastDialogue = dialogue;
    }

}
