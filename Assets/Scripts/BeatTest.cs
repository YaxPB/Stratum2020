﻿using UnityEngine;

public class BeatTest : MonoBehaviour
{
    public AudioSource theBeats;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;

    public GameObject dancingSprite;
    public float sizeFactor = 1;

    public float minSize = 0;
    public float maxSize = 500;

    private void Awake()
    {
        clipSampleData = new float[sampleDataLength];
    }

    private void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if(currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            theBeats.clip.GetData(clipSampleData, theBeats.timeSamples);
            clipLoudness = 0f;
            foreach(var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength;

            clipLoudness *= sizeFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);
            dancingSprite.transform.localScale = new Vector3(clipLoudness, clipLoudness, clipLoudness);
        }
    }
}
