using System;
using System.Collections;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    public WeaponState weaponState;
    public Action EnemyHit;
    public Action OnAttack;
    public Action OnStartCharge;

    [SerializeField] AudioSource attackAudio;
    public float AttackSpeed;
    public float AttackCooldown;
    public float AttackCharging;

    private void Awake()
    {
        weaponState = WeaponState.Normal;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (weaponState == WeaponState.Charged)
        {
            if (collision.transform.CompareTag("Player"))
            {
                attackAudio.Play();
                EnemyHit?.Invoke();
                StartCoroutine(Cooldown());
                Debug.Log("Enemy Hit");
            }
        }
    }
    
    public IEnumerator Cooldown()
    {
        if(weaponState != WeaponState.Cooldown)
        {
            OnAttack?.Invoke();
            weaponState = WeaponState.Cooldown;
            yield return new WaitForSeconds(AttackCooldown);
            weaponState = WeaponState.Normal;
        }
    }

    public IEnumerator Charging()
    {
        if(weaponState == WeaponState.Normal)
        {
            OnStartCharge?.Invoke();
            weaponState = WeaponState.Charging;
            yield return new WaitForSeconds(AttackCharging);
            weaponState = WeaponState.Charged;
            StartCoroutine(AttackTime());
        }
    }

    public IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(AttackSpeed);
        StartCoroutine(Cooldown());
    }

    public enum WeaponState
    {
        Charging,
        Cooldown,
        Charged,
        Normal
    }
}
