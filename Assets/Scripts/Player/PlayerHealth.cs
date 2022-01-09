﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public int MaxHealth;
    [HideInInspector] public int Health;

    [SerializeField] float IFrameTime = 1f;
    float lastHitTime;

    public UnityEvent<int> OnHealthChange;

    Rigidbody2D m_rigidbody;

    void Start() {
        Health = MaxHealth;
        lastHitTime = Time.time;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if(enemyHealth)
                TakeDamage((int)enemyHealth.Damage);
            else{
                Projectile projectile = other.GetComponent<Projectile>();
                TakeDamage((int)projectile.Damage);
                projectile.DestroySelf();
            }
        }
    }

    public void Heal() {
        Heal(MaxHealth);
    }

    public void Heal(int heal) {
        heal = Mathf.Min(heal,MaxHealth-Health);
        Health += heal;
        OnHealthChange.Invoke(heal);
    }

    public void TakeDamage(int damage) {
        if(Time.time - lastHitTime > IFrameTime && Health > 0){
            lastHitTime = Time.time;
            Health = Mathf.Max(Health-damage,0);
            OnHealthChange.Invoke(-damage);
            if(Health <= 0){
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            TimeManager.EditTime(.1f,.05f);
        }
    }

    public void Die() {
        SaveSystem.SaveData = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
