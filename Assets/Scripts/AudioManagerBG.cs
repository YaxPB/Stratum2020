﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManagerBG : MonoBehaviour
{
    public static AudioManagerBG instance;
    public static AudioSource theSource;
    public static AudioSource secondLayer;
    public static AudioClip combatBass124, combatTheme, grandmaTheme, nightmareTheme, stratumTheme;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        combatBass124 = Resources.Load<AudioClip>("Sounds/aud_124bpmBass");
        combatTheme = Resources.Load<AudioClip>("Sounds/aud_berimbeatSnippet");
        grandmaTheme = Resources.Load<AudioClip>("Sounds/bg_nightmareWhoopie");
        nightmareTheme = Resources.Load<AudioClip>("Sounds/bg_nightmare1");
        stratumTheme = Resources.Load<AudioClip>("Sounds/bg_explorationPhase");
        theSource = GetComponent<AudioSource>();
        if(theSource == null)
        {
            theSource = gameObject.AddComponent<AudioSource>();
        }
        secondLayer = gameObject.AddComponent<AudioSource>();
        SwitchTrack("stratum");
    }

    public static void GetClipInfo()
    {
        string currentClip = theSource.clip.ToString();
    }

    // Call this method from any other GameObject
    public static void SwitchTrack(string themeName)
    {
        theSource.loop = true;
        theSource.volume = 0.8f;
        switch (themeName)
        {
            case "menu":
                // nothing yet 
                break;
            case "stratum":
                if (theSource.clip != stratumTheme)
                {
                    theSource.clip = stratumTheme;
                }
                theSource.Play();
                break;
            case "nightmare":
                if (theSource.clip != nightmareTheme)
                {
                    theSource.clip = nightmareTheme;
                }
                theSource.Play();
                break;
            case "grandma":
                if (theSource.clip != grandmaTheme)
                {
                    theSource.clip = grandmaTheme;
                }
                theSource.Play();
                break;
            case "combat":
                if(theSource.clip != combatTheme)
                {
                    theSource.clip = combatTheme;
                    // secondLayer.clip = combatBass124;
                }
                theSource.volume = 0.5f;
                // secondLayer.volume = 0.6f;
                theSource.Play();
                // secondLayer.Play();
                break;

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}