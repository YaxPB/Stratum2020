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
    private Animator hitMe;

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
    [SerializeField] private bool isStunned;
    public float stunDuration = 2f;
    bool isAttacking;

    public bool loggingEnabled = false;

    void Start()
    {
        currentHealth = maxHealth;
        theCanvas.SetActive(false);
        regSpeed = speed;

        target = GameObject.FindGameObjectWithTag("Player");
        hitMe = theCanvas.GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            if (loggingEnabled)
            {
                Debug.Log("nani");
            }
            
            ChasePlayer(); 
        }
        else
        {
            StopChasePlayer();
        }
        

        if (targetDistance <= stopDistance && !isStunned)
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
        anim.SetTrigger("stopChase");
    }

    private void ChasePlayer()
    {
        theCanvas.SetActive(true);
        anim.SetTrigger("chasePlayer");
        // add in a correct flip function to follow player
        if (transform.position.x < target.transform.position.x)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // play hurt anim
        // anim.SetTrigger("Hurt");

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
        theCanvas.SetActive(false);
        hitMe.SetBool("isCombat", false);
        Debug.Log("Enemy died!");
        AudioManagerSFX.PlaySound("enemyDied");
        // anim.SetBool("IsDead", true);

        // enemy gameobj is not destroyed, body is left behind
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
        anim.ResetTrigger("basicAttack");
        anim.ResetTrigger("strongAttack");
        if (pc.currentHealth < 0)
        {
            isAttacking = false;
        }

        if (isAttacking)
        {

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
                    anim.SetTrigger("basicAttack");
                    AudioManagerSFX.PlaySound("basicAttack");
                    Debug.Log("weak attack");
                    //apply damage 
                    foreach (Collider2D player in hitPlayer)
                    {
                        pc.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
                    }
                    break;
                case 2:
                    //play strong attack anim
                    // anim.SetTrigger("strongAttack");
                    // AudioManagerSFX.PlaySound("strongAttack");
                    Debug.Log("strong attack");
                    //apply damage 
                    foreach (Collider2D player in hitPlayer)
                    {
                        // player.GetComponent<PlayerCombat>().TakeDamage(strongDamage);
                    }
                    break;
                case 3:
                    //play attack anim
                    // anim.SetTrigger("Miss");
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
        if (other.CompareTag("Music"))
        {
            Debug.Log("oh nooo i'm stunned");
            isStunned = true;
            speed = 0;
            anim.StopPlayback();
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
        anim.StartPlayback();
    }

    void LockedOn(bool linedUp)
    {
        if (!linedUp)
        {
            hitMe.enabled = false;
        }
        Debug.Log("Locked on!");
        hitMe.enabled = true;
        hitMe.SetBool("isCombat", true);
        hitMe.SetBool("withinRange", true);
    }
}
