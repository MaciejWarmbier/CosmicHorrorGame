using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITeleporter : EnemyAI
{
    [SerializeField] private int teleportCooldown = 5000;

    private bool _isTeleportOnCooldown = false;

    private void Start()
    {
        _isTeleportOnCooldown = true;
        WaitForTeleport();
    }

    protected override void Update()
    {
        base.Update();

        if(target != null)
        {
            if(!_isTeleportOnCooldown)
            {
                Teleport();
            }
        }
    }

    private void Teleport()
    {
        _isTeleportOnCooldown = true;
        var direction = target.position - this.transform.position;
        direction.y = 0;

        Vector3 randomPoint = this.transform.position + direction.normalized*5;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
        {
            this.transform.position = hit.position;
            WaitForTeleport();
        }
        else
        {
            _isTeleportOnCooldown = false;
        }
    }

    private async void WaitForTeleport()
    {
        await Task.Delay(teleportCooldown);

        _isTeleportOnCooldown = false;
    }
}
