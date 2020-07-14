using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float runSpeed = 1f;
    private Vector3 slideDir;

    float horizontal;
    float vertical;
    bool facingRight;

    Animator animator;

    private Rigidbody2D rb;
    public float rollSpeed;
    private float rollTime;
    public float startRollTime;
    private int direction;

    //bool isJumping;

    private void Awake()
    {
        animator = GetComponent<Animator>();

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
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 rolling = new Vector3(horizontal * rollSpeed, vertical * rollSpeed, 0.0f);

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
            if(rollTime <= 0)
            {
                direction = 0;
                rollTime = startRollTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                DodgeRoll();
            }
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

    private void DodgeRoll()
    {

        if (Input.GetButtonDown("Dodge"))
        {
            rollTime -= Time.deltaTime;

            if(direction == 1)
            {
                Debug.Log("rolled left");
                rb.velocity = Vector2.left * rollSpeed;
                direction = 0;
            }else if(direction == 2){
                Debug.Log("rolled right");
                rb.velocity = Vector2.right * rollSpeed;
                direction = 0;
            }
            else if(direction == 3){
                Debug.Log("rolled up");
                rb.velocity = Vector2.up * rollSpeed;
                direction = 0;
            }
            else if(direction == 4){
                Debug.Log("rolled down");
                rb.velocity = Vector2.down * rollSpeed;
                direction = 0;
            }
        }
    }
}
