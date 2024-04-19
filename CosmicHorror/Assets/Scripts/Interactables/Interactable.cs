using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] List<InteractableAction> actionList;
    [SerializeField] int timesToUse = 0;
    [SerializeField] int cooldownTimeMS = 2000;

    private int wasHitTimes = 0;
    private bool isOnCooldown = false;

    public void OnRaycastHit()
    {
        if (!isOnCooldown && (wasHitTimes == 0 || wasHitTimes < timesToUse))
        {
            foreach (InteractableAction action in actionList)
            {
                action.PlayAction();
            }

            wasHitTimes++;
        }
    }
}
