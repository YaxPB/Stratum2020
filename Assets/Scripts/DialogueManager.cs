using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;       // Holds the name of the NPC or object being interacted with
    public Text dialogueText;   // Holds the text to be displayed upon interaction
    public Animator animator;   // The animator in charge of enter/exit animation of the dialogue box
    private bool convoStarted;  // Boolean to determine if a conversation has started
    private int pressedTimes;   // Number of times the F key is pressed

    // Uses a Queue (of Strings) data structure (FIFO)
    public Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        convoStarted = false;
    }

    // ONLY TOOK ME TWO FULL [working] DAYS BUT I FINALLY FUCKING FIXED IT ON 5/30/20 @ 12:38 AM
    private void Update()
    {
        // THE DUMB BANDAID IF-STATEMENT THAT FIXED THIS FOR OUR PROJECT ON 7/8/20 @ 7:47 PM 
        if (Input.GetKeyDown(KeyCode.F))
        {
            pressedTimes += 1;
        }

        // Checks if the conversation has been started and player presses 'F' to continue
        if(convoStarted && Input.GetKeyDown(KeyCode.F))
        {
            if(pressedTimes > 1)
            {
                // Run the DisplayNextSentence() method
                DisplayNextSentence();
            } // if the first sentence has been displayed

        } // if the conversation has started and user hits F key

    } // Update

    public void StartDialogue(Dialogue theDialogue)
    {
        MovePlayer.instance.canMove = false;
        // Activates the animator that brings up the DialogueBox
        animator.SetBool("isOpen", true);
        // Just a boolean to determine whether or not a conversation has started
        convoStarted = true;

        Debug.Log("Starting conversation with " + theDialogue.name);
        // Sets and displays the NPC name
        nameText.text = theDialogue.name;

        // Makes sure the Queue is empty before starting conversation
        sentences.Clear();

        foreach(string sentence in theDialogue.sentences)
        {
            // Queue each sentence in the target object/NPC
            sentences.Enqueue(sentence);
        } 

        // Runs the DisplayNextSentence without button prompt to display the first queued sentence
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            convoStarted = false;
            return;
        } // if there are no more sentences to display

        Debug.Log("Current count is " + sentences.Count);

        // Pop the bottom (first) sentence off the queue
        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);

        // Makes sure the previous animation stops before starting a new one
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {

        dialogueText.text = "";
        AudioManagerSFX.PlaySound("dialogue");
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // 1 frame buffer time between each letter rendering
            yield return null;
        }
        AudioManagerSFX.theSource.Stop();
    }

    void EndDialogue()
    {
        AudioManagerSFX.theSource.Stop();
        MovePlayer.instance.canMove = true;
        // Closes the dialogue box and ends the conversation
        Debug.Log("End of conversation.");
        animator.SetBool("isOpen", false);
        // Reset variable for future conversations
        pressedTimes = 0;
    }

}
