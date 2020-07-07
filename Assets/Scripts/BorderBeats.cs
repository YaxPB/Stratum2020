using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBeats : MonoBehaviour
{
    public Animator borderAnimTL;
    public Animator borderAnimTR;
    public AudioSource justBeatIt;
    private bool combatMode;
    private bool isHit;
    public GameObject theLeft;
    public GameObject theRight;
    private CircleCollider2D borderLeft;
    private CircleCollider2D borderRight;

    public static BorderBeats instance;
    TheBeats beatIt;

    // should we map abilities to KOL; ? (movement to WASD)
    private KeyCode capPunch = KeyCode.O;
    private KeyCode capKick = KeyCode.K;
    private KeyCode capDodge = KeyCode.L;
    private KeyCode capBerimbau = KeyCode.Semicolon;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        borderLeft = theLeft.GetComponent<CircleCollider2D>();
        borderRight = theRight.GetComponent<CircleCollider2D>();
        borderAnimTL = theLeft.GetComponent<Animator>();
        borderAnimTR = theRight.GetComponent<Animator>();
        combatMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!combatMode && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Combat Mode Start");
            justBeatIt.Play();
            borderAnimTL.SetBool("CombatMode", true);
            borderAnimTR.SetBool("CombatMode", true);
            borderAnimTL.enabled = true;
            borderAnimTR.enabled = true;
            combatMode = true;

        }
        if (Input.GetKeyDown(capPunch) || Input.GetKeyDown(capKick) || Input.GetKeyDown(capDodge) || Input.GetKeyDown(capBerimbau))
        {
            Debug.Log("Key Pressed");
            BeatGenerator.instance.SpawnBeat();

        }

    }

    public void HitAnim()
    {
        borderAnimTL.SetBool("PunchyPunchy", true);
        borderAnimTR.SetBool("PunchyPunchy", true);
    }

}
