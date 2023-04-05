using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class AudioMicController : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioLoudnessDetection detection;

    [Header("Audio Loudness Parameters")]
    [SerializeField] float loudnessSensibility = 1;
    [SerializeField] float threshold = 0f;
    [SerializeField] float changeTime = 1;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detection.GetLoudnessFromMic() * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;

        audioSource.volume = Mathf.Lerp(audioSource.volume, Mathf.Lerp(0, 1, loudness), changeTime * Time.deltaTime);
    }
}
