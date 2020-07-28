using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vomiting : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 15;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    MovePlayer mp;
    PlayerCombat pc;

    public bool didHit { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        pc = hitInfo.GetComponent<PlayerCombat>();
        mp = hitInfo.GetComponent<MovePlayer>();

        if (pc != null)
        {
            didHit = true;
            //mp.runSpeed = mp.runSpeed / 2;
            //Instantiate(impactEffect, transform.position, transform.rotation);
            pc.TakeDamage(damage);
            StartCoroutine(Normal(mp, pc));
        }
    }
    
    private IEnumerator Normal(MovePlayer mp,PlayerCombat pc)
    {
        yield return new WaitForSeconds(2f);

        //mp.runSpeed = pc.regSpeed;
        Destroy(gameObject);

    }
}

