using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int maxHealth = 25;
    int currentHealth;
    private Animator postAnim;
    private Transform thisLampPos;

    HealthPickup drop;
    public HealthPickup hp;

    public void Start()
    {
        currentHealth = maxHealth;
        postAnim = gameObject.GetComponent<Animator>();
        thisLampPos = gameObject.GetComponent<Transform>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        postAnim.SetTrigger("isHit");
        if (currentHealth <= 0)
        {
            postAnim.SetTrigger("isDead");
            Die();
        }
    }

    void Die()
    {
        AudioManagerSFX.PlaySound("lampBreak");

        //this is the only breakable object currently, so small healthpickup drop chance
        var r = Random.Range(0, 10);
        if (r > 9)
            drop = Instantiate<HealthPickup>(hp, transform.position, transform.rotation);

        this.enabled = false;

        Destroy(gameObject, 2f);
    }
}
