using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoYouWishToProceed : MonoBehaviour
{

    private bool withinRange;
    private bool hasStarted = false;
    public Animator doorMessage;
    public Collider2D doorCollider;

    public void Start()
    {
        doorMessage = GetComponentInChildren<Animator>();
        doorMessage.enabled = false;
        doorCollider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        if(withinRange && Input.GetKeyDown(KeyCode.F))
        {
            AreYouSure();
        }
        if (hasStarted && Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Yes!");
            OpenSesame();
        }
        else if (hasStarted && Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("NO.");
            Reset();
        }
    }

    // Test with two colliders, one set to onTrigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            withinRange = true;
            Debug.Log("Entering interactable area.");
        } // If the player enters the isTrigger collider of an interactable object
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            withinRange = false;
            Debug.Log("Now exiting interactable area.");
        } // If the player exits the isTrigger collider of an interactable object
    }

    private void AreYouSure()
    {
        hasStarted = true;
        doorMessage.enabled = true;
        doorMessage.SetBool("isOpen", true);
    }

    private void OpenSesame()
    {
        AudioManagerSFX.PlaySound("open");
        doorMessage.SetBool("isOpen", false);
        doorCollider.enabled = false;
        Destroy(this);
    }

    private void Reset()
    {
        doorMessage.SetBool("isOpen", false);
    }
}
