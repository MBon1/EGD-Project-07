using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleHP : MonoBehaviour
{
    public Image healthBar;
    public float scaleTime = 1;

    public void ScaleHPBar(float newAmt, float total)
    {
        healthBar.fillAmount = Mathf.Clamp(newAmt, 0, total) / total;
    }

    public IEnumerator Scale(float amount, float total)
    {
        float time = 0;
        float startTime = Time.time;
        while (time - startTime < scaleTime)
        {
            time = Time.time;
            ScaleHPBar(amount - amount * ((time - startTime) / scaleTime), total);
            yield return null;
        }
        ScaleHPBar(amount, total);
    }
}
