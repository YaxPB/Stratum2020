using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float runSpeed = 1f;
    public float rollWindup = 0.1f;
    private Vector3 slideDir;

    float horizontal;
    float vertical;
    bool facingRight;

    Animator animator;
    
    private Rigidbody2D rb;
    public float rollSpeed;

    [SerializeField]
    private float rollTime;

    public float startRollTime;

    [SerializeField]
    private int direction;
    bool isDodging;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

        if (rollTime > 0)
        {
            rollTime -= Time.deltaTime;
        }

        if (!isDodging)
        {
            if (direction != 0)
            {
                //start coroutine
                if (Input.GetButtonDown("Dodge"))
                {
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
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
        transform.position = transform.position + movement * Time.deltaTime;
        Flip(horizontal);
    }

    private void Flip(float horizontal)
    {
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)   
        {
            facingRight = !facingRight;

            /*Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;*/
            Debug.Log("flipping");

            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator BeginDodgeRoll()
    {
        Debug.Log("im rolling");

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
            //direction = 0;
            rollTime = startRollTime;
            rb.velocity = Vector2.zero;
        }
    }
}
