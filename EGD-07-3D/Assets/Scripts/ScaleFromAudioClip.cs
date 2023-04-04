using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detection;

    public float loudest = 0;

    public float loudnessSensibility = 1;
    public float threshold = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detection.GetLoudnessFromAudioClip(source.timeSamples, source.clip) * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;

        if (loudness > loudest)
        {
            loudest = loudness;
        }

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        Debug.Log(loudness);
    }
}
