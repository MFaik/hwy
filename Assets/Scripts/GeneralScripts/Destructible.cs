using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] float Duration = 10;
    [SerializeField] float ExplosionForce;
    [SerializeField] float ExplosionArcStartDegree;
    [SerializeField] float ExplosionArcEndDegree;

    public void GetDestroyed() {
        Invoke(nameof(GetDisabled),Duration);
        foreach(Rigidbody2D child in GetComponentsInChildren<Rigidbody2D>()){
            child.simulated = true;
            
            float randomDegree = ExplosionArcStartDegree + Random.Range(0,ExplosionArcEndDegree);
            Vector2 explosionForce = Quaternion.Euler(0,0,randomDegree) * new Vector2(ExplosionForce,0);
            child.AddForce(explosionForce,ForceMode2D.Impulse);
        }
        CompositeCollider2D composite = GetComponent<CompositeCollider2D>();
        if(composite){
            composite.enabled = false;
        } else{
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void GetDisabled() {
        gameObject.SetActive(false);
    }
}
