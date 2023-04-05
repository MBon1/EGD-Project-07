using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    private Material material;
    public Shader shader;

    // Start is called before the first frame update
    void Start()
    {
        material = new Material(shader);
    }

    // Update is called once per frame
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
