using System.Collections.Generic;
using UnityEngine;
using static DialoguePanel;

public class InteractableActionDisplayDialogue : InteractableAction
{
    [SerializeField] List<DialogueData> dialogueData;

    public override void PlayAction()
    {
        foreach (DialogueData data in dialogueData)
        {
            DialoguePanel.DialoguePanelInstance.ShowDialogue(data);
        }
    }
}
