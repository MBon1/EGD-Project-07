using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterAudio : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip clip;

    [SerializeField] AudioLoudnessDetection audioDetection;

    // Start is called before the first frame update
    void Start()
    {
        /*clip = audioDetection.micClip;
        audioSource.clip = clip;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
