using System;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    public Action EnemyHit;

    public bool _isWeaponOnCooldown= false;


    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && !_isWeaponOnCooldown)
        {
            EnemyHit?.Invoke();
            SetupCooldown(true);
            Debug.Log("Enemy Hit");
        }
    }

    public void SetupCooldown(bool setup)
    {
        _isWeaponOnCooldown = setup;
    }
}
