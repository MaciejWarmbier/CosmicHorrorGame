using System.Threading.Tasks;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    [SerializeField] int weaponDamage = 20;
    [SerializeField] int damageCooldownMS = 2000;

    private bool _isWeaponOnCooldown= false;
    private bool _isTriggerOnCooldown = false;

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            WeaponCooldown();
            Debug.Log("Enemy Hit");
        }
    }

    private async void WeaponCooldown()
    {
        if (!_isWeaponOnCooldown)
        {
            _isWeaponOnCooldown = true;
            PlayerStatistics.PlayerStatisticslInstance.ChangeHealth(-weaponDamage);
            await Task.Delay(damageCooldownMS);

            _isWeaponOnCooldown = false;
        }
    }
}
