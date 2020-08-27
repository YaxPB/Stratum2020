using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    public Image berimbauimageCooldown;
    public Image futbolimageCooldown;
    public Animator berimanim;
    public Animator futbolanim;

    public float berimDown;
    public float futbolDown;
    public bool berimbauIsCooling;
    public bool ballIsCooling;
    
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Q))
        {
            berimbauIsCooling = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ballIsCooling = true;
        }*/ 

        if (berimbauIsCooling)
        {
            //find actual berimbau cooldown time and scale animation
            berimanim.SetBool("NoteCooling", true);
            berimbauimageCooldown.fillAmount += 1 / berimDown * Time.deltaTime;
            if(berimbauimageCooldown.fillAmount >= 1)
            {
                berimanim.SetBool("NoteCooling", false);
                berimbauimageCooldown.fillAmount = 0;
                berimbauIsCooling = false;
            }
        }

        if (ballIsCooling)
        {
            //find acutual futbol cooldown time and scale 
            futbolanim.SetBool("BallCooling", true);
            futbolimageCooldown.fillAmount += 1 / futbolDown * Time.deltaTime;
            if (futbolimageCooldown.fillAmount >= 1)
            {
                futbolanim.SetBool("BallCooling", false);
                futbolimageCooldown.fillAmount = 0;
                ballIsCooling = false;
            }
        }
    }
}
