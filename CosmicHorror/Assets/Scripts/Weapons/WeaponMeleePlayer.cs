using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponMeleePlayer : WeaponPlayer
{
    [SerializeField] Transform WeaponMain;
    [SerializeField] PlayerCollider attackCollider;
    [SerializeField] int weaponDamage = 10;
    [SerializeField] float weaponCooldown = 1f;
    [SerializeField] GameObject explosion;

    Sequence mainSequence;

    bool isWeaponOnCooldown = false;


    private void Start()
    {
        attackCollider.SetCollider(false);
        attackCollider.HandleOnTriggerStay += OnOpponentHit;
    }

    private void OnOpponentHit(Collider collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyAI enemy))
            {
                if (enemy != null)
                {
                    StartCoroutine(enemy.TakeDamage(weaponDamage));
                }
            }
        }
    }

    public override void Attack()
    {
        if(!isWeaponOnCooldown)
        {
            isWeaponOnCooldown =true;
            StartCoroutine(WeaponCooldown());
            StartCoroutine(ActivateCollider());
            PlayAttackAnimation();
        }
    }

    private IEnumerator ActivateCollider()
    {
        attackCollider.SetCollider(true);
        yield return new WaitForSeconds(weaponCooldown/2);
        attackCollider.SetCollider(false);
    }

    private IEnumerator WeaponCooldown()
    {
        yield return new WaitForSeconds(weaponCooldown);
        isWeaponOnCooldown = false;
    }

    private void PlayAttackAnimation()
    {
        mainSequence?.Kill();
        mainSequence = DOTween.Sequence();

        var originalRotationMain = WeaponMain.localEulerAngles;
        var originalPositionMain = WeaponMain.localPosition;
        var newRotationMain = new Vector3(0.637f, -9.168f, 130.592f);
        var newPositionMain = new Vector3(0.553f, -0.64f, 2.232f);

        mainSequence.Append(WeaponMain.DOLocalRotate(newRotationMain, weaponCooldown))
            .Join(WeaponMain.DOLocalMove(newPositionMain, weaponCooldown))
            .Append(WeaponMain.DOLocalRotate(originalRotationMain, weaponCooldown ))
            .Join(WeaponMain.DOLocalMove(originalPositionMain, weaponCooldown));
        mainSequence.SetEase(Ease.OutExpo);

        mainSequence.Play();
    }
}
