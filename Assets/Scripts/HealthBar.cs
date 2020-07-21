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
    private Animator heartBeat;

    private void Start()
    {
        instance = this;
        this.enabled = false;
        heartBeat = GetComponentInChildren<Animator>();
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

    void StartBeat(bool startOrEnd)
    {
        if (!startOrEnd)
        {
            heartBeat.enabled = false;
            heartBeat.SetBool("isCombat", false);
            return;
        }
        heartBeat.enabled = true;
        heartBeat.SetBool("isCombat", true);
    }
}
