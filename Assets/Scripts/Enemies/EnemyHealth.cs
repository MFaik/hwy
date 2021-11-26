using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject Parent;
    [SerializeField] GameObject DestroyParticle;
    [SerializeField] float Health = 3f;
    public float Damage = 1f;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerBullet")){
            Projectile playerBulletProjectile = other.GetComponent<Projectile>();
            if(!playerBulletProjectile){
                Debug.LogError("PlayerBullet doesn't have Projectile script");
            } else {
                TakeDamage(playerBulletProjectile.Damage);
                playerBulletProjectile.DestroySelf();
            }
        } else if(other.CompareTag("EnemyDespawner")){
            GetDestroyed();
        }
    }

    void TakeDamage(float damage) {
        Health -=  damage;
        if(Health <= 0){
            GetDestroyed();
        }
    }

    void GetDestroyed() {
        if(DestroyParticle)
            Instantiate(DestroyParticle,transform.position,transform.rotation);
        
        if(Parent)
            Destroy(Parent);
        else 
            Destroy(gameObject);
    }
}
