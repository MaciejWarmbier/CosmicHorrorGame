using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    public Action EnemyHit;
    [SerializeField] AudioSource attackAudio;
    [SerializeField] float chargingTime;

    public bool _isWeaponOnCooldown= false;
    public bool _isWeaponCharged= false;


    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && !_isWeaponOnCooldown && _isWeaponCharged)
        {
            EnemyHit?.Invoke();
            SetupCooldown(true);
            attackAudio.Play();
            Debug.Log("Enemy Hit");
            _isWeaponCharged = false;
        }
    }

    public IEnumerator ChargeWeapon()
    {
        if (!_isWeaponOnCooldown)
        {
            yield return new WaitForSeconds(chargingTime);

            _isWeaponCharged = true;
        }
    }

    public void SetupCooldown(bool setup)
    {
        _isWeaponOnCooldown = setup;
    }
}
