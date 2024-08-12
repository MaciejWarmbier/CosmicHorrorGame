using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameController;
using static SoundConfig;

public class NPC : MonoBehaviour
{
    public static NPC NPCInstance;
    [SerializeField] float timeForDisplayingDialogue = 3;
    [SerializeField] Color dialogueColor;
    [SerializeField] List<NpcInteractions> interactions;
    [SerializeField] List<NpcInteractions> intros;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioEnum> npcAudio;


    private List<NpcInteractions> _doneInteractions = new();
    private List<NpcInteractions> _pendingInteractions = new();
    private List<NpcInteractions> _availableInteractions = new();
    private bool _isInteractionPlaying = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (NPC.NPCInstance == null)
        {
            NPC.NPCInstance = this;
            _pendingInteractions = interactions;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        CalculateDialogues();
    }

    private void CalculateDialogues()
    {
        foreach (var interaction in _pendingInteractions)
        {
            bool isNotWaitingForOtherInteraction =
                interaction.interactionBlocker.interactionEnum == InteractionEnum.None 
                || _doneInteractions.Exists(x => x.interactionEnum == interaction.interactionBlocker.interactionEnum);
           
            
            bool isNotWaitingForEvent = interaction.interactionBlocker.gameEventsEnum == GameEventsEnum.None || GameController.GameControllerInstance.WasEventDone(interaction.interactionBlocker.gameEventsEnum);

            if (isNotWaitingForOtherInteraction && isNotWaitingForEvent)
            {
                _availableInteractions.Add(interaction);
            }
        }

        foreach(var interaction in _availableInteractions)
        {
            _pendingInteractions.Remove(interaction);
        }
    }

    public void StartDialogue()
    {
        CalculateDialogues();
        DialoguePanel.DialoguePanelInstance.ShowDialogueOptions(_availableInteractions.Select(x=>x.startingPrompt).ToList(), dialogueColor,ShowInteraction);
    }

    public void ShowInteraction(int interactionIndex)
    {
        DialoguePanel.DialoguePanelInstance.possibleDialogues = 0;
        ShowInteraction(_availableInteractions[interactionIndex-1].interactionEnum);
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
            yield return StartCoroutine(DialoguePanel.DialoguePanelInstance.ShowDialogueCoroutine(new DialoguePanel.DialogueData( text, timeForDisplayingDialogue, dialogueColor, GameEventsEnum.None)));
        }
        _isInteractionPlaying = false;
        InputManager.InputManagerInstance.EnableInputSystem();
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
    Welcome,
    YouAreBack,
    AdventurerLikeYou
}
