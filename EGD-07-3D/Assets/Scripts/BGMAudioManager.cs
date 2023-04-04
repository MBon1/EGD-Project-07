using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Implementation from https://www.youtube.com/watch?v=PHa5SNe1Mmk
public class BGMAudioManager : MonoBehaviour
{
    private static BGMAudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public static void DestroyInstance()
    {
        if (instance != null)
            SceneManager.MoveGameObjectToScene(instance.gameObject, SceneManager.GetActiveScene());
    }
}
