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
    private Animator heartBoi;
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

    private RaycastHit2D deathRay;

    private int layerMask = 1 << 8;

    //despawn timer lol
    public float berimgone = 4f;
    private Canvas temp;
    
    void Start()
    {
        currentHealth = maxHealth;
        regSpeed = mp.runSpeed;
        
        healthBar.SetMaxHealth(maxHealth);
        healthCanvas.SetActive(true);
        heartBoi = healthCanvas.GetComponentInChildren<Animator>();
        heartBoi.SetBool("isCombat", false);
    }

    // Update is called once per frame
    void Update()
    {
        isCombat = CombatTrigger.isCombat;
        if (isCombat)
        {
            Debug.Log("In Combat Mode");
            heartBoi.SetBool("isCombat", true);
            deathRay = Physics2D.Raycast(attackPoint.position, new Vector2(transform.rotation.y, 0f), attackRange, layerMask);

            if (deathRay.collider != null)
            {
                Debug.DrawRay(attackPoint.position, attackPoint.TransformDirection(Vector3.right) * deathRay.distance, Color.yellow);
                Debug.Log("Hit!");
                enemyCollision = deathRay.collider;
                nearbyEnemy = enemyCollision.gameObject;
                enemyAnim = nearbyEnemy.GetComponentInChildren(typeof(Animator), true) as Animator;
                enemyAnim.enabled = true;
                enemyAnim.SetBool("isCombat", true);
                enemyAnim.SetBool("withinRange", true);

            }
            else
            {
                Debug.DrawRay(attackPoint.position, attackPoint.TransformDirection(Vector3.right) * deathRay.distance, Color.white);
                Debug.Log("Miss!");
            }
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
        Debug.Log("You died!");

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
        mp.runSpeed = mp.runSpeed / 3;
        Debug.Log("MUSIC!");
        GameObject flight = Instantiate(notePrefab, noteStart.position, noteStart.rotation);
        Destroy(flight, berimgone);
    }
}
