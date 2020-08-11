using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overhead : MonoBehaviour
{
    int totalPool;
    int count;
    int changingPool;
    int spawner = 0;

    //public GameObject[] enemies;
    
    public int[] maxhealthPool;
    public WaveSpawner[] waveComs;

    public EnemyHealthBar healthBar;
    public GameObject healthCanvas;

    public bool readyToDecrease;

    private void Start()
    {
        healthCanvas.SetActive(false);
        waveComs = FindObjectsOfType<WaveSpawner>();
    }

    public void SetOverhead(int wave)
    {
        if (!waveComs[spawner].completed)
        {
            count = waveComs[spawner].waves[wave].count;
            totalPool = count * 100;
            changingPool = totalPool;
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

        if (waveComs[spawner].completed)
            spawner++;
    }

    //called to adjust the health pool after each enemy death
    public void AdjustPool(int damage)
    {
        changingPool -= damage;
        Debug.Log(changingPool);
        healthBar.SetHealth(changingPool);
    }
}
