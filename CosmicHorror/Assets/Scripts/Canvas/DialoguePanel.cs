using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public Action onDialogueEnded;
    public static DialoguePanel DialoguePanelInstance = null;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] float dialogueTime= 4;

    private bool _isDialogueActive = false;
    private Queue<DialogueData> dialogueQueue = new();

    void Start()
    {
        DialoguePanelInstance = this;
    }

    public IEnumerator ShowDialogue(DialogueData dialogueData)
    {
        if (!_isDialogueActive)
        {
            _isDialogueActive = true;
            dialogueText.text = dialogueData.Text;
            dialogueText.gameObject.SetActive(true);

            yield return new WaitForSeconds(dialogueData.ShowTime != 0 ? dialogueData.ShowTime : dialogueTime);

            dialogueText.gameObject.SetActive(false);
            onDialogueEnded?.Invoke();
            _isDialogueActive =false;

            DeQueueDialogue();
        }
        else
        {
            QueueDialogue(dialogueData);
        }
    }

    public void QueueDialogue(DialogueData dialogueData)
    {
        dialogueQueue.Enqueue(dialogueData);
    }

    public void DeQueueDialogue()
    {
        if(dialogueQueue != null && dialogueQueue.Count > 0)
        {
            var dialogue = dialogueQueue.Dequeue();
            ShowDialogue(dialogue);
        }
    }

    [Serializable]
    public class DialogueData
    {
        public string Text;
        public float ShowTime;
        public Color Color;

        public DialogueData(string text, float showTime, Color color)
        {
            Text = text;
            ShowTime = showTime;
            Color = color;
        }
    }
}
