using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Action<EnemyAI> OnEnemyDeath;

    [Header ("Statisytics")]
    public long MaxEnemyHealth = 100;
    public long EnemyHealth {  get; private set; }
    public float AttackSpeed;
    public long Damage;
    public long Stress;
    public long Speed = 7;

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

        weaponMelee.EnemyHit += () => StartCoroutine(WeaponCooldown());
    }

    protected virtual void Update()
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
            isAttacking = true;
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

    public IEnumerator TakeDamage(float pushPower, int damage)
    {
        if(!hasTakenDamage)
        {
            hasTakenDamage = true;

            EnemyHealth -= damage;

            if (EnemyHealth < 0)
            {
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

    private IEnumerator StopPushing()
    {
        yield return new WaitForSeconds(damagedSeconds);

        rb.isKinematic = true;
        agent.enabled = true;
    }

    private IEnumerator WeaponCooldown()
    {
        isAttacking = false;
        PlayerStatistics.PlayerStatisticslInstance.ChangeHealth(-Damage);
        Player.PlayerlInstance.MovePlayer(transform);
        yield return new WaitForSeconds(AttackSpeed);
        weaponMelee.SetupCooldown(false);
        isAttacking = true;
    }
}
