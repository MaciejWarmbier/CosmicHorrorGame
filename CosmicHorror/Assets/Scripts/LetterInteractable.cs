using UnityEngine;

public class LetterInteractable : InteractableAction
{
    [SerializeField] string letter;

    public override void PlayAction()
    {
        GameController.GameControllerInstance.AddLetter(letter);
    }
}
