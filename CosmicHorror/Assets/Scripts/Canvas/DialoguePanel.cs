using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public Action onDialogueEnded;
    public static DialoguePanel DialoguePanelInstance = null;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] int dialogueTimeMS = 4000;

    private bool _isDialogueActive = false;
    private Queue<DialogueData> dialogueQueue = new();

    void Start()
    {
        DialoguePanelInstance = this;
    }

    public async void ShowDialogue(DialogueData dialogueData)
    {
        if (!_isDialogueActive)
        {
            _isDialogueActive = true;
            dialogueText.text = dialogueData.Text;
            dialogueText.gameObject.SetActive(true);

            await Task.Delay(dialogueData.ShowTimeMS != 0 ? dialogueData.ShowTimeMS : dialogueTimeMS);

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
        public int ShowTimeMS;
        public Color Color;
    }
}
