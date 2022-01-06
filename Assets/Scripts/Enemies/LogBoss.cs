using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class LogBoss : MonoBehaviour
{
    Rigidbody2D m_rigidbody;

    bool m_rotateWithMovement = true;

    [SerializeField] GameObject DoneLog;

    [SerializeField] Vector2 DonePosition;
    [SerializeField] Vector2 RoomPosition;

    [SerializeField] GameObject LeafSpawner;
    [SerializeField] GameObject LogPrefab;
    [SerializeField] GameObject Poof;
    Vector3 m_poofPosition;
    [SerializeField] GameObject HitEffect;

    [Header("Wall Attack")]
    [SerializeField] Vector2 WallAttackStartPosition;
    [SerializeField] float WallWoodSpeed = 6;
    [SerializeField] GameObject WallBuildUp;
    [SerializeField] GameObject RightWallBuildUp;
    [SerializeField] GameObject LeftWallBuildUp;
    [SerializeField] GameObject RightWallDespawner;
    [SerializeField] GameObject LeftWallDespawner;
    [SerializeField] List<Vector2> RightWallSpawns;
    [SerializeField] List<Vector2> LeftWallSpawns;

    [Header("Strong Hit")]
    [SerializeField] Vector2 StrongHitStartPosition;
    [SerializeField] GameObject StrongHitBuildUp;
    [SerializeField] GameObject GroundDespawner;
    [SerializeField] Destructible GroundDestructable1;
    [SerializeField] Destructible GroundDestructable2;
    [SerializeField] Destructible PlatformDestructible;
    [SerializeField] List<Vector2> StrongHitLogSpawns; 

    void Start() {
        m_rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(nameof(AttackController));
        m_poofPosition = Poof.transform.localPosition;
    }

    void Update() {
        if(m_rotateWithMovement)
            m_rigidbody.angularVelocity = -m_rigidbody.velocity.x*10;
    }

    void OnCollisionEnter2D(Collision2D other) {
        CameraManager.ShakeCamera(1,0.2f);
        foreach(ContactPoint2D contact in other.contacts)   
            Instantiate(HitEffect, contact.point, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Finish")){
            PlatformDestructible.GetDestroyed();
            transform.DOMove(DonePosition,.4f).OnComplete(()=>{
                SaveSystem.SetProgress(ProgressEnum.LogBossDefeated,true);
                DoneLog.SetActive(true);
                Destroy(gameObject);
            });
            m_rigidbody.simulated = false;
        }
    }

    IEnumerator AttackController() {
        yield return new WaitForSeconds(3f);

        yield return SpinAttack();
        yield return StrongHit();
        yield return WallAttack();

        yield return SpinAttack();
        yield return StrongHit();

        yield return WallAttack();
        yield return SpinAttack();

        yield return StrongHit();
    }

    IEnumerator SpinAttack() {
        m_rigidbody.angularVelocity = -300;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        m_rotateWithMovement = false;
        
        yield return new WaitForSeconds(2f);

        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rotateWithMovement = true;
        m_rigidbody.velocity = new Vector2(12, 0);
        Poof.transform.position = m_poofPosition + transform.position;
        Poof.transform.rotation = Quaternion.identity;
        Poof.SetActive(true);
        yield return new WaitForSeconds(10f);
    }
    int m_strongHitCounter = 0;
    IEnumerator StrongHit() {
        //Move to start position
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;

        transform.DORotate(Vector3.zero,1f);
        yield return transform.DOMove(StrongHitStartPosition+RoomPosition,1f).WaitForCompletion();
        //buildup before falldown
        StrongHitBuildUp.SetActive(true);
        yield return new WaitUntil(()=>!StrongHitBuildUp.activeSelf);
        //start falldown
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.velocity = new Vector2(0,-100);
        yield return new WaitUntil(()=>m_rigidbody.velocity.y >= 0);
        //fall effects
        switch(m_strongHitCounter++) {
            case 0 :
                LeafSpawner.SetActive(true);
            break;
            case 1 : 
                GroundDestructable1.GetDestroyed();
            break;
            default:
                GroundDestructable2.GetDestroyed();
                m_rigidbody.velocity = new Vector2(0,-5);
                LeafSpawner.SetActive(false);
                yield break;
        }

        TimeManager.EditTime(.5f,0.2f);
        CameraManager.ShakeCamera(10,0.1f);

        GroundDespawner.SetActive(true);
        foreach(Vector2 pos in StrongHitLogSpawns){
            Instantiate(LogPrefab,pos + RoomPosition,Quaternion.identity);
        }
        //jump back up
        m_rigidbody.velocity = new Vector2(0,10);
        yield return new WaitForSeconds(3.6f);//random wait
        //cleanup
        GroundDespawner.SetActive(false);
    }

    IEnumerator WallAttack() {
        GameObject logInstance = null;
    //Right Attack
        //Get in position
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;

        transform.DORotate(Vector3.zero,1f);
        yield return transform.DOMove(WallAttackStartPosition+RoomPosition,1f).WaitForCompletion();
        //attack effects
        RightWallBuildUp.SetActive(true);
        LeftWallBuildUp.SetActive(true);
        WallBuildUp.SetActive(true);
        yield return new WaitForSeconds(3f);
        WallBuildUp.SetActive(false);
        //hit right
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.velocity = new Vector2(10,0);
        Poof.transform.position = m_poofPosition + transform.position;
        Poof.transform.rotation = Quaternion.identity;
        Poof.SetActive(true);
        yield return new WaitUntil(()=>m_rigidbody.velocity.x <= 0);

        //disable log Despawners
        LeftWallDespawner.SetActive(false);
        RightWallDespawner.SetActive(false);
        //Instantiate right wall logs
        foreach(Vector2 pos in RightWallSpawns){
            Instantiate(LogPrefab,pos + RoomPosition,Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-WallWoodSpeed,0);
        }
        RightWallBuildUp.SetActive(false);
        yield return new WaitForSeconds(1f);
    //Left Attack
        //hit left
        yield return new WaitUntil(()=>m_rigidbody.velocity.x >= 0);
        //Instantiate left wall logs;
        foreach(Vector2 pos in LeftWallSpawns){
            logInstance = Instantiate(LogPrefab,pos + RoomPosition,Quaternion.identity);
            logInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(WallWoodSpeed,0);
        }
        LeftWallBuildUp.SetActive(false);
        yield return new WaitForSeconds(1f);
        //set Despawners to kill Logs
        LeftWallDespawner.SetActive(true);
        RightWallDespawner.SetActive(true);

        yield return new WaitForSeconds(5f);//this will end early if you take damage from the last log
    }  
}
