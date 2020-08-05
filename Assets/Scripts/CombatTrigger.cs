using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    // For camera switching
    public Camera mainCam;
    public Camera combatCam;

    // Reference to the LifeForce Canvas that holds the HealthBar info
    public Canvas healthBar;
    public static bool isCombat;

    // Thinking about initializing a counter to hold numEnemiesLeft
    // Every time an enemy is instantiated, use SendMessage to invoke a function
    // that just adds 1 to counter; minus 1 every time player defeats an enemy
    // Once the counter hits zero, activate NextZoneUI 
    // Need to go through Yax's Respawn/Enemy Spawn script to figure out approach

    // Start is called before the first frame update
    void Start()
    {
        combatCam.enabled = false;
        isCombat = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isCombat = true;
            ActivateCombatMode();
        }
        // Lock camera to focus on scene
        // Enable combatMusic and rhythm UI
        // Communicates with event camera or some kind of state change? 
        // Considering an OnTriggerExit2D that turns off CombatMode
        // i.e. collider stretches across the length of the entire battleground
        // Maybe add a door with an "Are you sure you want to proceed?" prompt
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Update isCombat boolean to false once player exits the combat zone
        isCombat = false;
        // Immediately stop playing the combat beat (NOTE: thinking of writing a quick fade out script)
        AudioManagerBG.SwitchTrack("previous");
        // Turns the main camera (+CamFollow) back on
        mainCam.enabled = true;
        // Turns off the scene-locked combatCam
        combatCam.enabled = false;
        // Tells the HealthBar to stop playing CombatMode animations
        HealthBar.instance.gameObject.SendMessage("StartBeat", false);
        PlayerCombat.instance.gameObject.SendMessage("TimeToFight", false);
    }

    public void ActivateCombatMode()
    {
        // Enables the HealthBar to appear and activates its attached script
        healthBar.enabled = true;

        AudioManagerBG.SwitchTrack("combat");
        // Eventually change the next 2 lines to invoke something like CamFollow.CombatMode
        // Disables the main camera
        mainCam.enabled = false;
        // Switches to a combatCam that locks onto the entirety of the current combat area
        combatCam.enabled = true;
        // Tells the HealthBar to start playing its CombatMode animations
        HealthBar.instance.gameObject.SendMessage("StartBeat", true);
        PlayerCombat.instance.gameObject.SendMessage("TimeToFight", true);
    }
}
