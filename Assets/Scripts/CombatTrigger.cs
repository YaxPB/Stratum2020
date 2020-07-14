using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public Camera mainCam;
    public Camera combatCam;

    public bool isCombat;
    private Animator theBeats;
    public AudioSource capBeat;

    // Start is called before the first frame update
    void Start()
    {
        combatCam.enabled = false;
        isCombat = false;
        theBeats = GetComponentInChildren<Animator>();
        capBeat = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if there are any enemies in the surrounding area
        // if not, trigger NextArea UI 
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
        isCombat = false;
        capBeat.Stop();
        mainCam.enabled = true;
        combatCam.enabled = false;
    }

    public void ActivateCombatMode()
    {
        capBeat.Play();
        mainCam.enabled = false;
        combatCam.enabled = true;

    }
}
