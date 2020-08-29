using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSFX : MonoBehaviour
{
    public static AudioManagerSFX instance;
    public static AudioSource theSource;
    public static AudioClip playerFootsteps, playerHIT, playerHITLamp, berimBAM, attackUP;
    public static AudioClip enemyGrowl, enemyBasicAttack, enemyStrongAttack, enemyHIT, enemyDie;
    public static AudioClip ballIsLife, ballHIT, crowdCheer, crowdClaps, dodge, doorOpen, healthPickup, lampBreak, pandeiro, typingText;
    public static float pitchRandom = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        ballHIT = Resources.Load<AudioClip>("Sounds/aud_ballHit1");
        ballIsLife = Resources.Load<AudioClip>("Sounds/aud_ballKick1");
        berimBAM = Resources.Load<AudioClip>("Sounds/aud_berimbauNote2");
        crowdCheer = Resources.Load<AudioClip>("Sounds/aud_crowdCheer1");
        crowdClaps = Resources.Load<AudioClip>("Sounds/aud_crowdClap1");
        dodge = Resources.Load<AudioClip>("Sounds/aud_dodge1");
        doorOpen = Resources.Load<AudioClip>("Sounds/aud_doorOpen");
        healthPickup = Resources.Load<AudioClip>("Sounds/aud_healthPickup");
        lampBreak = Resources.Load<AudioClip>("Sounds/aud_lampCrash1");
        pandeiro = Resources.Load<AudioClip>("Sounds/aud_pandeiro1");
        typingText = Resources.Load<AudioClip>("Sounds/aud_textBlip1");

        enemyBasicAttack = Resources.Load<AudioClip>("Sounds/aud_kick5");
        enemyHIT = Resources.Load<AudioClip>("Sounds/aud_enemyTakeDamage");
        enemyDie = Resources.Load<AudioClip>("Sounds/aud_enemyDies");

        playerFootsteps = Resources.Load<AudioClip>("Sounds/aud_footsteps1");
        playerHIT = Resources.Load<AudioClip>("Sounds/aud_kick4");
        playerHITLamp = Resources.Load<AudioClip>("Sounds/aud_lampHit1");
        attackUP = Resources.Load<AudioClip>("Sounds/berimbauPowerUP");
        
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
            case "dodge":
                theSource.PlayOneShot(dodge);
                break;
            case "kickEnemy":
                // theSource.PlayOneShot(berimBAM);
                theSource.PlayOneShot(playerHIT);
                theSource.volume = 1f;
                break;
            case "enemyHIT":
                theSource.PlayOneShot(enemyHIT);
                break;
            case "kickLamp":
                theSource.PlayOneShot(playerHIT);
                theSource.pitch = Random.Range(1.0f - pitchRandom, 1.0f + pitchRandom);
                theSource.PlayOneShot(playerHITLamp);
                break;
            case "lampBreak":
                theSource.clip = lampBreak;
                theSource.PlayDelayed(0.5f);
                break;
            case "ballIsLife":
                theSource.PlayOneShot(ballIsLife);
                break;
            case "ballHIT":
                theSource.PlayOneShot(ballHIT);
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
            case "attackUP":
                theSource.PlayOneShot(attackUP);
                break;
            case "healthUP":
                theSource.PlayOneShot(healthPickup);
                break;
            case "hoorah":
                theSource.loop = true;
                theSource.clip = crowdCheer;
                theSource.PlayOneShot(crowdClaps);
                break;
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
