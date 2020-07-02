using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public float speed;
    public float chaseDistance;
    public float stopDistance;

    public Animator anim;
    public GameObject target;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 20;

    private float targetDistance;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            ChasePlayer(); 
        }
        else
            StopChasePlayer();

        if (targetDistance <= stopDistance)
        {
            if (Time.time >= nextAttack)
            {
                    Debug.Log("player next to me");
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
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
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
        anim.SetTrigger("EAttack");

        //detect player in range
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        //apply damage 
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
