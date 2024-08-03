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
    [SerializeField] WeaponPlayer weapon;

    private void Start()
    {
        PlayerlInstance = this;
    }

    public void ShootWeapon()
    {
        weapon.Attack();
    }
}
