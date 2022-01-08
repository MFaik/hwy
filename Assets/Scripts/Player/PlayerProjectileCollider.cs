using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireLayer("ProjectileCollider")]
public class PlayerProjectileCollider : MonoBehaviour
{
    List<Transform> m_projectiles = new List<Transform>();

    void OnTriggerEnter2D(Collider2D other) {
        m_projectiles.Add(other.transform);
    }

    void OnTriggerExit2D(Collider2D other) {
        m_projectiles.Remove(other.transform);
    }

    public Transform GetProjectile() {
        Transform closest = null;
        foreach(Transform projectile in m_projectiles){
            if(!closest){
                closest = projectile;
                continue;
            }
            if((transform.position-closest.position).sqrMagnitude == 0 
            || (transform.position-closest.position).sqrMagnitude > (transform.position-projectile.position).sqrMagnitude)
                closest = projectile;
        }
        return closest;
    }
}
