using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static ItemsConfig;

public class EnemyAI : MonoBehaviour
{
    public Action<EnemyAI> OnEnemyDeath;

    [Header ("Statisytics")]
    public long MaxEnemyHealth = 100;
    public long EnemyHealth {  get; private set; }
    public long Damage;
    public int Stress;
    public long Speed = 7;
    [SerializeField] List<RandomItemData> lootData;

    [Header("Mechanics")]
    public Color DamagedColor;
    public float damagedSeconds = 0.5f;
    public Color SlowedColor;

    [Header("Sprites")]
    [SerializeField] float changeSpriteSeconds = 0.3f;
    [SerializeField] protected Sprite normalLeft;
    [SerializeField] protected Sprite normalRight;
    [SerializeField] protected Sprite attackLeft;
    [SerializeField] protected Sprite attackRight;


    [Header("Components")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Transform target;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected WeaponMelee weaponMelee;

    private bool hasTakenDamage = false;
    private bool isAnimationOnCooldown = false;
    private bool isAttacking = false;
    private bool isLeft = false;

    protected virtual void Start()
    {
        target = Player.PlayerlInstance.transform;
        EnemyHealth = MaxEnemyHealth;
        hasTakenDamage = false;
        isLeft = false;
        isAnimationOnCooldown = false; 
        isAttacking = false;

        agent.speed = Speed;

        weaponMelee.EnemyHit += () =>OnHit();
        weaponMelee.OnAttack += () => { isAttacking = false; };
        weaponMelee.OnStartCharge += () => { isAttacking = true; };
    }

    protected virtual void FixedUpdate()
    {
        if(target != null)
        {
            if(agent.enabled)
            {
                agent.SetDestination(target.position);
            }
        }

        if(!isAnimationOnCooldown)
        {
            StartCoroutine(ChangeSprite());
        }

        if(Vector3.Distance(target.position, transform.position) < 10)
        {
            StartCoroutine(weaponMelee.Charging());
        }
    }

    private IEnumerator ChangeSprite() 
    {
        if (!isAnimationOnCooldown)
        {
            isAnimationOnCooldown = true;

            if (isAttacking)
            {
                if (isLeft)
                {
                    spriteRenderer.sprite = attackLeft;
                }
                else
                {
                    spriteRenderer.sprite = attackRight;
                }
            }
            else
            {
                if (isLeft)
                {
                    spriteRenderer.sprite = normalLeft;
                }
                else
                {
                    spriteRenderer.sprite = normalRight;
                }
            }
            
            yield return new WaitForSeconds(changeSpriteSeconds);

            isLeft = !isLeft;
            isAnimationOnCooldown = false;
        }
    }

    public void TakeDamage(float pushPower, int damage)
    {
        StartCoroutine(TakeDamageCoroutine(pushPower, damage));
    }

    private IEnumerator TakeDamageCoroutine(float pushPower, int damage)
    {
        if(!hasTakenDamage)
        {
            hasTakenDamage = true;

            EnemyHealth -= damage;

            if (EnemyHealth < 0)
            {
                SpawnLoot();
                Destroy(gameObject);
                OnEnemyDeath?.Invoke(this);
                yield return null;
            }
            else
            {
                spriteRenderer.color = DamagedColor;

                rb.isKinematic = false;
                agent.enabled = false;
                rb.AddForce(-transform.forward.normalized * pushPower, ForceMode.Impulse);
                StartCoroutine(StopPushing());
                

                yield return new WaitForSeconds(damagedSeconds);

                spriteRenderer.color = Color.white;
                hasTakenDamage = false;

            }
        }
    }

    private void SpawnLoot()
    {
        if (lootData != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, 100);
            var itemEnum = lootData.FirstOrDefault(x => x.chanceMax > randomIndex && x.chanceMin <= randomIndex).itemEnum;

            if (itemEnum != ItemsEnum.None)
            {
                var prefab = GameController.GameControllerInstance.ItemsConfig.GetItem(itemEnum);

                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator StopPushing()
    {
        yield return new WaitForSeconds(damagedSeconds);

        rb.isKinematic = true;
        agent.enabled = true;
    }

    private void OnHit()
    {
        PlayerStatistics.PlayerStatisticslInstance.ChangeHealth(-Damage);
        PlayerStatistics.PlayerStatisticslInstance.ChangeStress(Stress);
        Player.PlayerlInstance.MovePlayer(transform);
    }
}
