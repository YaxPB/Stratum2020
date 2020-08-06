using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float speed;
    public float regSpeed;
    public float chaseDistance;
    public float stopDistance;

    public GameObject floatyText;
    public Animator anim;
    private GameObject target;

    Overhead oh;
    public GameObject theCanvas;
    private Animator hitMe;

    public HealthBar healthBar;
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

    void Start()
    {
        oh = FindObjectOfType<Overhead>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthCanvas.SetActive(true);

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
                EnemyAttack();
                nextAttack = Time.time + 1f / attackRate;
            }
        }
    }

    private void StopChasePlayer()
    {
        anim.SetBool("isWalking", false);
        //anim.SetBool("isBlocking", false);
        //isBlocking = false;
        //speed = regSpeed;
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
        if (isBlocking)
        {
            stopped = true;
            currentHealth -= damage / 3;
            StopBlock();
        }
        else
        {
            currentHealth -= damage;
        }
        
        isStunned = true;
        anim.SetTrigger("Hurt");
        healthBar.SetHealth(currentHealth);
        if(oh != null && oh.readyToDecrease)
            oh.AdjustPool(damage);
        
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

    void ShowFloatyText(int damage)
    {
        var go = Instantiate(floatyText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }

    void Die()
    {
        //theTarget.SetBool("CombatMode", false);
        theCanvas.SetActive(false);
        hitMe.SetBool("isCombat", false);
        Debug.Log("Enemy died!");
        AudioManagerSFX.PlaySound("enemyDied");
        // anim.SetBool("IsDead", true);

        anim.SetBool("isWalking", false);

        // enemy gameobj is not destroyed, body is left behind
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 2f);
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
                    //play attack anim
                    anim.SetTrigger("basicAttack");
                    AudioManagerSFX.PlaySound("basicAttack");
                    anim.SetTrigger("isAttacking");
                    Debug.Log("weak attack");
                    //apply damage 
                    foreach (Collider2D player in hitPlayer)
                    {
                        player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
                    }
                    break;
                case 2:
                    //play strong attack anim
                    // anim.SetTrigger("strongAttack");
                    // AudioManagerSFX.PlaySound("strongAttack");
                    Debug.Log("strong attack");
                    anim.SetTrigger("SAttack");
                    Debug.Log("strong attack");
                    
                    //apply damage 
                    foreach (Collider2D player in hitPlayer)
                    {
                        // This fixed the NullReferenceException that was happening during enemy attacks
                        PlayerCombat.instance.TakeDamage(strongDamage);
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
        if (other.CompareTag("Music") && isStunned == false)
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
        anim.StopPlayback();
    }

    void NotStunned()
    {
        isStunned = false;
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

    void ByeVomit()
    {
        if (!flight.didHit)
        {
            Destroy(flight);
        }
    }

    void StopBlock()
    {
        isBlocking = false;
        anim.SetBool("isBlocking", false);
        speed = regSpeed;
    }
}
