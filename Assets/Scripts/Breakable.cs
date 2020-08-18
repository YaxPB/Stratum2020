using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int maxHealth = 25;
    int currentHealth;
    private Animator postAnim;


    public void Start()
    {
        currentHealth = maxHealth;
        postAnim = gameObject.GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt anim
        // anim.SetTrigger("Hurt");
        postAnim.SetTrigger("isHit");
        if (currentHealth <= 0)
        {
            postAnim.SetTrigger("isDead");
            Die();
        }
    }

    void Die()
    {
        // theTarget.SetBool("CombatMode", false);
        Debug.Log("Lamp died!");

        //enemy gameobj is not destroyed, body is left behind
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 2f);
    }
}
