using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float runSpeed = 1f;
    public float jumpForce = 300f;

    float horizontal;
    float vertical;
    bool facingRight;

    Animator animator;

    Rigidbody2D rigidBody;
    float axisY;
    bool isJumping;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
        transform.position = transform.position + movement * Time.deltaTime;
        Flip(horizontal);

        if(transform.position.y <= axisY)
        {
            OnLanding();
        }

        /*Input.GetButtonDown("Jump") && !isJumping)
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

    void OnLanding()
    {
        isJumping = false;
        rigidBody.gravityScale = 0f;
        rigidBody.Sleep();
        axisY = transform.position.y;
        //animator.SetBool("isJumping", false);
    }
}
