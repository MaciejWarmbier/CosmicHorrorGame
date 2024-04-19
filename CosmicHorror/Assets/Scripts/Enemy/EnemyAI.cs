using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public long MaxEnemyHealth = 100;
    public long EnemyHealth {  get; private set; }

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform target;

    private void Start()
    {
        EnemyHealth = MaxEnemyHealth;
    }

    protected virtual void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void TakeDamage(int damage)
    {
        EnemyHealth -= damage;

        if(EnemyHealth < 0)
        {
            Destroy(gameObject);
        }
    }
}
