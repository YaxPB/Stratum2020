using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    // Reference to the LifeForce Canvas that holds the HealthBar info
    private Canvas healthBar;
    public static bool isCombat;
    private Collider2D[] spawnPoints;
    private Collider2D[] hitEnemies;
    public Collider2D zoneWall;
    public LayerMask spawnLayers;
    public LayerMask enemyLayers;
    public float zoneX;
    public float zoneY;
    // public Animator nextZone;
    
    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>().GetComponentInParent<Canvas>();
        isCombat = false;
        // Detect all enemies within a combat area
        hitEnemies = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(zoneX, zoneY), 0, enemyLayers);
        spawnPoints = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(zoneX, zoneY), 0, spawnLayers);
    }

    private void Update()
    {
        // Because enemy objects are destroyed, if index 0 is null, it SHOULD be that the zone has been cleared
        // i.e. Check if there are any enemies left in a given combatZone
        if (hitEnemies.Length <= 0 && spawnPoints.Length <= 0)
        {
            Debug.Log("Beep boop");
            // If all enemies have been defeated, player may progress to next area
            zoneWall.enabled = false;
            // nextZone.SetTrigger("allClear");
            // Might rethink this next line so commented out for now
            Destroy(this.gameObject, 4f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isCombat = true;
            ActivateCombatMode();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Update isCombat boolean to false once player exits the combat zone
        isCombat = false;
        // Immediately stop playing the combat beat (NOTE: thinking of writing a quick fade out script)
        // AudioManagerBG.SwitchTrack("previous");
        // Tells the HealthBar to stop playing CombatMode animations **Should probably find an alternative to SendMessage like this
        HealthBar.instance.gameObject.SendMessage("StartBeat", false);
        // PlayerCombat.instance.gameObject.SendMessage("TimeToFight", false);
    }

    public void ActivateCombatMode()
    {
        // Enables the HealthBar to appear and activates its attached script
        healthBar.enabled = true;

        AudioManagerBG.SwitchTrack("combat");

        // Tells the HealthBar to start playing its CombatMode animations
        HealthBar.instance.gameObject.SendMessage("StartBeat", true);
        PlayerCombat.instance.gameObject.SendMessage("TimeToFight", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(zoneX, zoneY, 1));
    }
}
