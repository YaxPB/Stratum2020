﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overhead : MonoBehaviour
{
    int totalPool;
    int changingPool;

    //public GameObject foos;
    public Enemy[] maxhealthPool;
    public GameObject[] enemies;

    public HealthBar healthBar;
    public GameObject healthCanvas;
    public WaveSpawner ws;

    bool counted;
    bool set;

    void Start()
    {
        if (ws.allSpawned)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                maxhealthPool[i] = enemies[i].GetComponent<Enemy>();
            }
        }
    }
    
    void Update()
    {
        //sets the waves entire health pool
        if (!counted)
        {
            foreach (var health in maxhealthPool)
            {
                totalPool += health.currentHealth;
            }
            changingPool = totalPool;
        }
        else
        {
            if (!set)
            {
                //Debug.Log(totalPool);
                healthBar.SetMaxHealth(totalPool);
                healthCanvas.SetActive(true);
                set = true;
            }
        }
        counted = true;

        if(changingPool <= 0)
            healthCanvas.SetActive(false);

        //globalPool = foos.GetComponentsInChildren<Enemy>();
    }

    //called to adjust the health pool after each enemy death
    public void AdjustPool(int damage)
    {
        changingPool -= damage;
        Debug.Log(changingPool);
        healthBar.SetHealth(changingPool);
    }
}
