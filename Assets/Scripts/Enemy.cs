﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public float speed;
    public float regSpeed;
    public float chaseDistance;
    public float stopDistance;
    
    // public Animator anim;
    public GameObject target;

    public GameObject theCanvas;
    private Animator hitMe;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 20;

    private float targetDistance;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public int noteDamo = 10;
    public bool isStunned;
    public float stunDuration = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        theCanvas.SetActive(false);
        hitMe = theCanvas.GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            Debug.Log("nani");
            ChasePlayer(); 
        }
        else
            StopChasePlayer();

        if (targetDistance <= stopDistance)
        {
            if (Time.time >= nextAttack)
            {
                    EnemyAttack();
                    nextAttack = Time.time + 1f / attackRate;
            }
        }
    }

    private void StopChasePlayer()
    {
        //nothing wow
    }

    private void ChasePlayer()
    {
        theCanvas.SetActive(true);
        
        //add in a correct flip function to follow player
        if (transform.position.x < target.transform.position.x)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt anim
        // anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // theTarget.SetBool("CombatMode", false);
        theCanvas.SetActive(false);
        Debug.Log("Enemy died!");

        // anim.SetBool("IsDead", true);

        //enemy gameobj is not destroyed, body is left behind
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        //EnemyManager.instance.enemyCount--;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("player next to me");
            EnemyAttack();
        }
    }

    public void EnemyAttack()
    {
        //play attack anim
        // anim.SetTrigger("EAttack");

        // theTarget.SetBool("CombatMode", true);

        //detect player in range
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer, 0f, 5f);

        //apply damage 
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Music") && isStunned == false)
        {
            isStunned = true;
            TakeDamage(noteDamo);
            speed = speed / 4;
            Invoke("NotStunned", stunDuration);
        }
        if (other.CompareTag("PewPew"))
        {
            Debug.Log("Raycast detected.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PewPew"))
        {
            Debug.Log("Raycast has left the building");
            hitMe.SetBool("withinRange", false);
        }
    }

    void NotStunned()
    {
        isStunned = false;
        Debug.Log("unstunning");
        speed = regSpeed;
    }
}
