using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSFX : MonoBehaviour
{
    public static AudioManagerSFX instance;
    public static AudioSource theSource;
    public static AudioClip playerFootsteps, playerHit, playerHitLamp, berimBAM, doorOpen, typingText, lampBreak, pandeiro;
    public static AudioClip enemyGrowl, enemyBasicAttack, enemyStrongAttack, enemyHit, enemyDie;
    public static float pitchRandom = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        berimBAM = Resources.Load<AudioClip>("Sounds/aud_berimbauNote2");
        doorOpen = Resources.Load<AudioClip>("Sounds/aud_doorOpen");
        lampBreak = Resources.Load<AudioClip>("Sounds/aud_lampCrash1");
        pandeiro = Resources.Load<AudioClip>("Sounds/aud_pandeiro1");
        typingText = Resources.Load<AudioClip>("Sounds/aud_textBlip1");

        enemyBasicAttack = Resources.Load<AudioClip>("Sounds/aud_kick5");
        enemyHit = Resources.Load<AudioClip>("Sounds/aud_enemyTakeDamage");
        enemyDie = Resources.Load<AudioClip>("Sounds/aud_enemyDies");

        playerFootsteps = Resources.Load<AudioClip>("Sounds/aud_footsteps1");
        playerHit = Resources.Load<AudioClip>("Sounds/aud_kick4");
        playerHitLamp = Resources.Load<AudioClip>("Sounds/aud_lampHit1");
        
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
        theSource.volume = 0.8f;
        switch (theSFX)
        {
            case "run":
                theSource.pitch = Random.Range(1.0f - pitchRandom, 1.0f + pitchRandom);
                theSource.PlayOneShot(playerFootsteps); 
                break;
            case "kickEnemy":
                // theSource.PlayOneShot(berimBAM);
                theSource.PlayOneShot(playerHit);
                theSource.volume = 1f;
                theSource.clip = enemyHit;
                theSource.PlayDelayed(0.2f);
                break;
            case "kickLamp":
                theSource.PlayOneShot(playerHit);
                theSource.pitch = Random.Range(1.0f - pitchRandom, 1.0f + pitchRandom);
                theSource.PlayOneShot(playerHitLamp);
                break;
            case "lampBreak":
                theSource.clip = lampBreak;
                theSource.PlayDelayed(0.5f);
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
            case "basicAttack":
                theSource.volume = 1f;
                theSource.PlayOneShot(enemyBasicAttack);
                break;
            case "enemyDied":
                theSource.volume = 1f;
                theSource.PlayOneShot(enemyDie);
                break;
            case "berimBAM":
                theSource.PlayOneShot(pandeiro);
                theSource.pitch = 1.2f;
                theSource.PlayOneShot(berimBAM);
                break;
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
