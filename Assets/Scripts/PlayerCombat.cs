using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private int Lives;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject healthCanvas;
    [SerializeField]
    private bool isCombat;

    public GameObject respawn;
    public GameObject floatyText;
    private GameObject GameOver;

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
    private GameObject flight;
    private LevelLoader ll;

    [SerializeField] private int attackDamage;
    private int baseDamage = 25;

    public float attackRate = 1.5f;
    float nextAttack = 0f;

    public Transform noteStart;
    public GameObject notePrefab;
    private CircleCollider2D berimbauRange;

    public MovePlayer mp;
    public float regSpeed;

    public float musicCoolDown = 5f;    
    private float nextMusic = 0;

    //despawn timer lol
    public float berimgone = 4.6f;
    private float[] timerRotationZ = new float[4] { -42.5f, 42.5f, 135, -135 };
    Collider2D[] hitEnemies;
    Collider2D[] hitBreakables;

    public GameObject berimBeatDownTimer;
    private bool isPlaying = false;

    public bool loggingEnabled = false;

    public bool dead { get; private set; }

    // this will be the only instance of PlayerCombat at any given time; can be referenced by other scripts
    public static PlayerCombat instance;
    public bool nextLevel = false;

    CameraShake cs;
    SpriteRenderer sp;

    private float respawn_time = 3f;

    public bool timeToRestart = false;

    private void Awake()
    {
        GameOver = GameObject.Find("GameOver");
        cs = FindObjectOfType<CameraShake>();
        ll = FindObjectOfType<LevelLoader>();
        sp = GetComponentInChildren<SpriteRenderer>();
        GameOver.SetActive(false);
        Lives = 3;
    }

    void Start()
    {
        instance = this;
        currentHealth = maxHealth;
        healthBar.GetComponent<HealthBar>().SetMaxHealth(maxHealth);
        regSpeed = mp.runSpeed;
        attackDamage = baseDamage;

        healthCanvas.SetActive(true);
        berimBeatDownTimer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCombat)
        {
            //detect enemies in range
            hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0, enemyLayers);

            hitBreakables = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0, breakableLayers);
            if (Time.time >= nextAttack && !isPlaying)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Attack();
                    nextAttack = Time.time + 1f / attackRate;
                }
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
        mp.runSpeed = 0f;
        Invoke("ResetSpeed", 1f);

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
            AudioManagerSFX.PlaySound("kickEnemy");
        }

        foreach (Collider2D breakable in hitBreakables)
        {
            AudioManagerSFX.PlaySound("kickLamp");
            breakable.GetComponent<Breakable>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!dead)
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
            }

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
        //cause floaty text to flip with Player object
        var go = Instantiate(floatyText, transform.position + transform.up * 4, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }

    void ResetShake()
    {
        cs.shakeDistance = 0f;
    }

    void ResetSpeed()
    {
        mp.runSpeed = regSpeed;
    }

    void Die()
    {
        Lives--;
        healthBar.AdjustLives(Lives);
        dead = true;
        anim.SetBool("isWalking", false);
        anim.SetBool("IsDead", true);
        if (loggingEnabled)
        {
            Debug.Log("You died!");
        }

        this.enabled = false;
        mp.enabled = false;
        // healthCanvas.SetActive(false);

        if (ll != null && nextLevel)
        {
            ll.LoadNextLevel();
        }
        else
        {
            Invoke("Respawn", 3.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 1));
        // Gizmos.DrawWireSphere(noteStart.position, noteStart.GetComponent<CircleCollider2D>().radius);
    }

    void Music()
    {
        if (loggingEnabled)
        {
            Debug.Log("MUSIC!");
        }

        anim.SetBool("Berimbau",true);
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

        // Freeze the player (momentarily), play some music, button prompts
        MovePlayer.instance.canMove = false;

        Destroy(flight, berimgone);
        // Then either a combo multiplies total damage to affect enemies all at once at the end of the ability
        // OR hits that happen in quick succession with each correctly timed button press
        yield return new WaitForSeconds(berimgone);
        anim.SetBool("Berimbau", false);
        MovePlayer.instance.canMove = true;
        isPlaying = false;
        yield return new WaitForSeconds(berimgone);
        attackDamage = baseDamage;

    }

    void BuffBoi(int powMultiplier)
    {
        attackDamage += powMultiplier * 10;
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

    public void Respawn()
    {
        if(Lives > 0)
        {
            anim.SetBool("IsDead", false);
            transform.position = respawn.transform.position;
        
            GetComponent<Collider2D>().enabled = true;
            this.enabled = true;
            mp.enabled = true;

            Start();
            StartCoroutine(Respawning());
        }
        else
        {
            timeToRestart = true;
            GameOver.SetActive(true);
            healthCanvas.SetActive(false);
        }
    }

    //if player dies in tutorial swarm area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DeathCheck")
        {
            nextLevel = true;
        }
    }

    private IEnumerator Respawning()
    {
        float timeElapsed = 0f;
        float totalTime = respawn_time;
        while (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime;
            sp.color = Color.Lerp(new Color(1,1,1,0), new Color(1,1,1,1), timeElapsed / totalTime);
            yield return null;
        }
        dead = false;
    }
}

