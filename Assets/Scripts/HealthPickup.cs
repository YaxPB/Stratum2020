using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    PlayerCombat pc;
    int healthBonus = 20;
    HealthBar healthBar;

    // Start is called before the first frame update
    void Awake()
    {
        pc = FindObjectOfType<PlayerCombat>();
        healthBar = GameObject.Find("LifeForce").GetComponentInChildren<HealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //add healthpickup sound effect
            pc.currentHealth += healthBonus;
            healthBar.SetHealth(pc.currentHealth);
            Destroy(gameObject);
        }
    }
}
