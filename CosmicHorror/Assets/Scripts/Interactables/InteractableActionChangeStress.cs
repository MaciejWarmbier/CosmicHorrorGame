using UnityEngine;

public class InteractableActionChangeStress : InteractableAction
{
    [SerializeField] int stress;

    public override void PlayAction()
    {
        PlayerStatistics.PlayerStatisticslInstance.ChangeStress(stress);
    }
}
