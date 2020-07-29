using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject healthCanvas;

    public GameObject respawn;

    public Animator anim;

    public Transform attackPoint;
    //public float attackRange = 0.5f;
    public float attackRangeX;
    public float attackRangeY;
    public LayerMask enemyLayers;

    public int attackDamage = 40;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public Transform noteStart;
    public GameObject notePrefab;

    public MovePlayer mp;
    public float regSpeed;

    public float musicCoolDown = 5f;
    private float nextMusic = 0;
    
    //despawn timer lol
    public float berimgone = 4f;

    void Start()
    {
        currentHealth = maxHealth;
        regSpeed = mp.runSpeed;

        healthBar.SetMaxHealth(maxHealth);
        healthCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
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
        anim.SetTrigger("Attack");

        //detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY),0 , enemyLayers);
        
        //apply damage 
        foreach(Collider2D enemy in hitEnemies)
        {
            var damo = enemy.GetComponent<Enemy>();
            if (damo.currentHealth > attackDamage)
            {
                damo.TakeDamage(attackDamage);
            }
            else
            {
                damo.TakeDamage(damo.currentHealth);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!mp.isDodging)
        {
            currentHealth -= damage;

            anim.SetTrigger("Hurt");
            healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        //anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        mp.enabled = false;
        healthCanvas.SetActive(false);

        Invoke("Respawn", 5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX,attackRangeY, 1));
    }

    void Music()
    {
        mp.runSpeed = mp.runSpeed / 3;
        Debug.Log("MUSIC!");
        GameObject flight = Instantiate(notePrefab, noteStart.position, noteStart.rotation);
        Destroy(flight, berimgone);
    }

    void Respawn()
    {
        transform.position = respawn.transform.position;

        //anim.SetBool("IsDead", false);

        GetComponent<Collider2D>().enabled = true;
        this.enabled = true;
        mp.enabled = true;
        healthCanvas.SetActive(true);

        Start();
    }
}
