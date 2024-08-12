using UnityEngine;
using static GameController;

public class LetterInteractable : InteractableAction
{
    [SerializeField] string letter;
    [SerializeField] AudioSource letterAudio;

    public override void PlayAction()
    {
        if (GameController.GameControllerInstance.knownLetters.Contains(letter))
        {
            DialoguePanel.DialoguePanelInstance.ShowDialogue(new DialoguePanel.DialogueData("Pamiętam ten znak...", 2, Color.red, GameEventsEnum.None));
        }else
        {
            GameController.GameControllerInstance.AddLetter(letter);
            letterAudio.Play();
            PlayerStatistics.PlayerStatisticslInstance.ChangeStress(1);
        }
    }
}
