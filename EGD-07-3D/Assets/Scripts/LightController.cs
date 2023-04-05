using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour
{
    new Light light;
    float maxRange = 10;

    [SerializeField] AudioLoudnessDetection detection;

    [Header("Audio Loudness Parameters")]
    [SerializeField] float loudnessSensibility = 1;
    [SerializeField] float threshold = 0f;

    private void Awake()
    {
        light = this.GetComponent<Light>();
        maxRange = light.range;
        light.range = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detection.GetLoudnessFromMic() * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;

        light.range = Mathf.Lerp(0, maxRange, loudness);
    }
}
