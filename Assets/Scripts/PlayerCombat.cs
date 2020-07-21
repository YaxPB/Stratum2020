using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public HealthBar healthBar;
    public GameObject healthCanvas;
    private bool isCombat;

    // public Animator anim;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private Collider2D enemyCollision;
    private GameObject nearbyEnemy;
    private Animator enemyAnim;
    private Enemy currentTarget;

    public int attackDamage = 40;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public Transform noteStart;
    public GameObject notePrefab;

    public MovePlayer mp;
    public float regSpeed;

    public float musicCoolDown = 5f;
    private float nextMusic = 0;

    private RaycastHit2D rangeRay;

    // Make sure layerMask is configured correctly
    private int layerMask = 1 << 8;
    // private LayerMask mask = LayerMask.GetMask("Wall");

    //despawn timer lol
    public float berimgone = 4f;
    private Canvas temp;

    public bool loggingEnabled = false;
    // this will be the only instance of PlayerCombat at any given time; can be referenced by other scripts
    public static PlayerCombat instance;
    
    void Start()
    {
        instance = this;
        currentHealth = maxHealth;
        regSpeed = mp.runSpeed;
        
        healthBar.SetMaxHealth(maxHealth);
        healthCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCombat)
        {
            // Detects if enemy is within range to attack (targeting function)
            TargetAssist();
        }

        if (Time.time >= nextAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttack = Time.time + 1f / attackRate;
            }
        }

        if (Time.time > nextMusic)
        {
            mp.runSpeed = regSpeed;
            if (Input.GetButtonDown("Berimbau"))
            {
                Music();
                nextMusic = Time.time + musicCoolDown;
            }
        }
    }

    void Attack()
    {
        //play attack anim
        // anim.SetTrigger("Attack");

        //detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        //apply damage 
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            AudioManager.PlaySound("kick");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt anim
        // anim.SetTrigger("Hurt");
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (loggingEnabled)
        {
            Debug.Log("You died!");
        }

        // anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        mp.enabled = false;
        healthCanvas.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Music()
    {
        if (loggingEnabled)
        {
            Debug.Log("MUSIC!");
        }
        mp.runSpeed = mp.runSpeed / 3;
        GameObject flight = Instantiate(notePrefab, noteStart.position, noteStart.rotation);
        Destroy(flight, berimgone);
    }

    void TargetAssist()
    {
        if (isCombat)
        {
            if (loggingEnabled)
            {
                Debug.Log("In Combat Mode");
            }
            rangeRay = Physics2D.Raycast(attackPoint.position, new Vector2(transform.rotation.y, 0f), attackRange, layerMask);

            if (rangeRay.collider != null)
            {
                if (loggingEnabled)
                {
                    Debug.DrawRay(attackPoint.position, attackPoint.TransformDirection(Vector3.right) * rangeRay.distance, Color.yellow);
                    Debug.Log("Hit!");
                }

                enemyCollision = rangeRay.collider;
                nearbyEnemy = enemyCollision.gameObject;

                nearbyEnemy.SendMessage("LockedOn", true);

            }
            else
            {
                if (loggingEnabled)
                {
                    Debug.DrawRay(attackPoint.position, attackPoint.TransformDirection(Vector3.right) * rangeRay.distance, Color.white);
                    Debug.Log("Miss!");
                }
            }
        }
    }

    void TimeToFight(bool combatMode)
    {
        if (!combatMode)
        {
            instance.enabled = false;
        }
        isCombat = combatMode;
        instance.enabled = true;
    }
}
