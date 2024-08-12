using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GridBrushBase;

public class EnemyAITeleporter : EnemyAI
{
    [SerializeField] private int teleportCooldownMin = 3000;
    [SerializeField] private int teleportCooldownMax = 7000;
    [SerializeField] AudioSource teleportSound;

    private bool _isTeleportOnCooldown = false;

    protected override void Start()
    {
        base.Start();

        _isTeleportOnCooldown = true;
        WaitForTeleport();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

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

        Quaternion myRotation = Quaternion.AngleAxis(-30, direction.normalized);
        Vector3 randomPoint = this.transform.position + myRotation * direction.normalized*20 ;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 20.0f, NavMesh.AllAreas))
        {
            this.transform.position = hit.position;
            teleportSound.Play();
            WaitForTeleport();
        }
        else
        {
            _isTeleportOnCooldown = false;
        }
    }

    private async void WaitForTeleport()
    {
        await Task.Delay(Random.Range(teleportCooldownMin, teleportCooldownMax));

        _isTeleportOnCooldown = false;
    }
}
