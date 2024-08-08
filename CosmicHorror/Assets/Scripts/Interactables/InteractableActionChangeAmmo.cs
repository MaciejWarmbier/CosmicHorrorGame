using UnityEngine;

public class InteractableActionChangeAmmo : InteractableAction
{
    [SerializeField] int ammo;

    public override void PlayAction()
    {
        PlayerStatistics.PlayerStatisticslInstance.ChangeAmmo(ammo);
    }
}
