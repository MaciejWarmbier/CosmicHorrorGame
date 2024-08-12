using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameController;

public class NPC : MonoBehaviour
{
    public static NPC NPCInstance;
    [SerializeField] float timeForDisplayingDialogue = 3;
    [SerializeField] Color dialogueColor;
    [SerializeField] List<NpcInteractions> interactions;
    [SerializeField] GameObject apple;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip npcAudioNormal;
    [SerializeField] AudioClip npcAudioNot1;
    [SerializeField] AudioClip npcAudioNot2;
    [SerializeField] Sprite npcNormal;
    [SerializeField] Sprite npcNormalSpeaking;
    [SerializeField] Sprite npcNormalNot1;
    [SerializeField] Sprite npcNormalNot1Speaking;
    [SerializeField] Sprite npcNormalNot2;
    [SerializeField] Sprite npcNormalNot2Speaking;
    [SerializeField] SpriteRenderer npcImage;

    private Sprite notSpeaking;
    private Sprite speaking;
    private AudioClip npcAudio;
    private List<NpcInteractions> _doneInteractions = new();
    private List<NpcInteractions> _pendingInteractions = new();
    public List<string> _pendingInteractionsFirstFruit = new();
    private List<NpcInteractions> _availableInteractions = new();
    private bool _isInteractionPlaying = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (NPC.NPCInstance == null)
        {
            NPC.NPCInstance = this;
            _pendingInteractions = interactions;
            notSpeaking = npcNormal;
            speaking = npcNormalSpeaking; 
            npcAudio = npcAudioNormal;
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

        if (GameController.GameControllerInstance.WasEventDone(GameEventsEnum.WasAppleGiven)
            && GameController.GameControllerInstance.WasEventDone(GameEventsEnum.FlintHistory)
            && GameController.GameControllerInstance.WasEventDone(GameEventsEnum.PlantHistory))
        {
            notSpeaking = npcNormalNot2;
            speaking = npcNormalNot2Speaking;
            npcAudio = npcAudioNot2;
            GameController.GameControllerInstance.AddEvent(GameEventsEnum.Finish);
        }
        else if (GameController.GameControllerInstance.WasEventDone(GameEventsEnum.WasAppleGiven))
        {
            interaction.dialogue.Insert(0, _pendingInteractionsFirstFruit[UnityEngine.Random.Range(0, _pendingInteractionsFirstFruit.Count)]);
        }

        if(interaction.interactionEnum == InteractionEnum.GiveApple)
        {
            notSpeaking = npcNormalNot1;
            speaking = npcNormalNot1Speaking;
            npcAudio = npcAudioNot1;
            apple.SetActive(true);
            GameController.GameControllerInstance.AddEvent(GameEventsEnum.WasAppleGiven);
            apple.SetActive(true);
        }
        

        StartCoroutine(ShowInteractionCoroutine(interaction));

        _doneInteractions.Add(interaction);
        _availableInteractions.Remove(interaction);
    }

    private IEnumerator ShowInteractionCoroutine(NpcInteractions interaction)
    {
        npcImage.sprite = notSpeaking;

        _isInteractionPlaying = true;

        audioSource.clip = npcAudio;
        audioSource.Play();
        foreach (var text in interaction.dialogue)
        {
            npcImage.sprite = speaking;
            yield return StartCoroutine(DialoguePanel.DialoguePanelInstance.ShowDialogueCoroutine(new DialoguePanel.DialogueData( text, timeForDisplayingDialogue, interaction.dialogueColor, GameEventsEnum.None)));
            npcImage.sprite = notSpeaking;
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
    public Color dialogueColor;

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
    Thot,
    HowAmIAlive,
    AmIImmortal,
    StrangePulsing,
    Mutants,
    GiveApple,
    IsEverythingAlright,
    SeenFlint,
    SeenPlant,
    Finish,
    Magazine,
    Alphabet
}
