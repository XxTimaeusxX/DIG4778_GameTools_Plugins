using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class Billboard : MonoBehaviour
{
   
    private ImageLoader imageLoader;
    
    private string[] imageUrl = new string[]
    {
        "https://platform.polygon.com/wp-content/uploads/sites/2/chorus/uploads/chorus_asset/file/15777911/snk_logo.0.0.1488963123.jpg?quality=90&strip=all&crop=7.7954545454545,0,84.409090909091,100",
        "https://www.rfkracing.com/wp-content/uploads/2022/02/Celsius-Announcement-Graphic-Website-2.png",  
        "https://static0.gamerantimages.com/wordpress/wp-content/uploads/2023/11/sega.jpg"
    };
    public int imageIndex = 0;

    void Start()
    {
        imageLoader = gameObject.AddComponent<ImageLoader>();

        // Ensure that the imageIndex is within the bounds of the imageUrls array
        if (imageIndex >= 0 && imageIndex < imageUrl.Length)
        {
            string imageUrls = imageUrl[imageIndex];

            // Load the image and apply it to the billboard
            imageLoader.GetWebImage(imageUrls, texture =>
            {
                // Apply the texture to the material of the object
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.mainTexture = texture;
                }
            });
        }
        else
        {
            Debug.LogError("Invalid image index for this billboard.");
        }

    }
}
