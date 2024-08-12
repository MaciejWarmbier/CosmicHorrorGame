using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class Interactable : MonoBehaviour
{
    [SerializeField] List<InteractableAction> actionList;
    [SerializeField] GameEventsEnum gameEvent;
    [SerializeField] bool deleteIfEvent = true;
    [SerializeField] int timesToUse = 1;
    [SerializeField] int cooldownTimeMS = 2000;

    public bool CanBeInteracted => !isOnCooldown && (wasHitTimes == 0 || wasHitTimes < timesToUse || timesToUse == 0);

    private int wasHitTimes = 0;
    private bool isOnCooldown = false;

    private void Start()
    {
        if (gameEvent != GameEventsEnum.None && GameController.GameControllerInstance.WasEventDone(gameEvent) && deleteIfEvent)
        {
            if(deleteIfEvent)
            {
                Destroy(this.gameObject);
            }
            else
            {
                isOnCooldown = true;
            }
        }
    }

    public void OnRaycastHit()
    {
        if (!isOnCooldown && (wasHitTimes == 0 || wasHitTimes < timesToUse || timesToUse == 0))
        {
            foreach (InteractableAction action in actionList)
            {
                action.PlayAction();
            }

            wasHitTimes++;
        }
    }
}
