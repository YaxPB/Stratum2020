using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSFX : MonoBehaviour
{
    public static AudioManagerSFX instance;
    public static AudioSource theSource;
    public static AudioClip playerFootsteps, playerHit, berimBAM, doorOpen, typingText;
    public static float pitchRandom = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        doorOpen = Resources.Load<AudioClip>("Sounds/aud_doorOpen");
        playerFootsteps = Resources.Load<AudioClip>("Sounds/aud_footsteps1");
        playerHit = Resources.Load<AudioClip>("Sounds/aud_kick4");
        berimBAM = Resources.Load<AudioClip>("Sounds/aud_berimbauNote3");
        typingText = Resources.Load<AudioClip>("Sounds/aud_textBlip1");
        theSource = GetComponent<AudioSource>();
        if(theSource == null)
        {
            theSource.gameObject.AddComponent<AudioSource>();
        }
    }


    public static void PlaySound(string theSFX)
    {
        theSource.loop = false;
        theSource.pitch = 1.0f;
        switch (theSFX)
        {
            case "run":
                theSource.pitch = Random.Range(1.0f - pitchRandom, 1.0f + pitchRandom);
                theSource.PlayOneShot(playerFootsteps);
                
                break;
            case "kick":
                theSource.PlayOneShot(berimBAM);
                theSource.PlayOneShot(playerHit);
                break;
            case "open":
                theSource.PlayOneShot(doorOpen);
                break;
            case "dialogue":
                theSource.Stop();
                if(theSource.clip != typingText)
                {
                    theSource.clip = typingText;
                }
                theSource.loop = true;
                theSource.Play();
                break;

        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
