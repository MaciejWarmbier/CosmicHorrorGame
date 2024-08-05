using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponProjectile : WeaponPlayer
{
    [SerializeField] Transform WeaponMain;
    [SerializeField] Transform Drum;
    [SerializeField] Transform Trigger;

    [SerializeField] int weaponDamage = 10;
    [SerializeField] int weaponAmmo = 5;
    [SerializeField] float weaponCooldown = 1f;
    [SerializeField] float weaponReload = 1.5f;
    [SerializeField] float pushPower = 0.5f;
    [SerializeField] GameObject explosion;
    [SerializeField] ParticleSystem ParticleSystem;
    [SerializeField] AudioSource AudioSource;

    List<ParticleCollisionEvent> CollisionEvents = new();
    Sequence mainSequence;
    Sequence drumSequence;
    Sequence triggerSequence;

    bool isWeaponOnCooldown = false;
    bool isWeaponOnRealod = false;

    private void OnEnable()
    {
        isWeaponOnCooldown = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        int events = ParticleSystem.GetCollisionEvents(other, CollisionEvents);

        for(int i = 0; i < events; i++)
        {
            var explosionGO= Instantiate(explosion, CollisionEvents[i].intersection, Quaternion.LookRotation(CollisionEvents[i].normal));
            var expParticles= explosionGO.GetComponent<ParticleSystem>();

            Destroy(explosionGO, expParticles.main.duration);
        }

        if(other.TryGetComponent(out EnemyAI enemy))
        {
            if(enemy != null)
            {
                StartCoroutine(enemy.TakeDamage(pushPower, weaponDamage));
            }
        }
    }

    public override void Attack()
    {
        if(PlayerStatistics.PlayerStatisticslInstance.RevolverLoadedAmmo > 0)
        {
            if (!isWeaponOnCooldown && !isWeaponOnRealod)
            {
                isWeaponOnCooldown = true;
                StartCoroutine(WeaponCooldown());
                ParticleSystem.Play();
                PlayShootAnimation();
                AudioSource.clip = MusicPanel.MusicPanelInstance.SoundConfig.GetAudioClip(SoundConfig.AudioEnum.shot);
                AudioSource.Play();
                PlayerStatistics.PlayerStatisticslInstance.UseRevolver();
            }
        }
        else
        {
            Reload();
        }
    }

    public override void Reload()
    {
        base.Reload();

        if (PlayerStatistics.PlayerStatisticslInstance.RevolverAmmo > 0
            && PlayerStatistics.PlayerStatisticslInstance.GetReloadValue() > 0)
        {
            if (!isWeaponOnRealod && !isWeaponOnCooldown)
            {
                isWeaponOnRealod = true;
                StartCoroutine(WeaponReload());
                PlayReloadAnimation();
                AudioSource.clip = MusicPanel.MusicPanelInstance.SoundConfig.GetAudioClip(SoundConfig.AudioEnum.reload);
                AudioSource.Play();
            }
        }
        else
        {
            AudioSource.clip = MusicPanel.MusicPanelInstance.SoundConfig.GetAudioClip(SoundConfig.AudioEnum.empty_gun);
            AudioSource.Play();
        }
    }

    private IEnumerator WeaponReload()
    {
        yield return new WaitForSeconds(weaponReload);
        isWeaponOnRealod = false;
        PlayerStatistics.PlayerStatisticslInstance.ReloadRevolver();
    }

    private IEnumerator WeaponCooldown()
    {
        yield return new WaitForSeconds(weaponCooldown);
        isWeaponOnCooldown = false;
    }

    private void PlayShootAnimation()
    {
        drumSequence?.Kill();
        mainSequence?.Kill();
        triggerSequence?.Kill();

        drumSequence = DOTween.Sequence();
        mainSequence = DOTween.Sequence();
        triggerSequence = DOTween.Sequence();

        var newDrumRotation = new Vector3(Drum.localEulerAngles.x, Drum.localEulerAngles.y, Drum.localEulerAngles.z - 60);

        drumSequence.Append(Drum.DOLocalRotate(newDrumRotation, weaponCooldown));

        var originalRotationMain = WeaponMain.localEulerAngles;
        var newRotationMain = WeaponMain.localEulerAngles + new Vector3(-50, 0, 0);

        mainSequence.Append(WeaponMain.DOLocalRotate(newRotationMain, weaponCooldown / 2))
            .Append(WeaponMain.DOLocalRotate(originalRotationMain, weaponCooldown / 2));
        mainSequence.SetEase(Ease.OutExpo);

        var originalRotationTrigger = WeaponMain.localEulerAngles;
        var newRotationTrigger = WeaponMain.localEulerAngles + new Vector3(-9, 0, 0);

        triggerSequence.Append(Trigger.DOLocalRotate(newRotationTrigger, weaponCooldown / 2))
            .Append(Trigger.DOLocalRotate(originalRotationTrigger, weaponCooldown / 2));
        triggerSequence.SetEase(Ease.OutBack);

        triggerSequence.Play();
        mainSequence.Play();
        drumSequence.Play();
    }

    private void PlayReloadAnimation()
    {
        mainSequence?.Kill();

        mainSequence = DOTween.Sequence();

        var originalRotationMain = WeaponMain.localEulerAngles;
        var newRotationMain = WeaponMain.localEulerAngles + new Vector3(70, 0, 0);

        mainSequence.Append(WeaponMain.DOLocalRotate(newRotationMain, weaponReload *3/4))
            .Append(WeaponMain.DOLocalRotate(originalRotationMain, weaponCooldown *1/4));
        mainSequence.SetEase(Ease.Linear);

        mainSequence.Play();
    }
}
