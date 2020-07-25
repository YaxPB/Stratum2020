using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overhead : MonoBehaviour
{
    int totalPool;
    int changedPool;

    public GameObject foos;
    public Enemy[] maxhealthPool;
    public Enemy[] globalPool;

    public HealthBar healthBar;
    public GameObject healthCanvas;

    //GameObject[] Enemies;

    bool counted;
    bool checkedHealth = false;

    void Start()
    {
        maxhealthPool = foos.GetComponentsInChildren<Enemy>();
        //healthCanvas.SetActive(true);
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
        }
        else
        {
            healthBar.SetMaxHealth(totalPool);
            healthCanvas.SetActive(true);
        }
        counted = true;

        globalPool = foos.GetComponentsInChildren<Enemy>();

        AdjustPool();
        checkedHealth = true;
    }

    //called to adjust the health pool after each enemy death
    void AdjustPool()
    {
        if (!checkedHealth)
        {
            foreach (var healthChange in globalPool)
            {
                changedPool += healthChange.currentHealth;
            }
        }
        else
        {
            checkedHealth = false;
            healthBar.SetHealth(changedPool);
            Debug.Log(changedPool);
        }

    }
}
