using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LettersConfig", menuName = "ScriptableObjects/LettersConfig", order = 1)]
public class LettersConfig : ScriptableObject
{
    public List<LetterData> letterData;

    [Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite letterSprite;
        public Sprite letterNewSprite;
    }
}
