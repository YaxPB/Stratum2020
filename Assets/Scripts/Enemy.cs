using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private bool dead = false;

    public float speed;
    public float regSpeed;
    public float chaseDistance;
    public float stopDistance;

    public GameObject floatyText;
    public Animator anim;
    private GameObject target;
    private PlayerCombat combat;

    Overhead oh;
    public GameObject theCanvas;
    private Animator hitMe;

    public EnemyHealthBar healthBar;
    public GameObject healthCanvas;

    public Transform attackPoint;
    public Transform pukePoint;
    //public float attackRange = 0.5f;
    public float attackRangeX;
    public float attackRangeY;

    public LayerMask playerLayer;
    public int attackDamage = 10;
    public int strongDamage = 20;
    private int attackType;

    private float targetDistance;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public int noteDamo = 20;
    public bool isStunned;
    public float stunDuration = 2f;
    public float timeAfterDamo = 1.4f;
    bool isAttacking;
    bool isBlocking;

    int arrCount;

    public bool loggingEnabled = false;
    private bool stopped;

    Vomiting flight;
    public Vomiting vomitPrefab;
    public float despawn = 2f;

    //health drop chance
    HealthPickup drop;
    public HealthPickup hp;

    void Start()
    {
        combat = FindObjectOfType<PlayerCombat>();
        oh = FindObjectOfType<Overhead>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthCanvas.SetActive(true);

        theCanvas.SetActive(false);
        regSpeed = speed;
        target = GameObject.FindGameObjectWithTag("Player");
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
        

        if (targetDistance <= stopDistance && !isStunned && !combat.dead)
        {
            if (Time.time >= nextAttack)
            {
                //random chance for enemy to attack player so they're not constantly aggressive
                var r = Random.Range(0, 100);
                if (r < 80)
                {
                    isAttacking = true;
                    EnemyAttack();
                }
                nextAttack = Time.time + 1f / attackRate;
            }
        }
    }

    private void StopChasePlayer()
    {
        anim.SetBool("isWalking", false);
    }

    private void ChasePlayer()
    {
        theCanvas.SetActive(true);

        if(!isBlocking)
            anim.SetBool("isWalking", true);

        if (transform.position.x < target.transform.position.x)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        else
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (!dead)
        {
            if (isBlocking && currentHealth > 0)
            {
                stopped = true;
                currentHealth -= damage / 3;

                if (oh != null && oh.readyToDecrease)
                    oh.AdjustPool(damage / 3);
                StopBlock();
            }
            else if (currentHealth > 0)
            {
                currentHealth -= damage;
                if (oh != null && oh.readyToDecrease)
                    oh.AdjustPool(damage);
            }

            isStunned = true;
            anim.SetTrigger("Hurt");
            healthBar.SetHealth(currentHealth);

            Invoke("NotStunned", timeAfterDamo);

            if (floatyText != null && currentHealth > 0)
            {
                ShowFloatyText(damage);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void ShowFloatyText(int damage)
    {
        var go = Instantiate(floatyText, transform.position + transform.up * 3, Quaternion.identity, transform);
        if(isBlocking)
        {
            int blockdamo = damage / 3;
            go.GetComponent<TextMesh>().text = blockdamo.ToString();
        }
        else
        {
        go.GetComponent<TextMesh>().text = damage.ToString();
        }
    }

    void Die()
    {
        dead = true;
        theCanvas.SetActive(false);
        AudioManagerSFX.PlaySound("enemyDied");
        anim.SetBool("isWalking", false);

        anim.SetBool("isDead", true);
        // enemy gameobj is not destroyed, body is left behind for 2 seconds
        this.enabled = false;

        var r = Random.Range(0, 100);
        if (r > 90)
            drop = Instantiate<HealthPickup>(hp, transform.position + transform.right * 1, transform.rotation);

        Destroy(gameObject, 2f);
    }

    public void EnemyAttack()
    {
        var pc = target.GetComponent<PlayerCombat>();
        if (pc.currentHealth < 0)
        {
            isAttacking = false;
        }

        if (isAttacking && !isBlocking && !isStunned)
        {
            //detect player in range
            Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0, playerLayer);

            var r = Random.Range(0, 100);
            if (r < 60)
            {
                //60% probability WEAK
                attackType = 1;
            }
            else if (r >60 && r < 75)
            {
                //15% probability STRONG
                attackType = 2;
            }
            else if (r >= 75 && r < 90)
            {
                //15% probability VOMIT
                attackType = 3;
            }
            else if (r >= 90)
            {
                //10% probability BLOCK
                attackType = 4;
            }
            
            anim.SetBool("isWalking", false);

            switch (attackType)
            {
                case 1:
                    anim.SetTrigger("basicAttack");
                    AudioManagerSFX.PlaySound("basicAttack");
                    Debug.Log("weak attack");
                    foreach (Collider2D player in hitPlayer)
                    {
                        pc.TakeDamage(attackDamage);
                    }
                    break;
                case 2:
                    anim.SetTrigger("strongAttack");
                    AudioManagerSFX.PlaySound("strongAttack");
                    Debug.Log("strong attack");
                    foreach (Collider2D player in hitPlayer)
                    {
                        pc.TakeDamage(strongDamage);
                    }
                    break;
                case 3:
                    anim.SetTrigger("Vomit");
                    Debug.Log("vomit");
                    Vector2 direction = Vector2.right;
                    if(GetComponentInChildren<SpriteRenderer>().flipX)
                    {
                        direction = Vector2.left;
                    }
                    flight = Vomiting.CreateVomit(vomitPrefab, pukePoint, direction);
                    Invoke("ByeVomit", despawn);
                    break;
                case 4:
                    stopped = false;
                    Debug.Log("blocking");
                    speed = speed/2;
                    anim.SetBool("isBlocking", true);
                    anim.SetTrigger("Block");
                    anim.SetBool("isWalking", false);
                    isBlocking = true;
                    if (!stopped)
                    {
                        Invoke("StopBlock", 3f);
                    }
                    break;
                default:
                    Debug.Log("welp that happened");
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 1));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Music"))
        {
            Debug.Log("oh nooo i'm stunned");
            isStunned = true;
            speed = 0;
            Invoke("NotStunned", stunDuration);
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Music"))
        {
            NotStunned();
        }
    }

    void Stunned()
    {
        Debug.Log("oh nooo i'm stunned");
        isStunned = true;
        speed = 0;
    }*/

    void NotStunned()
    {
        isStunned = false;
        speed = regSpeed;
    }

    void ByeVomit()
    {
        if (!flight.didHit)
        {
            Destroy(flight);
        }
        Destroy(flight, 5f);
    }

    void StopBlock()
    {
        isBlocking = false;
        anim.SetBool("isBlocking", false);
        speed = regSpeed;
    }
}
