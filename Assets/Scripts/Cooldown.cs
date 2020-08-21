using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    public Image berimbauimageCooldown;
    public Image futbolimageCooldown;
    public float berimDown;
    public float futbolDown = 4f;
    public bool berimbauIsCooling;
    public bool ballIsCooling;
    
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Q))
        {
            berimbauIsCooling = true;
        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ballIsCooling = true;
        }

        if (berimbauIsCooling)
        {
            berimbauimageCooldown.fillAmount += 1 / berimDown * Time.deltaTime;
            if(berimbauimageCooldown.fillAmount >= 1)
            {
                berimbauimageCooldown.fillAmount = 0;
                berimbauIsCooling = false;
            }
        }

        if (ballIsCooling)
        {
            futbolimageCooldown.fillAmount += 1 / futbolDown * Time.deltaTime;
            if (futbolimageCooldown.fillAmount >= 1)
            {
                futbolimageCooldown.fillAmount = 0;
                ballIsCooling = false;
            }
        }
    }
}
