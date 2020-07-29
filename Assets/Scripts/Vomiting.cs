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

    public static Vomiting CreateVomit(Vomiting vomit, Transform at, Vector2 direction)
    {
        Vomiting flight = Instantiate<Vomiting>(vomit, at.position, at.rotation);
        flight.rb.velocity = direction * flight.speed;
        return flight;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
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

