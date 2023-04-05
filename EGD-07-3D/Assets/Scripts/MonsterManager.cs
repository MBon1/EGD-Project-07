using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bot))]
public class MonsterManager : MonoBehaviour
{
    Bot bot;

    [SerializeField] AudioLoudnessDetection detection;

    [Header("Audio Loudness Parameters")]
    [SerializeField] float loudnessSensibility = 1;
    [SerializeField] float threshold = 0f;
    float lastLoudness = 0;

    [Header("Pursue Duration Parameters")]
    float lingeringPursueStartTime;
    [SerializeField] float lingeringPursueTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        bot = this.GetComponent<Bot>();
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detection.GetLoudnessFromMic() * loudnessSensibility;

        if (loudness <= threshold)
            loudness = 0;

        if (loudness > 0 || Time.time - lingeringPursueStartTime < lingeringPursueTime)
        {
            bot.operation = Bot.Operation.Pursue;
        }
        else
        {
            if (lastLoudness > 0)
            {
                lingeringPursueStartTime = Time.time;
            }
            else
            {
                bot.operation = Bot.Operation.Wander;
            }
        }

        lastLoudness = loudness;
    }
}
