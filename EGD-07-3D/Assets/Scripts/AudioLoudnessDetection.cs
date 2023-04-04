using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int micIndex = 0;
    string micName;
    private AudioClip micClip;

    public int sampleWindow = 1024;   // 64
    [Header("Sampling Method")]
    public bool useMaxVolume = true;

    private void OnEnable()
    {
        StartMic();
    }

    private void OnDisable()
    {
        StopMic();
    }

    private void OnDestroy()
    {
        StopMic();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartMic()
    {
        micName = Microphone.devices[micIndex];
        micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);        // Look more into AudioSettings
    }

    void StopMic()
    {
        Microphone.End(micName);
    }

    public float GetLoudnessFromMic()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[micIndex]), micClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        
        if (startPosition < 0)
        {
            return 0;
        }
        
        float[] wavData = new float[sampleWindow];
        clip.GetData(wavData, startPosition);


        if (useMaxVolume) {
            // Compute Loudest peak in the audio sample
            float levelMax = 0;
            for (int i = 0; i < sampleWindow; i++)
            {
                // Value of wave data ranges from -1 to 1
                // 0 = No Sound
                float wavePeak = Mathf.Abs(wavData[i]);
                if (levelMax < wavePeak)
                {
                    levelMax = wavePeak;
                }
            }

            return levelMax;
        }
        else
        {
            // Compute Loudness (average amplitude)
            float totalLoudness = 0;
            for (int i = 0; i < sampleWindow; i++)
            {
                // Value of wave data ranges from -1 to 1
                // 0 = No Sound
                totalLoudness += Mathf.Abs(wavData[i]);
            }
            
            return totalLoudness / sampleWindow;
        }
    }
}
