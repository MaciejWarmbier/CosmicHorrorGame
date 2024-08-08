using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Action<string> OnLetterAdded;

    public static GameController GameControllerInstance = null;
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
    }

    public void AddLetter(string letter)
    {
        knownLetters.Add(letter);
        OnLetterAdded?.Invoke(letter);
    }


    public void AddEvent(GameEventsEnum eventsEnum)
    {
        events.Add(eventsEnum);
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
        FlintHistory,
        KilledFirstEnemy,
        RespawnedForFirstTime
    }
}
