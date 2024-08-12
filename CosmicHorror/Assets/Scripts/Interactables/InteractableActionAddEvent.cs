using UnityEngine;
using static GameController;

public class InteractableActionAddEvent : InteractableAction
{
    [SerializeField] GameEventsEnum gameEvent;

    public override void PlayAction()
    {
        GameController.GameControllerInstance.AddEvent(gameEvent);
    }
}
