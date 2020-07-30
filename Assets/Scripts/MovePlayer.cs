using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float runSpeed;
    public float rollWindup = 0.1f;
    private Vector3 slideDir;
    private Rigidbody2D playerRB;
    private Vector3 rolling;

    float horizontal;
    float vertical;
    bool facingRight;

    bool canMove;

    //Animator animator;

    public Animator animator;
    
    private Rigidbody2D rb;
    public float rollSpeed;

    [SerializeField]
    private float rollTime;
    public float startRollTime;

    [SerializeField]
    private int direction;
    public bool isDodging { get; private set; }

    //bool isJumping;
    bool isRolling;

    // Sound variables
    private float stepOffset;
    private AudioSource playerFX;


    private void Awake()
    {
        canMove = true;
        playerRB = GetComponent<Rigidbody2D>();
        playerFX = GetComponent<AudioSource>();

        /*rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.Sleep();*/
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rollTime = startRollTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (stepOffset > 0)
        {
            stepOffset -= Time.deltaTime;
        }
        if (stepOffset < 0)
        {
            stepOffset = 0;
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rolling = new Vector3(horizontal * rollSpeed, vertical * rollSpeed, 0.0f);
        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && stepOffset == 0)
        {
            AudioManagerSFX.PlaySound("run");
            // Can change this to adjust speed of footstep sounds
            stepOffset = 0.25f;
        }

        /*if (!canMove)
        {
            playerRB.velocity = Vector2.zero;
            return;
        }*/

        if (!isDodging)
        {
            if (direction != 0)
            {
                //start coroutine
                if (Input.GetButtonDown("Dodge"))
                {
                    isDodging = true;
                    StartCoroutine(BeginDodgeRoll());
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
                direction = 1;
            else if (Input.GetKeyDown(KeyCode.D))
                direction = 2;
            else if (Input.GetKeyDown(KeyCode.W))
                direction = 3;
            else if (Input.GetKeyDown(KeyCode.S))
                direction = 4;
        }
        else
        {
            //start coroutine
            if (Input.GetButtonDown("Dodge"))
            {
                StartCoroutine(BeginDodgeRoll());
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);

        if(horizontal > 0 || horizontal < 0 || vertical > 0 || vertical < 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);

        }

        transform.position = transform.position + movement * Time.deltaTime;
        Flip(horizontal);
    }

    private void Flip(float horizontal)
    {
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)   
        {
            facingRight = !facingRight;

            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator BeginDodgeRoll()
    {
        switch (direction)
        {
            case 1:
                Debug.Log("rolled left");
                rb.velocity = Vector2.left * rollSpeed;
                direction = 0;
                break;
            case 2:
                Debug.Log("rolled right");
                rb.velocity = Vector2.right * rollSpeed;
                direction = 0;
                break;
            case 3:
                Debug.Log("rolled up");
                rb.velocity = Vector2.up * rollSpeed;
                direction = 0;
                break;
            case 4:
                Debug.Log("rolled down");
                rb.velocity = Vector2.down * rollSpeed;
                direction = 0;
                break;
            default:
                Debug.Log("Uh oh,no direction");
                break;
        }
        Invoke("Reset", startRollTime);
        yield return new WaitForSeconds(rollWindup);
    }

    void Reset()
    {
        if (rollTime <= 0)
        {
            rollTime = startRollTime;
            rb.velocity = Vector2.zero;
            isDodging = false;
        }
    }
}
