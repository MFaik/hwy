using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Vector2 Velocity;

    [SerializeField] GameObject Object;
    [SerializeField,Min(0)] float Interval = 1f;
    [SerializeField] bool Loop;

    float startTime;

    List<GameObject> spawnedObjects = new List<GameObject>();

    void Start() {
        if(!Loop)
            Destroy(gameObject,Interval);
    }

    void Update() {
        if(Time.time - startTime > Interval){
            Spawn();
            startTime = Time.time;
        }
    }

    void Spawn() {
        GameObject newObject = Instantiate(Object,transform.position,transform.rotation,transform);
        Rigidbody2D rigidbody = newObject.GetComponent<Rigidbody2D>();
        if(rigidbody)
            rigidbody.velocity = Velocity;

        spawnedObjects.Add(newObject);
    }

    void OnDisable() {
        foreach(GameObject obj in spawnedObjects){
            Destroy(obj);
        }
    }
    void OnEnable() {
        startTime = Time.time;    
    }
}
