using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage = 1f;

    [SerializeField] GameObject Self;

    [SerializeField] GameObject DestroyParticle;
    [SerializeField] float TimeToLive = 10;
    float m_healthTimer = 0;

    void Update() {
        if(m_healthTimer >= TimeToLive)
            DestroySelf();
        else 
            m_healthTimer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        //????
    }
    
    public void DestroySelf() {
        if(DestroyParticle)
            Instantiate(DestroyParticle,transform.position,transform.rotation);
        if(!Self)
            Destroy(gameObject);
        else 
            Destroy(Self);
    }
}
