using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameController;
using static SoundConfig;

public class NPC : MonoBehaviour
{
    [SerializeField] int timeForDisplayingDialogueMS = 3000;
    [SerializeField] Color dialogueColor;
    [SerializeField] List<NpcInteractions> interactions;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioEnum> npcAudio;


    private List<NpcInteractions> _doneInteractions = new();
    private List<NpcInteractions> _pendingInteractions = new();
    private List<NpcInteractions> _availableInteractions = new();
    private bool _isInteractionPlaying = false;

    private void Awake()
    {
        _pendingInteractions = interactions;
    }

    private void Start()
    {
        foreach(var interaction in _pendingInteractions)
        {
            bool isWaitingForOtherInteraction = _pendingInteractions.Exists(x=> x.interactionEnum == interaction.interactionBlocker.interactionEnum);
            bool isWaitingForEvent = !GameController.GameControllerInstance.WasEventDone(interaction.interactionBlocker.gameEventsEnum);

            if(!isWaitingForOtherInteraction && !isWaitingForEvent)
            {
                _availableInteractions.Add(interaction);
                _pendingInteractions.Remove(interaction);
            }
        }
    }

    public void ShowInteraction(InteractionEnum interactionEnum)
    {
        var interaction = _availableInteractions.FirstOrDefault(x=> x.interactionEnum == interactionEnum);

        if(interaction == null) 
        {
            Debug.LogError($"Not proper interaction displayer {interactionEnum}");
            return;
        }

        StartCoroutine(ShowInteractionCoroutine(interaction));

        _doneInteractions.Add(interaction);
        _availableInteractions.Remove(interaction);
    }

    private IEnumerator ShowInteractionCoroutine(NpcInteractions interaction)
    {
        _isInteractionPlaying = true;
        foreach (var text in interaction.dialogue)
        {
            yield return StartCoroutine(DialoguePanel.DialoguePanelInstance.ShowDialogue(new DialoguePanel.DialogueData( text, timeForDisplayingDialogueMS, dialogueColor)));
        }
        _isInteractionPlaying = false;
    }
}

[Serializable]
public class NpcInteractions
{
    public InteractionEnum interactionEnum= InteractionEnum.None;
    public string startingPrompt;
    public List<string> dialogue;

    public InteractionBlocker interactionBlocker;
}

[Serializable]
public class InteractionBlocker
{
    public InteractionEnum interactionEnum;
    public GameEventsEnum gameEventsEnum;
}

public enum InteractionEnum
{
    None,
}
