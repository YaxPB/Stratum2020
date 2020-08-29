using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overhead : MonoBehaviour
{
    int totalPool = 0;
    int count = 0;
    int changingPool = 0;
    int enemyPerWave = 0;
    
    public int[] maxhealthPool;

    public EnemyHealthBar healthBar;
    public GameObject healthCanvas;
    public Animator waveInfo;

    public bool readyToDecrease;

    // Changed to reflect overall health per CombatZone (WaveSpawner area)
    public void SetOverhead(WaveSpawner spawner, int numWaves)
    {
        enemyPerWave = 0;
        count = 0;
        if (spawner.waves[0] == null || numWaves < 1)
        {
            Debug.Log("Oops. No wave here.");
            return;
        } // if there are no waves for whatever reason

        for (int i = 0; i < spawner.waves.Length; i++)
        {
            // The number of enemies per wave = Wave.count * number of SpawnPoints
            enemyPerWave = spawner.waves[i].count * spawner.spawnPoints.Length;
            // Add value of enemyPerWave to total count value
            count += enemyPerWave;
            Debug.Log("There are " + enemyPerWave + " enemies in wave " + (i + 1) + ".");
        } // for iterating through each wave's info in the given WaveSpawner

        Debug.Log("Count is " + count);
        totalPool = count * 100;
        changingPool = totalPool;

        Debug.Log("Total enemy health is: " + totalPool);
        healthBar.SetMaxHealth(totalPool);
        waveInfo.SetBool("OverheadUp", true);
        //healthCanvas.SetActive(true);
        readyToDecrease = true;
    }

    void Update()
    {
        if (changingPool <= 0)
        {
            waveInfo.SetBool("OverheadUp", false);
            //healthCanvas.SetActive(false);
            readyToDecrease = false;
        }
    }

    //called to adjust the health pool after each enemy death
    public void AdjustPool(int damage)
    {
        if(changingPool - damage <= 0)
        {
            Debug.Log("CombatZone complete.");
            readyToDecrease = false;
        }
        changingPool -= damage;
        Debug.Log(changingPool);
        healthBar.SetHealth(changingPool);
    }
}
