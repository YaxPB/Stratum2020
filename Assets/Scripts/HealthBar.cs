using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    // because there should only ever be this one healthbar object at any given time
    // also so other scripts can reference this
    public static HealthBar instance;
    private Animator lives;
    private bool isCombat = false;

    private void Start()
    {
        instance = this;
        this.enabled = false;
        lives = GetComponentInChildren<Animator>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int newhealth)
    {
        slider.value = newhealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void AdjustLives(int count)
    {
        if (count == 2)
            lives.SetBool("2", true);
        else if (count == 1)
            lives.SetBool("1", true);
    }

    void StartBeat(bool theStart)
    {
        if (!theStart)
        {
            isCombat = false;
            //heartBeat.enabled = false;
            //heartBeat.SetBool("isCombat", false);
            return;
        }
        isCombat = true;
        //heartBeat.enabled = true;
        //heartBeat.SetBool("isCombat", true);
    }
}
