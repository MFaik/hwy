using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage = 1f;

    [SerializeField] GameObject Parent;

    [SerializeField] GameObject DestroyParticle;
    [SerializeField] float TimeToLive = 10;
    float m_healthTimer = 0;
    [SerializeField] bool DestroyOnGround = false;

    void Update() {
        if(m_healthTimer >= TimeToLive)
            DestroySelf();
        else 
            m_healthTimer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("EnemyDespawner"))
            DestroySelf();
        if(DestroyOnGround && other.CompareTag("Ground"))
            DestroySelf();
    }
    
    public void DestroySelf() {
        if(DestroyParticle)
            Instantiate(DestroyParticle,transform.position,transform.rotation);
        if(!Parent)
            Destroy(gameObject);
        else 
            Destroy(Parent);
    }
}
