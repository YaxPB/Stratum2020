using System.Collections;
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

    public GameObject floatyText;
    public Animator anim;
    private GameObject target;

    public GameObject theCanvas;
    private Animator theTarget;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 15;
    public int strongDamage = 30;
    private int attackType;

    private float targetDistance;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public int noteDamo = 10;
    public bool isStunned;
    public float stunDuration = 2f;
    bool isAttacking;

    void Start()
    {
        currentHealth = maxHealth;
        theCanvas.SetActive(false);
        theTarget = theCanvas.GetComponent<Animator>();
        regSpeed = speed;

        target = GameObject.FindGameObjectWithTag("Player");
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
                isAttacking = true;
                Debug.Log(target);
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
        anim.SetTrigger("Hurt");

        if (floatyText != null && currentHealth > 0)
        {
            ShowFloatyText(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ShowFloatyText(int damage)
    {
        var go = Instantiate(floatyText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }

    void Die()
    {
        theTarget.SetBool("CombatMode", false);
        theCanvas.SetActive(false);

        anim.SetBool("IsDead", true);

        //enemy gameobj is not destroyed, body is left behind
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            EnemyAttack();
        }
    }

    public void EnemyAttack()
    {
        var pc = target.GetComponent<PlayerCombat>();

        if (pc.currentHealth < 0)
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            theTarget.SetBool("CombatMode", true);

            //detect player in range
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

            var r = Random.Range(0, 100);
            if (r < 70)
            {
                //70% probability WEAK
                attackType = 1;
            }
            else if (r >= 70 && r < 90)
            {
                //20% probability STRONG
                attackType = 2;
            }
            else if (r >= 90)
            {
                //10% probability MISS
                attackType = 3;
            }
            
            switch (attackType)
            {
                case 1:
                    //play attack anim
                    anim.SetTrigger("EAttack");
                    Debug.Log("weak attack");
                    //apply damage 
                    foreach (Collider2D player in hitPlayer)
                    {
                        player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
                    }
                    break;
                case 2:
                    //play strong attack anim
                    anim.SetTrigger("SAttack");
                    Debug.Log("strong atta  ck");
                    //apply damage 
                    foreach (Collider2D player in hitPlayer)
                    {
                        player.GetComponent<PlayerCombat>().TakeDamage(strongDamage);
                    }
                    break;
                case 3:
                    //play attack anim
                    anim.SetTrigger("Miss");
                    Debug.Log("you suck");
                    break;
                default:
                    Debug.Log("shit happens");
                    break;
            }
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
            Debug.Log("ouch");
            isStunned = true;
            TakeDamage(noteDamo);
            speed = 0;
            Invoke("NotStunned", stunDuration);
        }
    }

    void NotStunned()
    {
        isStunned = false;
        Debug.Log("unstunning");
        speed = regSpeed;
    }
}
