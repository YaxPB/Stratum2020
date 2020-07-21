using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioSource theSource;
    public static AudioClip playerFootsteps, playerHit, berimBAM;
    public static float pitchRandom = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        playerFootsteps = Resources.Load<AudioClip>("Sounds/aud_footsteps1");
        playerHit = Resources.Load<AudioClip>("Sounds/aud_kick4");
        berimBAM = Resources.Load<AudioClip>("Sounds/aud_berimbauNote2");
        theSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string theSFX)
    {
        switch (theSFX)
        {
            case "walk":
                theSource.pitch = Random.Range(1.0f - pitchRandom, 1.0f + pitchRandom);
                theSource.PlayOneShot(playerFootsteps);
                break;
            case "kick":
                theSource.PlayOneShot(berimBAM);
                theSource.PlayOneShot(playerHit);
                break;
            

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
