using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int maxHealth = 25;
    int currentHealth;
    private Animator postAnim;
    private Transform thisLampPos;


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
        Debug.Log("Lamp died!");
        AudioManagerSFX.PlaySound("lampBreak");
        this.enabled = false;

        Destroy(gameObject, 2f);
    }
}
