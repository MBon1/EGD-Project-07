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
    [SerializeField] float changeTime = 0;
    [SerializeField] GameObject player;
    [SerializeField] float distanceMultiplier = 1;

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
        float distanceFromPlayer = 1;

        if (player != null)
        {
            distanceFromPlayer = Mathf.Abs(maxRange * distanceMultiplier - DistanceFromPlayer());
            Debug.Log(distanceFromPlayer);
            distanceFromPlayer = Mathf.Clamp(distanceFromPlayer, 0, 1);
        }
        


        float loudness = detection.GetLoudnessFromMic() * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;

        if (changeTime <= 0)
        {
            light.range = Mathf.Lerp(0, maxRange, loudness);
        }
        else
        {
            light.range = Mathf.Lerp(light.range, Mathf.Lerp(0, maxRange, loudness), changeTime * Time.deltaTime);
        }


        light.range *= distanceFromPlayer;
    }

    float DistanceFromPlayer()
    {
        Vector3 pos = this.transform.position;
        pos.y = 0;
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0;
        return Vector3.Distance(pos, playerPos);
    }
}
