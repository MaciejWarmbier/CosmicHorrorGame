using UnityEngine;

public class InteractableActionChangeHealth : InteractableAction
{
    [SerializeField] long health;

    public override void PlayAction()
    {
        PlayerStatistics.PlayerStatisticslInstance.ChangeHealth(health);
    }
}
