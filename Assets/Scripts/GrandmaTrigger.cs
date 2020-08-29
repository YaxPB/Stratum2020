using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaTrigger : MonoBehaviour
{
    public Dialogue theDialogue;
    public GameObject theCam;
    public GameObject granGran;
    private Transform grandmaPos;
    public Transform playerPos;

    private DialogueManager theDM;
    private Collider2D triggerZone;
    private Animator grandmaAnim;

    private void Start()
    {
        triggerZone = GetComponent<Collider2D>();
        theDM = FindObjectOfType<DialogueManager>();
        grandmaPos = granGran.transform;
        grandmaAnim = granGran.GetComponentInChildren<Animator>();
    }

    // Test with two colliders, one set to onTrigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(grandmaAnim != null)
            {
                grandmaAnim.SetBool("isSpeaking", true);
                granGran.GetComponentInChildren<ParticleSystem>().Play();
            }
            theCam.GetComponent<CameraFollow>().target = grandmaPos;
            TriggerDialogue();
            Debug.Log("Entering interactable area.");
        } // If the player enters the isTrigger collider of an interactable object
    }

    public void TriggerDialogue()
    {
        Debug.Log("Dialogue Triggered.");
        theDM.StartDialogue(theDialogue);
        theDM.CueGrandma(this);
    }

    public void ResetCam()
    {
        if(grandmaAnim != null)
        {
            grandmaAnim.SetBool("isSpeaking", false);
            granGran.GetComponentInChildren<ParticleSystem>().Stop();
        }
        // Reset camera to resume following player
        theCam.GetComponent<CameraFollow>().target = playerPos;
        // So the dialogue attached to this object can only be played once
        triggerZone.enabled = false;
    }

}
