using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public string imageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Cat_August_2010-4.jpg/2560px-Cat_August_2010-4.jpg";
    private ImageLoader imageLoader;

    void Start()
    {
        imageLoader = gameObject.AddComponent<ImageLoader>();

        // Get the image and apply it to the billboard
        imageLoader.GetWebImage(imageUrl, texture =>
        {
            // Apply the texture to a material (e.g., a plane or cube)
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
            }
        });
    }
}
