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
    public int possibleDialogues = 0;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] List<TextMeshProUGUI> dialogueOptions;
    [SerializeField] float dialogueTime= 4;

    private bool _isDialogueActive = false;
    public Action<int> OnDialogueClick;
    private Queue<DialogueData> dialogueQueue = new();

    void Start()
    {
        DialoguePanelInstance = this;
    }

    public void ShowDialogueOptions(List<string> dialogues, Color color, Action<int> onDialogueClick)
    {
        possibleDialogues = dialogues.Count;
        OnDialogueClick = onDialogueClick;

        for (int i = 0; i< possibleDialogues; i++)
        {
            dialogueOptions[i].text = $"{i+1}. {dialogues[i]}";
            dialogueOptions[i].color = color;
            dialogueOptions[i].gameObject.SetActive(true);
        }

        for (int i = possibleDialogues; i < dialogueOptions.Count; i++)
        {
            dialogueOptions[i].gameObject.SetActive(false);
        }
    }

    public void ShowDialogue(DialogueData dialogueData)
    {
        StartCoroutine(ShowDialogueCoroutine (dialogueData));
    }

    public IEnumerator ShowDialogueCoroutine(DialogueData dialogueData)
    {
        for (int i = 0; i < dialogueOptions.Count; i++)
        {
            dialogueOptions[i].gameObject.SetActive(false);
        }

        possibleDialogues = 0;

        if (dialogueData.GameEventsEnum == GameController.GameEventsEnum.None ||
            GameController.GameControllerInstance.WasEventDone(dialogueData.GameEventsEnum))
        {
            if (!_isDialogueActive)
            {
                _isDialogueActive = true;
                dialogueText.text = dialogueData.Text;
                dialogueText.color = dialogueData.Color;
                dialogueText.gameObject.SetActive(true);

                //yield return new WaitForSeconds(dialogueData.ShowTime != 0 ? dialogueData.ShowTime : dialogueTime);
                yield return new WaitForSeconds(3);

                dialogueText.gameObject.SetActive(false);
                onDialogueEnded?.Invoke();
                _isDialogueActive = false;

                DeQueueDialogue();
                yield return null;
            }
            else
            {
                QueueDialogue(dialogueData);
                yield return null;
            }
        }
        else
        {
            yield return null;
        }
        yield return null;
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
        public GameController.GameEventsEnum GameEventsEnum; 
        public GameController.GameEventsEnum BlockedAfterGameEventsEnum; 
        public string Text;
        public float ShowTime;
        public Color Color;

        public DialogueData( string text, float showTime, Color color, GameController.GameEventsEnum gameEventsEnum)
        {
            Text = text;
            ShowTime = showTime;
            Color = color;
            GameEventsEnum = gameEventsEnum;
        }
    }
}
