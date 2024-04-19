using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialoguePanel;

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
                DialoguePanel.DialoguePanelInstance.ShowDialogue(data);
                Collider.enabled = false;
            }
        }
    }
}
