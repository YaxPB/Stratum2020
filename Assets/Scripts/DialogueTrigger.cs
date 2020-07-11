using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue theDialogue;
    private bool withinRange;
    // private bool hasStarted;

    public void Update()
    {
        if(withinRange && Input.GetKeyDown(KeyCode.F))
        {
            // Immediately set to false so the next button press doesn't reset dialogue
            withinRange = false;

            // Triggers the start of a conversation
            TriggerDialogue();
        }

    }

    // Test with two colliders, one set to onTrigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            withinRange = true;
            Debug.Log("Entering interactable area.");
        } // If the player enters the isTrigger collider of an interactable object
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            withinRange = false;
            Debug.Log("Now exiting interactable area.");
        } // If the player exits the isTrigger collider of an interactable object
    }

    public void TriggerDialogue()
    {
        Debug.Log("Dialogue Triggered.");
        FindObjectOfType<DialogueManager>().StartDialogue(theDialogue);
    }
}
