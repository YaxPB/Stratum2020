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

    public bool readyToDecrease;

    private void Start()
    {
        healthCanvas.SetActive(false);
    }

    public void SetOverhead(WaveSpawner spawner,int wave)
    {
        if (!spawner.completed)
        {
            count = spawner.waves[wave].count;
            totalPool = count * 100;
            changingPool = totalPool;
            Debug.Log(totalPool);
            healthBar.SetMaxHealth(totalPool);
            healthCanvas.SetActive(true);
            readyToDecrease = true;
        }
    }
    
    void Update()
    {
        if (healthBar.slider.value <= 0)
        {
            healthCanvas.SetActive(false);
            readyToDecrease = false;
        }
    }

    //called to adjust the health pool after each enemy death
    public void AdjustPool(int damage)
    {
        changingPool -= damage;
        Debug.Log(changingPool);
        healthBar.SetHealth(changingPool);
    }
}
