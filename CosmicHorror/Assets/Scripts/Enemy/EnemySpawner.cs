using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform spawnerRT;
    [SerializeField] EnemyAI enemyToSpawn;
    public float spawnDelay;

    public EnemyAI Spawn()
    {
        var enemy = Instantiate(enemyToSpawn, spawnerRT.position, Quaternion.identity);

        return enemy;
    }
}
