using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float runSpeed = 1f;
    public float rollWindup = 0.1f;
    private Vector3 slideDir;
    private Rigidbody2D playerRB;
    private Vector3 rolling;

    float horizontal;
    float vertical;
    bool facingRight;

    public bool canMove;

    Animator animator;

    private Rigidbody2D rb;
    public float rollSpeed;
    private float rollTime;
    public float startRollTime;
    private int direction;

    //bool isJumping;
    bool isRolling;

    // Sound variables
    private float stepOffset;
    private AudioSource playerFX;

    public static MovePlayer instance;


    private void Awake()
    {
        canMove = true;
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerFX = GetComponent<AudioSource>();

        /*rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.Sleep();*/
    }

    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        rollTime = startRollTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            playerRB.velocity = Vector2.zero;
            horizontal = 0f;
            vertical = 0f;
            return;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            rolling = new Vector3(horizontal * rollSpeed, vertical * rollSpeed, 0.0f);
        }

        if (stepOffset > 0)
        {
            stepOffset -= Time.deltaTime;
        }
        if (stepOffset <= 0)
        {
            stepOffset = 0;
        }

        if(canMove && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && stepOffset == 0)
        {
            AudioManagerSFX.PlaySound("run");
            // Can change this to adjust speed of footstep sounds
            stepOffset = 0.25f;
        }

        if (direction == 0) {
            if (Input.GetKeyDown(KeyCode.A))
                direction = 1;
            else if (Input.GetKeyDown(KeyCode.D))
                direction = 2;
            else if (Input.GetKeyDown(KeyCode.W))
                direction = 3;
            else if (Input.GetKeyDown(KeyCode.S))
                direction = 4;
        }
        else {
            //start coroutine
            if (Input.GetButtonDown("Dodge")) {
                rollTime -= Time.deltaTime;
                StartCoroutine(BeginDodgeRoll());
            }
        }

        if (rollTime <= 0)
        {
            direction = 0;
            rollTime = startRollTime;
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
        transform.position = transform.position + movement * Time.deltaTime;
        Flip(horizontal);

        /*if(transform.position.y <= axisY)
        {
            OnLanding();
        }

        Input.GetButtonDown("Jump") && !isJumping)
        {
            axisY = transform.position.y;
            isJumping = true;
            rigidBody.gravityScale = 1.5f;
            rigidBody.WakeUp();
            rigidBody.AddForce(new Vector2(transform.position.x + 7.5f, jumpForce));
            //animator.SetBool("isJumping", true);
        }*/
    }

    private void Flip(float horizontal)
    {
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)   
        {
            facingRight = !facingRight;

            /*Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;*/

            transform.Rotate(0f, 180f, 0f);
        }
    }

    /*void OnLanding()
    {
        isJumping = false;
        rigidBody.gravityScale = 0f;
        rigidBody.Sleep();
        axisY = transform.position.y;
        //animator.SetBool("isJumping", false);
    }*/

    private IEnumerator BeginDodgeRoll()
    {
        Debug.Log("im rolling");
        yield return new WaitForSeconds(rollWindup);

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
    }
}
