using UnityEngine;

public class LetterInteractable : InteractableAction
{
    [SerializeField] string letter;
    [SerializeField] AudioSource letterAudio;

    public override void PlayAction()
    {
        GameController.GameControllerInstance.AddLetter(letter);
        letterAudio.Play();
    }
}
