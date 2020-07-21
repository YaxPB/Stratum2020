using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
            s.source.loop = s.loop;
		}
    }
    void Update()
    {
        sounds[0].source.volume = sounds[0].volume;
        sounds[0].source.pitch = sounds[0].pitch;
        sounds[0].source.loop = sounds[0].loop;

        sounds[1].source.volume = sounds[1].volume;
        sounds[1].source.pitch = sounds[1].pitch;

        sounds[2].source.volume = sounds[2].volume;
        sounds[2].source.pitch = sounds[2].pitch;
        sounds[2].source.loop = sounds[2].loop;

        sounds[3].source.volume = sounds[3].volume;
        sounds[3].source.pitch = sounds[3].pitch;
        sounds[3].source.loop = sounds[3].loop;

        sounds[10].source.volume = sounds[10].volume;
        sounds[10].source.pitch = sounds[10].pitch;
    }

    void Start ()
	{
		Play("Theme");
        Play("Animals");
	}

    public void Deletus()
    {
        Destroy(sounds[0].source);
    }

    public void Play (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
		{
            Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
        s.source.Play();
	}
}
