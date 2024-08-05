using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player PlayerlInstance = null;

    [SerializeField] InputManager inputManager;
    [SerializeField] MovementController movementController;
    [SerializeField] LookController lookController;

    [SerializeField] PlayerStatistics playerStatistics;
    [SerializeField] WeaponPlayer Machete;
    [SerializeField] WeaponPlayer Revolver;

    private WeaponPlayer chosenWeapon;

    private void Start()
    {
        PlayerlInstance = this;
        chosenWeapon = Machete;
    }

    public void SwitchWeapon()
    {
        chosenWeapon.gameObject.SetActive(false);

        if (chosenWeapon.weaponEnum == WeaponPanel.WeaponEnum.Machete)
        {
            chosenWeapon = Revolver;
        }
        else
        {
            chosenWeapon = Machete;
        }

        chosenWeapon.gameObject.SetActive(true);
        WeaponPanel.WeaponPanelInstance.OnWeaponSwap(chosenWeapon.weaponEnum);
    }

    public void ShootWeapon()
    {
        chosenWeapon.Attack();
    }

    public void Reload()
    {
        chosenWeapon.Reload();
    }

    public void MovePlayer(Transform pushTransform)
    {
        if (pushTransform != null)
        {
            StartCoroutine(movementController.MovePlayer(pushTransform));
        }
    }
}
