using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float runSpeed = 0.5f;
    public float rollSpeed = 5f;
    private Vector3 slideDir;
    private Rigidbody2D playerRB;
    private Vector3 rolling;

    float horizontal;
    float vertical;
    bool facingRight;

    bool canMove;

    Animator animator;
    
    //bool isJumping;
    bool isRolling;

    // Sound variables
    private float stepOffset;
    private AudioSource playerFX;


    private void Awake()
    {
        canMove = true;
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerFX = GetComponent<AudioSource>();

        /*rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.Sleep();*/
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
        if((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && stepOffset == 0)
        {
            // Can change this to adjust speed of footstep sounds
            stepOffset = 0.25f;
        }

        /*if (!canMove)
        {
            playerRB.velocity = Vector2.zero;
            return;
        }*/

        if (Input.GetButtonDown("Dodge"))
        {
            isRolling = true;
            Debug.Log("dodged");
            DodgeRoll(rolling);
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

    private void DodgeRoll(Vector3 num)
    {
        if (isRolling)
        {
            
            //transform.position = transform.position + num * Time.deltaTime;
            isRolling = false;
        }
    }
}
