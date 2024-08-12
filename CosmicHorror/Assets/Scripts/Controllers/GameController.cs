using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Action<string> OnLetterAdded;

    public static GameController GameControllerInstance;
    public LettersConfig LettersConfig;
    public ItemsConfig ItemsConfig;
    public List<string> knownLetters = new List<string>();
    public List<GameEventsEnum> events = new List<GameEventsEnum>();


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (GameController.GameControllerInstance == null)
        {
            GameController.GameControllerInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        events.Remove(GameEventsEnum.GoIntoCave);
    }

    public void AddLetter(string letter)
    {
        knownLetters.Add(letter);

        if(knownLetters.Count == 3)
        {
            AddEvent(GameEventsEnum.ThreeLettersKnown);
        }

        OnLetterAdded?.Invoke(letter);
    }


    public void AddEvent(GameEventsEnum eventsEnum)
    {
        if (!events.Contains(eventsEnum))
        {
            events.Add(eventsEnum);
        }
    }

    public bool WasEventDone(GameEventsEnum eventsEnum)
    {
        if(events.Contains(eventsEnum))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public enum GameEventsEnum
    {
        None = 0,
        FlintHistory,
        KilledFirstEnemy,
        RespawnedForFirstTime,
        PlantHistory,
        AppleCollected,
        GoIntoCave,
        WasAppleGiven,
        ThreeLettersKnown,
        SeenMutant,
        Finish,
        RespawnedSecondTime,
        Magazine
    }
}
