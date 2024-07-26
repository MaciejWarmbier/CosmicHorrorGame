using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Action<string> OnLetterAdded;

    public static GameController GameControllerInstance = null;
    public LettersConfig LettersConfig;
    public List<string> knownLetters = new List<string>();
    public bool isFlintHistoryVisited = false;


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (GameController.GameControllerInstance == null)
        {
            GameController.GameControllerInstance = this;

            isFlintHistoryVisited = false;
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
}
