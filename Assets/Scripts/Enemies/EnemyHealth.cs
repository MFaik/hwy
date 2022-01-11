using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject Parent;
    [SerializeField] GameObject DestroyParticle;
    [SerializeField] float Health = 3f;
    public float Damage = 1f;

    [SerializeField] bool CanDespawn = true;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerProjectile")){
            Projectile PlayerProjectileProjectile = other.GetComponent<Projectile>();
            if(!PlayerProjectileProjectile){
                Debug.LogError("PlayerProjectile doesn't have Projectile script");
            } else {
                TakeDamage(PlayerProjectileProjectile.Damage);
                PlayerProjectileProjectile.DestroySelf();
            }
        } else if(CanDespawn && other.CompareTag("EnemyDespawner")){
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
