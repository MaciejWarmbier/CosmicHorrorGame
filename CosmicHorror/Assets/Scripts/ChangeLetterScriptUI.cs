using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static LettersConfig;

public class ChangeLetterScriptUI : MonoBehaviour
{
    [SerializeField] Image spriteRenderer;
    [SerializeField] string letter;

    private LetterData _letterData;

    public void Start()
    {
        _letterData = GameController.GameControllerInstance.LettersConfig.letterData.FirstOrDefault(x => x.letter == letter);

        if (GameController.GameControllerInstance.knownLetters.Contains(letter))
        {
            spriteRenderer.sprite = _letterData.letterSprite;
        }
        else
        {
            spriteRenderer.sprite = _letterData.letterNewSprite;
        }

        GameController.GameControllerInstance.OnLetterAdded += ChangeLetter;
    }

    public void ChangeLetter(string addedLetter)
    {
        if(letter == addedLetter)
        {
            spriteRenderer.sprite = _letterData.letterSprite;
        }
    }

    private void OnDestroy()
    {
        GameController.GameControllerInstance.OnLetterAdded -= ChangeLetter;
    }
}
