using System.Linq;
using UnityEngine;
using static LettersConfig;

public class ChangeLetterScriptSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] string letter;
    [SerializeField] AudioSource whisperAudio;

    private LetterData _letterData;

    public void Start()
    {
        _letterData = GameController.GameControllerInstance.LettersConfig.letterData.FirstOrDefault(x => x.letter == letter);

        if (GameController.GameControllerInstance.knownLetters.Contains(letter))
        {
            spriteRenderer.sprite = _letterData.letterSprite;
            if(whisperAudio != null)
            {
                whisperAudio.Stop();
            }
        }
        else
        {
            spriteRenderer.sprite = _letterData.letterNewSprite;
            if (whisperAudio != null)
            {
                whisperAudio.Play();
            }
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
