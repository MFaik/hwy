using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 5;
    [HideInInspector] public int Health;

    [SerializeField] float IFrameTime = 1f;
    float lastHitTime;

    public UnityEvent<int> OnHealthChange;

    void Start() {
        Health = MaxHealth;
        lastHitTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if(!enemyHealth){
                Debug.LogError("Enemy doesn't have EnemyHealth script");
            } else {
                TakeDamage((int)enemyHealth.Damage);
            }
        } else if(other.CompareTag("EnemyProjectile")){
            Projectile enemyProjectile = other.GetComponent<Projectile>();
            if(!enemyProjectile){
                Debug.LogError("EnemyProjectile doesn't have Projectile script");
            } else {
                TakeDamage((int)enemyProjectile.Damage);
                enemyProjectile.DestroySelf();
            }
        }
    }

    void TakeDamage(int damage) {
        if(Time.time - lastHitTime > IFrameTime){
            lastHitTime = Time.time;
            Health -= damage;
            OnHealthChange.Invoke(Health);
            TimeManager.EditTime(0.5f,.5f);
        }
    }
}
