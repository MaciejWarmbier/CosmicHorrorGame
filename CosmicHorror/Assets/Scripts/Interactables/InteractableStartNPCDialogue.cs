using UnityEngine;

public class InteractableStartNPCDialogue : InteractableAction
{
    [SerializeField] NPC npc;

    public override void PlayAction()
    {
        npc.StartDialogue();
    }
}
