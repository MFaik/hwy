using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Spawner : MonoBehaviour
{
    [SerializeField] bool RandomizeBetweenTwoVelocities;
    [SerializeField] Vector2 Velocity;
    [SerializeField,ConditionalField(nameof(RandomizeBetweenTwoVelocities))]Vector2 OtherVelocity;
    [SerializeField] GameObject Object;
    [SerializeField] float Offset = 0f;
    [SerializeField] float Interval = 1f;
    [SerializeField] bool Loop = true;

    float m_startTime;

    List<GameObject> m_spawnedObjects = new List<GameObject>();

    BoxCollider2D m_collider;

    void Start() {
        if(!Loop)
            Destroy(gameObject,Interval);

        m_collider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        if(Time.time - m_startTime > Interval){
            Spawn();
            m_startTime = Time.time;
        }
    }

    void Spawn() {
        Vector2 spawnPosition = transform.position;
        if(m_collider){
            spawnPosition = RandomPointInBounds(m_collider.bounds);
        }
        GameObject newObject = Instantiate(Object,spawnPosition,transform.rotation,transform);
        Rigidbody2D rigidbody = newObject.GetComponent<Rigidbody2D>();
        if(rigidbody){
            if(RandomizeBetweenTwoVelocities){
                Vector2 small = new Vector2(Mathf.Min(Velocity.x,OtherVelocity.x),Mathf.Min(Velocity.y,OtherVelocity.y));
                Vector2 big   = new Vector2(Mathf.Max(Velocity.x,OtherVelocity.x),Mathf.Max(Velocity.y,OtherVelocity.y));
                rigidbody.velocity = new Vector2(Random.Range(small.x,big.x),Random.Range(small.y,big.y));
            } else 
                rigidbody.velocity = Velocity;
        }
           

        m_spawnedObjects.Add(newObject);
    }

    void OnDisable() {
        foreach(GameObject obj in m_spawnedObjects){
            Destroy(obj);
        }
    }
    void OnEnable() {
        m_startTime = Time.time + (Offset - Interval);    
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}
