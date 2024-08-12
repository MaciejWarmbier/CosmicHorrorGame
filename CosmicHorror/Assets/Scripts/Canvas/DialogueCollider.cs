using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialoguePanel;
using static GameController;

public class DialogueCollider : MonoBehaviour
{
    [SerializeField] Collider Collider;
    [SerializeField] string TagCollider;
    [SerializeField] List<DialogueData> dialogueData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(TagCollider))
        {
            foreach (DialogueData data in dialogueData)
            {
                if (data.BlockedAfterGameEventsEnum == GameController.GameEventsEnum.None ||  !GameController.GameControllerInstance.WasEventDone(data.BlockedAfterGameEventsEnum))
                {
                    DialoguePanel.DialoguePanelInstance.ShowDialogue(data);
                }
            }
            Collider.enabled = false;
        }
    }
}
