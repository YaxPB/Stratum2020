using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overhead : MonoBehaviour
{
    int totalPool;
    int count;
    int changingPool;
    
    public int[] maxhealthPool;

    public HealthBar healthBar;
    public GameObject healthCanvas;
    public Animator waveInfo;

    public bool readyToDecrease;

    /*private void Start()
    {
        healthCanvas.SetActive(false);
    }*/

    public void SetOverhead(WaveSpawner spawner, int numWaves)
    {
        if(spawner.waves[0] == null)
        {
            Debug.Log("Oops. No wave here.");
            return;
        }
        // Number of Waves * Count * Number of SpawnPoints = total number of enemies
        count = numWaves * spawner.waves[0].count * spawner.spawnPoints.Length;
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
        }
        changingPool -= damage;
        Debug.Log(changingPool);
        healthBar.SetHealth(changingPool);
    }
}
