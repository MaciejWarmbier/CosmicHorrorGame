using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour
{
    [SerializeField] Collider Collider;
    [SerializeField] string TagCollider;
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] List<GameObject> doors;
    [SerializeField] List<GameObject> otherRoomColliders;
    [SerializeField] int stressDeletion;
    [SerializeField] bool isStarting = false;

    private List<EnemyAI> spawnedEnemies = new();
    bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(TagCollider) && !isUsed)
        {
            foreach (EnemySpawner enemySpawner in enemySpawners)
            {
                StartCoroutine(SpawnEnemy(enemySpawner));
            }

            foreach (GameObject door in doors)
            {
                door.SetActive(true);
            }

            foreach(GameObject otherRoom in otherRoomColliders)
            {
                otherRoom.SetActive(false);
            }

            if (isStarting)
            {
                DialoguePanel.DialoguePanelInstance.HideDialogueOptions();
            }

            isUsed = true;
            if (isStarting && GameController.GameControllerInstance.WasEventDone(GameController.GameEventsEnum.RespawnedForFirstTime))
            {
                CaveController.CaveControllerInstance.PutShaderOnFirstCooldown();
            }
        }
    }

    private void CheckForEndRoom(EnemyAI enemyAI)
    {
        spawnedEnemies.Remove(enemyAI);

        if (spawnedEnemies.Count == 0)
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }

            PlayerStatistics.PlayerStatisticslInstance.ChangeStress(-stressDeletion);
        }
    }

    private IEnumerator SpawnEnemy(EnemySpawner enemySpawner)
    {
        yield return new WaitForSeconds(enemySpawner.spawnDelay);
        var enemy = enemySpawner.Spawn();
        spawnedEnemies.Add(enemy);
        enemy.OnEnemyDeath += CheckForEndRoom;
    }
}
