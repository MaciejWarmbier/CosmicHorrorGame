using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] int weaponDamage = 20;
    [SerializeField] GameObject explosion;

    ParticleSystem ParticleSystem;
    List<ParticleCollisionEvent> CollisionEvents = new();

    private void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
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
            StartCoroutine(enemy.TakeDamage(weaponDamage));
        }
    }

    public void ShootParticle()
    {
        ParticleSystem.Play();
    }
}
