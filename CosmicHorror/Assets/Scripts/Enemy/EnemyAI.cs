using System.Collections;
using System.Threading.Tasks;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header ("Statisytics")]
    public long MaxEnemyHealth = 100;
    public long EnemyHealth {  get; private set; }
    public long AttackSpeed;
    public long Damage;
    public long Stress;
    public long Speed = 7;

    [Header("Mechanics")]
    public Color DamagedColor;
    public float damagedSeconds = 0.5f;
    public float attackCooldown = 0.5f;
    public Color SlowedColor;

    [Header("Sprites")]
    [SerializeField] float changeSpriteSeconds = 0.3f;
    [SerializeField] protected Sprite normalLeft;
    [SerializeField] protected Sprite normalRight;
    [SerializeField] protected Sprite attackLeft;
    [SerializeField] protected Sprite attackRight;


    [Header("Components")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform target;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    private bool hasTakenDamage = false;
    private bool isAnimationOnCooldown = false;
    private bool isAttacking = false;
    private bool isLeft = false;

    private void Start()
    {
        EnemyHealth = MaxEnemyHealth;
        hasTakenDamage = false;
        isLeft = false;
        isAnimationOnCooldown = false;

        agent.speed = Speed;
    }

    protected virtual void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
        }

        if(!isAnimationOnCooldown)
        {
            StartCoroutine(ChangeSprite());
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

    public IEnumerator TakeDamage(int damage)
    {
        if(!hasTakenDamage)
        {
            hasTakenDamage = true;

            EnemyHealth -= damage;

            if (EnemyHealth < 0)
            {
                Destroy(gameObject);
                yield return null;
            }
            else
            {
                spriteRenderer.color = DamagedColor;

                yield return new WaitForSeconds(damagedSeconds);

                spriteRenderer.color = Color.white;
                hasTakenDamage = false;
            }
        }
    }
}
