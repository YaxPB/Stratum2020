using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject healthCanvas;
    private bool isCombat;

    public GameObject respawn;

    public Animator anim;

    public Transform attackPoint;
    //public float attackRange = 0.5f;
    public float attackRangeX;
    public float attackRangeY;
    public LayerMask enemyLayers;
    public LayerMask breakableLayers;
    private Collider2D enemyCollision;
    private GameObject nearbyEnemy;
    private Animator enemyAnim;
    private Enemy currentTarget;

    public int attackDamage = 25;
    private int baseDamage;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public Transform noteStart;
    public GameObject notePrefab;
    private CircleCollider2D berimbauRange;

    public MovePlayer mp;
    public float regSpeed;

    public float musicCoolDown = 5f;
    private float nextMusic = 0;

    private RaycastHit2D rangeRay;
    
    private int layerMask = 1 << 8;
    // private LayerMask mask = LayerMask.GetMask("Wall");

    //despawn timer lol
    public float berimgone = 4.6f;
    private float[] timerRotationZ = new float[4] { -42.5f, 42.5f, 135, -135 };

    public GameObject berimBeatDownTimer;
    private bool isPlaying = false;

    public bool loggingEnabled = false;
    // this will be the only instance of PlayerCombat at any given time; can be referenced by other scripts
    public static PlayerCombat instance;

    public CameraShake cs;
    
    void Start()
    {
        instance = this;
        currentHealth = maxHealth;
        regSpeed = mp.runSpeed;
        baseDamage = attackDamage;

        healthBar.SetMaxHealth(maxHealth);
        healthCanvas.SetActive(true);
        berimBeatDownTimer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCombat)
        {
            // Detects if enemy is within range to attack (targeting function)
            TargetAssist();

            if (Time.time >= nextAttack && !isPlaying)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Attack();
                    nextAttack = Time.time + 1f / attackRate;
                }
            }

        if (Time.time > nextMusic)
        {
            //mp.runSpeed = regSpeed;
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
       
        Collider2D[] hitBreakables = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0, breakableLayers);
        mp.runSpeed = 0f;
        
        //apply damage 
        foreach (Collider2D enemy in hitEnemies)
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
            //AudioManagerSFX.PlaySound("kick");
        }

        Invoke("ResetSpeed", 0.45f);

        foreach (Collider2D breakable in hitBreakables)
        {
            AudioManagerSFX.PlaySound("kickLamp");
            breakable.GetComponent<Breakable>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!mp.isDodging)
        {
            mp.runSpeed = 0f;
            currentHealth -= damage;

            anim.SetTrigger("Hurt");
            healthBar.SetHealth(currentHealth);

            cs.shakeDistance = 0.06f;
            Invoke("ResetShake", 0.2f);
            Invoke("ResetSpeed", 0.2f);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        //anim.SetBool("IsDead", true);
        if (loggingEnabled)
        {
            Debug.Log("You died!");
        }

        // anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        mp.enabled = false;
        healthCanvas.SetActive(false);

        Invoke("Respawn", 3.5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX,attackRangeY, 1));
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(noteStart.position, noteStart.GetComponent<CircleCollider2D>().radius);
    }

    void Music()
    {

        if (loggingEnabled)
        {
            Debug.Log("MUSIC!");
        }

        StartCoroutine(BerimBeats());
        berimBeatDownTimer.SetActive(true);
    }

    // Will probably be an IEnumerator so we can add a quick yield delay for switching things off/on
    IEnumerator BerimBeats()
    {
        isPlaying = true;
        AudioManagerBG.SwitchTrack("berimBAM");
       
        GameObject flight = Instantiate(notePrefab, noteStart.position, noteStart.rotation, noteStart);
        berimbauRange = flight.GetComponent<CircleCollider2D>();
        Destroy(flight, berimgone);

        // Freeze the player (momentarily), play some music, button prompts
        MovePlayer.instance.canMove = false;

        // Then either a combo multiplies total damage to affect enemies all at once at the end of the ability
        // OR hits that happen in quick succession with each correctly timed button press
        yield return new WaitForSeconds(berimgone);
        MovePlayer.instance.canMove = true;
        isPlaying = false;
        yield return new WaitForSeconds(berimgone);
        attackDamage = baseDamage;

    }

    void BuffBoi(int powMultiplier)
    {
        attackDamage += powMultiplier * 10;
    }

    void TargetAssist()
    {
        if (isCombat)
        {
            if (loggingEnabled)
            {
                Debug.Log("In Combat Mode");
            }
            rangeRay = Physics2D.Raycast(attackPoint.position, new Vector2(transform.rotation.y, 0f),attackRangeX, layerMask);

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
        isCombat = combatMode;
        if (isCombat == false)
        {
            instance.enabled = false;
        }
        else
        {
            instance.enabled = true;
        }

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

    private void ResetShake()
    {
        cs.shakeDistance = 0f;
    }
    
    private void ResetSpeed()
    {
        mp.runSpeed = regSpeed;
    }
}
