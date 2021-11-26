using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    float m_health;

    void Start() {
        m_health = MaxHealth;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if(!enemyHealth){
                Debug.LogError("Enemy doesn't have EnemyHealth script");
            } else {
                TakeDamage(enemyHealth.Damage);
            }
        } else if(other.CompareTag("EnemyProjectile")){
            Projectile enemyProjectile = other.GetComponent<Projectile>();
            if(!enemyProjectile){
                Debug.LogError("EnemyProjectile doesn't have Projectile script");
            } else {
                TakeDamage(enemyProjectile.Damage);
                enemyProjectile.DestroySelf();
            }
        }
    }

    void TakeDamage(float damage) {
        m_health -= damage;
    }
}
