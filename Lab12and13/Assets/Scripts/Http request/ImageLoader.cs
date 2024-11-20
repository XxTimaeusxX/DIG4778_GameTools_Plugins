using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    // Dictionary to store the downloaded textures (cache)
    private Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

    // Method to download or get the cached image
    public void GetWebImage(string imageUrl, Action<Texture2D> callback)
    {
        // Check if the image is already cached
        if (imageCache.ContainsKey(imageUrl))
        {
            // If cached, return the stored image
            callback(imageCache[imageUrl]);
        }
        else
        {
            // If not cached, download the image
            StartCoroutine(DownloadImage(imageUrl, callback));
        }
    }

    // Method to download the image from the web
    private IEnumerator DownloadImage(string imageUrl, Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        // Check for errors in the web request
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download image: " + request.error);
            yield break;
        }

        // Get the texture from the web request response
        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        // Cache the texture for future use
        imageCache[imageUrl] = texture;

        // Call the callback with the downloaded texture
        callback(texture);
    }
}
