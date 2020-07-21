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
    private bool isCombat = false;

    private void Start()
    {
        instance = this;
        this.enabled = false;
        heartBeat = GetComponentInChildren<Animator>();
        //Select the instance of AudioProcessor and pass a reference
        //to this object
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);
        processor.onSpectrum.AddListener(onSpectrum);
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
            isCombat = false;
            heartBeat.enabled = false;
            heartBeat.SetBool("isCombat", false);
            return;
        }
        isCombat = true;
        heartBeat.enabled = true;
        heartBeat.SetBool("isCombat", true);
    }

    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    void onOnbeatDetected()
    {
        Debug.Log("Beat!!!");
    }

    //This event will be called every frame while music is playing
    void onSpectrum(float[] spectrum)
    {
        //The spectrum is logarithmically averaged
        //to 12 bands

        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }
}
